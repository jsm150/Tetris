using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace Tetris
{
    public class Tetris
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 20;
        private const int DELAY = 360;
        private static readonly Semaphore _drawLocker = new Semaphore(1, 1);
        protected static readonly Random _random = new Random();
        protected readonly TetrisBlock _block = new TetrisBlock();
        private readonly List<int> _clearLineList = new List<int>();
        private readonly MetroPanel _panel;
        private readonly KeyboardSetting _keyboardSetting;
        private readonly object _locker = new object();
        protected readonly int[,] _tetrisBoard = new int[HEIGHT, WIDTH];
        protected readonly Label _lblScore;
        private int _combo;
        protected int _currentX;
        protected int _currentY = -1;

        public long Score { get; private set; }
        public int PlayerId { get; }
        public bool GamePlaying { get; set; } = true;

        public Tetris(MetroPanel p, Label lblScore, KeyboardSetting key, int id)
        {
            PlayerId = id;
            _panel = p;
            _lblScore = lblScore;
            _keyboardSetting = key;
        }


        public event EventHandler<TetrisEventArgs> LineClearEvent;

        public virtual async Task GameStart()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                DrawColer(y, x);
            ReSetBlock();
            await LoopDownAsync();
        }

        protected virtual async Task LoopDownAsync()
        {
            while (true)
            {
                await Task.Delay(DELAY);
                MoveDown();
                if (!GamePlaying) break;
            }
        }

        protected virtual void ReSetBlock()
        {
            _block.NewBlock();
            NextBlockPreview();
            _currentY = 0 - _block.Block.GetLength(0);
            _currentX = _random.Next(0, 11 - _block.Block.GetLength(0));
        }

        protected virtual void DrawColer(int y, int x, int size = 30)
        {
            _drawLocker.WaitOne();
            using (Graphics g = _panel.CreateGraphics())
            {
                switch (_tetrisBoard[y, x])
                {
                    case 0:
                        g.FillRectangle(Brushes.Black, x * size, y * size, size, size);
                        g.DrawRectangle(new Pen(Brushes.Black), x * size, y * size, size, size);
                        break;
                    case 1:
                        g.FillRectangle(_block.BlockColor[_block.BlockNum], x * size, y * size, size,
                            size);
                        g.DrawRectangle(new Pen(Brushes.Black), x * size, y * size, size, size);
                        break;
                    case 3:
                        g.DrawRectangle(new Pen(Brushes.DarkGray), x * size, y * size, size, size);
                        break;
                    case 18:
                        g.FillRectangle(_block.BlockColor[_tetrisBoard[y, x] - 10], x * size,
                            y * size,
                            size, size);
                        g.DrawRectangle(new Pen(Brushes.Black), x * size, y * size, size, size);
                        break;
                    default:
                        {
                            if (_tetrisBoard[y, x] > 10 && _tetrisBoard[y, x] <= 17)
                            {
                                g.FillRectangle(_block.BlockColor[_tetrisBoard[y, x] - 10], x * size,
                                    y * size, size, size);
                                g.DrawRectangle(new Pen(Brushes.Black), x * size, y * size, size, size);
                            }

                            break;
                        }
                }
            }

            _drawLocker.Release();
        }

        private void NextBlockPreview()
        {
            _drawLocker.WaitOne();
            const int size = 30;
            int nextBlockNum = _block.NextBlockNum();
            int[,] block = _block.BlockCreate(nextBlockNum, 0);
            using (Graphics g = _panel.CreateGraphics())
            {
                int blockLen = block.GetLength(0);
                for (var y = 0; y < 2; y++)
                    for (var x = 0; x < 4; x++)
                    {
                        int offsetX = x + 3;
                        int offsetY = y + 5;
                        if (x >= blockLen || y >= blockLen || block[y, x] != 1)
                        {
                            g.FillRectangle(Brushes.Black, offsetX * size, offsetY * size, size, size);
                            g.DrawRectangle(new Pen(Brushes.Black), offsetX * size, offsetY * size, size, size);
                        }
                        else
                        {
                            g.FillRectangle(_block.BlockColor[nextBlockNum], offsetX * size, offsetY * size, size,
                                size);
                            g.DrawRectangle(new Pen(Brushes.Black), offsetX * size, offsetY * size, size, size);
                        }
                    }
            }

            _drawLocker.Release();
        }

        // 블럭 이동이 가능한지 체크, 게임오버 체크
        private bool CanMoveBlock(int[,] block, int ny, int nx, bool callByMoveDown = false)
        {
            int size = block.GetLength(0);

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (block[y, x] == 1)
                {
                    if (ny + y >= HEIGHT || nx + x >= WIDTH || nx + x < 0)
                        return false;
                    if (ny + y < 0)
                        continue;
                    if (_tetrisBoard[y + ny, x + nx] <= 10)
                        continue;
                    if (callByMoveDown && ny < 0)
                        GamePlaying = false;
                    return false;
                }

            return true;
        }

        protected virtual void ReDrawBlock()
        {
            RemoveRedBlock();
            MoveRedBlock();
            DeleteBlockDownPreview();
            DrawDownLocation(DownLocationCalc(_currentY, _currentX));
        }

        protected void RemoveRedBlock()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBoard[y, x] == 1)
                {
                    _tetrisBoard[y, x] = 0;
                    DrawColer(y, x);
                }
        }

        private int GetHighLine()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBoard[y, x] > 10)
                    return y;

            return 20;
        }

        private void ClearLine()
        {
            int highLine = GetHighLine();

            foreach (int i in _clearLineList)
                for (int y = i; y >= highLine; y--)
                for (var x = 0; x < WIDTH; x++)
                    if (y > 0 && _tetrisBoard[y - 1, x] > 10)
                        _tetrisBoard[y, x] = _tetrisBoard[y - 1, x];
                    else
                        _tetrisBoard[y, x] = 0;

            int max = _clearLineList.Max();
            for (int y = highLine; y <= max; y++)
            for (var x = 0; x < WIDTH; x++)
                DrawColer(y, x);

            LineClearEvent?.Invoke(this, new TetrisEventArgs(_clearLineList.Count, _combo));
        }

        private bool CanClearLine()
        {
            _clearLineList.Clear();
            var b = false;

            for (int y = _currentY; y < HEIGHT; y++)
            {
                if (y < 0)
                    continue;
                if (LineCheck(y))
                {
                    _clearLineList.Add(y);
                    b = true;
                }
            }

            return b;

            bool LineCheck(int y)
            {
                for (var x = 0; x < WIDTH; x++)
                    if (_tetrisBoard[y, x] <= 10)
                        return false;
                return true;
            }
        }

        protected void MoveRedBlock()
        {
            int size = _block.Block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (_block.Block[y, x] == 1)
                {
                    if (_currentY + y < 0)
                        continue;
                    _tetrisBoard[y + _currentY, x + _currentX] = 1;
                    DrawColer(y + _currentY, x + _currentX);
                }
        }

        private int DownLocationCalc(int nowY, int currentX)
        {
            return DownLocationCalc(nowY, currentX, _block.Block);
        }

        protected int DownLocationCalc(int nowY, int currentX, int[,] block)
        {
            int size = block.GetLength(0);
            for (int currentY = nowY; currentY <= HEIGHT; currentY++)
            for (int y = size - 1; y >= 0; y--)
            for (var x = 0; x < size; x++)
            {
                if (currentY + y < 0)
                    continue;
                if (block[y, x] != 1)
                    continue;
                if (y + currentY < HEIGHT && _tetrisBoard[y + currentY, x + currentX] <= 10)
                    continue;
                return currentY - 1;
            }

            return -1;
        }

        private void DrawDownLocation(int currentY)
        {
            int size = _block.Block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (y + currentY >= 0 && y + currentY < HEIGHT &&
                    _block.Block[y, x] == 1 && _tetrisBoard[y + currentY, x + _currentX] == 0)
                {
                    _tetrisBoard[y + currentY, x + _currentX] = 3;
                    DrawColer(y + currentY, x + _currentX);
                }
        }

        private void DeleteBlockDownPreview()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBoard[y, x] == 3)
                {
                    _tetrisBoard[y, x] = 0;
                    DrawColer(y, x);
                }
        }

        protected void RotationBlock()
        {
            int[,] block = _block.BlockCreate(_block.BlockNum, _block.RotationNum + 1);

            if (CanMoveBlock(block, _currentY, _currentX))
            {
                RotationBlockAction();
            }
            else if (_currentX < 0 || _currentX + block.GetLength(0) > WIDTH)
            {
                int currentX = _currentX < 0 ? 0 : WIDTH - block.GetLength(0);
                if (CanMoveBlock(block, _currentY, currentX))
                {
                    _currentX = currentX;
                    RotationBlockAction();
                }
            }

            void RotationBlockAction()
            {
                _block.SetRotationBlock();
                ReDrawBlock();
            }
        }

        protected void HardDown()
        {
            lock (_locker)
            {
                int size = _block.Block.GetLength(0);
                for (int currentY = _currentY; currentY <= HEIGHT; currentY++)
                for (int y = size - 1; y >= 0; y--)
                for (var x = 0; x < size; x++)
                {
                    if (_block.Block[y, x] != 1)
                        continue;
                    if (y + currentY < 0)
                        continue;
                    if (y + currentY != HEIGHT && _tetrisBoard[y + currentY, x + _currentX] <= 10)
                        continue;

                    if (CanMoveBlock(_block.Block, currentY - 1, _currentX, true))
                    {
                        Score += 2 * (currentY - 1 - _currentY);
                        _currentY = currentY - 1;
                        RemoveRedBlock();
                        MoveRedBlock();
                    }

                    goto LOOP_EXIT;
                }

                LOOP_EXIT: ;
            }

            MoveDown();
        }

        public void KeyBoardAction(KeyEventArgs e)
        {
            if (_keyboardSetting == null) return;

            if (_keyboardSetting.IsKeyDownAction(e))
                MoveDown();
            if (_keyboardSetting.IsKeyHardDownAction(e))
                HardDown();
            if (_keyboardSetting.IsKeyLeftAction(e))
                MoveLeft();
            if (_keyboardSetting.IsKeyRightAction(e))
                MoveRight();
            if (_keyboardSetting.IsKeyRotationAction(e))
                RotationBlock();
        }

        public void SetGarbageLine(int cnt)
        {
            lock (_locker)
            {
                if (cnt <= 0) return;

                int highLine = GetHighLine();
                int blank = _random.Next(0, WIDTH);
                for (int y = Math.Max(highLine - cnt, 0); y < HEIGHT - cnt; y++)
                for (var x = 0; x < WIDTH; x++)
                {
                    _tetrisBoard[y, x] = _tetrisBoard[y + cnt, x];
                    DrawColer(y, x);
                }

                for (int y = HEIGHT - cnt; y < HEIGHT; y++)
                for (var x = 0; x < WIDTH; x++)
                {
                    _tetrisBoard[y, x] = x == blank ? 0 : 18;
                    DrawColer(y, x);
                }

                for (var y = 0; y < _block.Block.GetLength(0); y++)
                {
                    if (_currentY + y < 0 || _currentY + y >= HEIGHT) continue;
                    for (var x = 0; x < _block.Block.GetLength(0); x++)
                    {
                        if (_currentX + x < 0 || _currentX + x >= WIDTH) continue;
                        if (_block.Block[y, x] == 1 && _tetrisBoard[_currentY + y, _currentX + x] > 10)
                            _currentX -= _block.Block.GetLength(0) - y;
                    }
                }

                if (highLine - cnt < 0) GamePlaying = false;
            }
        }

        private void MoveDown()
        {
            lock (_locker)
            {
                if (CanMoveBlock(_block.Block, _currentY + 1, _currentX, true))
                {
                    _currentY++;
                    Score += 2;
                    ReDrawBlock();
                }
                else
                {
                    if (!GamePlaying)
                        return;
                    Score += 10;
                    for (var i = 0; i < HEIGHT; i++)
                    for (var j = 0; j < WIDTH; j++)
                        if (_tetrisBoard[i, j] == 1)
                        {
                            _tetrisBoard[i, j] = _block.BlockNum + 10;
                            DrawColer(i, j);
                        }

                    if (CanClearLine())
                    {
                        _combo++;
                        Score += 300 * (int) Math.Pow(_clearLineList.Count, 2);
                        ClearLine();
                    }
                    else
                    {
                        _combo = 0;
                    }

                    ReSetBlock();
                }
            }

            SetScoreText();
        }

        protected void MoveRight()
        {
            if (!CanMoveBlock(_block.Block, _currentY, _currentX + 1)) return;
            _currentX++;
            ReDrawBlock();
        }

        protected void MoveLeft()
        {
            if (!CanMoveBlock(_block.Block, _currentY, _currentX - 1)) return;
            _currentX--;
            ReDrawBlock();
        }

        protected virtual void SetScoreText()
        {
            if (_lblScore.InvokeRequired)
                _lblScore.Invoke((MethodInvoker) SetScoreText);
            else
                _lblScore.Text = Score.ToString();
        }
    }
}