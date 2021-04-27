using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class Tetris
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 20;
        private const int DELAY = 360;
        private static readonly Random Random = new Random();
        private readonly TetrisBlock _block = new TetrisBlock();
        private readonly List<int> _clearLineList = new List<int>();
        private readonly Form1 _form;
        private readonly IKeyboardSetting _keyboardSetting;
        private readonly Label _label;
        private readonly object _locker = new object();
        private readonly int _offsetX;
        private readonly int[,] _tetrisBoard = new int[HEIGHT, WIDTH];
        private int _currentX;
        private int _currentY = -1;
        private int _combo;

        public Tetris(Form1 f, int offsetX, Label label, IKeyboardSetting key, int id)
        {
            PlayerId = id;
            _offsetX = offsetX;
            _form = f;
            _label = label;
            _keyboardSetting = key;
        }

        public int Score { get; private set; }
        public int PlayerId { get; }
        public bool GamePlaying { get; set; } = true;
        public bool AiPlaying { get; private set; }

        public event EventHandler<TetrisEventArgs> ConnectingToAi;
        public event EventHandler<TetrisEventArgs> ReSetBlockEvent;
        public event EventHandler<TetrisEventArgs> LineClearEvent;
        public event EventHandler GameEndEvent;

        public async Task GameStart()
        {
            ConnectingToAi?.Invoke(this, TetrisEventArgs.GameStartEvent(DownLocationCalc, _block, _keyboardSetting, HasPlayingAi));
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
                if (GamePlaying) continue;

                GameEndEvent?.Invoke(this, EventArgs.Empty);
                break;
            }
        }

        private void HasPlayingAi()
        {
            AiPlaying = true;
        }

        private void ReSetBlock()
        {
            _block.NewBlock();
            NextBlockPreview();
            _currentY = 0 - _block.Block.GetLength(0);
            _currentX = Random.Next(0, 11 - _block.Block.GetLength(0));
            ReSetBlockEvent?.Invoke(this, TetrisEventArgs.ReSetBlockEvent(_tetrisBoard, _currentX));
        }

        protected virtual void DrawColer(int y, int x, int offsetY = 7, int sizeX = 30, int sizeY = 30)
        {
            using (Graphics g = _form.CreateGraphics())
            {
                int offsetX = x + _offsetX;
                offsetY += y;
                switch (_tetrisBoard[y, x])
                {
                    case 0:
                        g.FillRectangle(Brushes.Black, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    case 1:
                        g.FillRectangle(_block.BlockColor[_block.BlockNum], offsetX * sizeX, offsetY * sizeY, sizeX,
                            sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    case 3:
                        g.DrawRectangle(new Pen(Brushes.DarkGray), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    case 18:
                        g.FillRectangle(_block.BlockColor[_tetrisBoard[y, x] - 10], offsetX * sizeX, offsetY * sizeY,
                            sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    default:
                    {
                        if (_tetrisBoard[y, x] > 10 && _tetrisBoard[y, x] <= 17)
                        {
                            // _block.BlockColor[_tetrisBoard[y, x] - 10]
                            // Brushes.White
                            g.FillRectangle(Brushes.White, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                            g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        }

                        break;
                    }
                }
            }
        }

        protected virtual void NextBlockPreview()
        {
            const int sizeX = 30;
            const int sizeY = 30;
            int nextBlockNum = _block.NextBlockNum();
            int[,] block = _block.BlockCreate(nextBlockNum, 0);
            using (Graphics g = _form.CreateGraphics())
            {
                int size = block.GetLength(0);
                for (var y = 0; y < 2; y++)
                for (var x = 0; x < 4; x++)
                {
                    int offsetX = x + _offsetX + 3;
                    int offsetY = y + 4;
                    if (x >= size || y >= size || block[y, x] != 1)
                    {
                        g.FillRectangle(Brushes.Black, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                    }
                    else
                    {
                        g.FillRectangle(_block.BlockColor[nextBlockNum], offsetX * sizeX, offsetY * sizeY, sizeX,
                            sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                    }
                }
            }
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

            return -1;
        }

        private void ClearLine()
        {
            int highLine = GetHighLine();

            foreach (int i in _clearLineList)
                for (int y = i; y >= highLine; y--)
                for (var x = 0; x < WIDTH; x++)
                {
                    if (y > 0 && _tetrisBoard[y - 1, x] > 10)
                        _tetrisBoard[y, x] = _tetrisBoard[y - 1, x];
                    else
                        _tetrisBoard[y, x] = 0;
                    DrawColer(y, x);
                }

            LineClearEvent?.Invoke(this, TetrisEventArgs.LineClearEvent(_clearLineList.Count, _combo));
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

        private int DownLocationCalc(int nowY, int currentX, int[,] block)
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
                if (y + currentY >= 0 && _block.Block[y, x] == 1 && _tetrisBoard[y + currentY, x + _currentX] == 0)
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

        private void RotationBlock()
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
                _block.RotationBlockCreate();
                ReDrawBlock();
            }
        }

        private void HardDown()
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
                        _label.Text = Score.ToString();
                        RemoveRedBlock();
                        MoveRedBlock();
                    }

                    goto LOOP_EXIT;
                }

                LOOP_EXIT:
                MoveDown();
            }
        }

        public void KeyBoardAction(KeyEventArgs e)
        {
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
                int blank = Random.Next(0, WIDTH);
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
                    else _combo = 0;

                    ReSetBlock();
                }

                _label.Text = Score.ToString();
            }
        }

        private void MoveRight()
        {
            if (!CanMoveBlock(_block.Block, _currentY, _currentX + 1)) return;
            _currentX++;
            ReDrawBlock();
        }

        private void MoveLeft()
        {
            if (!CanMoveBlock(_block.Block, _currentY, _currentX - 1)) return;
            _currentX--;
            ReDrawBlock();
        }

        protected void OnGameEnd()
        {
            GameEndEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}