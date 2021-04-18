using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class Tetris
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 20;
        private static readonly object Locker = new object();
        private readonly int[] _blockNumArr = Enumerable.Range(0, 8).ToArray();
        private readonly List<int> _clearLineList = new List<int>();
        private readonly Form1 _form;
        private readonly IKeyboardSetting _keyboardSetting;
        private readonly Label _label;
        private readonly int _offsetX;
        private readonly int _offsetY = 7;
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private readonly int[,] _tetrisBoard = new int[20, 10];
        private readonly Queue<int> _saveBlock = new Queue<int>();
        private int[,] _block;
        private int _blockNum;
        private int _blockNumPoint = 8;
        private int _currentX;
        private int _currentY = -1;
        private int _rotationNum;


        public Tetris(Form1 f, int offsetX, Label label, IKeyboardSetting key, int id)
        {
            PlayerId = id;
            _offsetX = offsetX;
            _form = f;
            _label = label;
            _keyboardSetting = key;
        }

        public int PlayerId { get; }
        public int Delay { get; private set; } = 450;
        public bool CanGameRun { get; set; } = true;
        public int Score { get; private set; }

        public async Task GameStart()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                DrawColer(y, x);
            SetBlockInit();
            await LoopDownAsync();
        }

        public async Task LoopDownAsync()
        {
            while (true)
            {
                await Task.Delay(Delay);
                MoveDown();
                if (!CanGameRun)
                    break;
            }
        }

        private void SetBlockInit()
        {
            NewBlock();
            NextBlockPreview();
            _block = BlockCreate(_blockNum, _rotationNum);
            _currentY = 0 - _block.GetLength(0);
            _currentX = _random.Next(0, 11 - _block.GetLength(0));
        }

        private void NewBlock()
        {
            int num = _random.Next(1, _blockNumPoint);
            _saveBlock.Enqueue(_blockNumArr[num]);

            (_blockNumArr[num], _blockNumArr[_blockNumPoint - 1]) = 
                (_blockNumArr[_blockNumPoint - 1], _blockNumArr[num]);
            _blockNumPoint--;
            if (_blockNumPoint <= 1) 
                _blockNumPoint = 8;

            if (_saveBlock.Count <= 1)
            {
                NewBlock();
                return;
            }
            _blockNum = _saveBlock.Dequeue();
            _rotationNum = 0;
        }

        private int[,] BlockCreate(int blockNum, int rotationNum)
        {
            switch (blockNum)
            {
                case 1:
                    // ####
                    if (rotationNum % 2 == 0)
                        return new[,] {{0, 0, 0, 0}, { 1, 1, 1, 1 }, { 0, 0, 0, 0}, { 0, 0, 0, 0}};
                    else
                        return new[,] {{0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}};
                // ##
                // ##
                case 2:
                    return new[,] {{1, 1}, {1, 1}};
                // ##
                //  ##
                case 3:
                    if (rotationNum % 2 == 0)
                        return new[,] {{0, 0, 1}, {0, 1, 1}, {0, 1, 0}};
                    else
                        return new[,] {{1, 1, 0}, {0, 1, 1}, {0, 0, 0}};
                case 4:
                    switch (rotationNum % 4)
                    {
                        // #
                        // ###
                        case 0:
                            return new[,] {{1, 0, 0}, {1, 1, 1}, {0, 0, 0}};
                        case 1:
                            return new[,] {{0, 1, 1}, {0, 1, 0}, {0, 1, 0}};
                        case 2:
                            return new[,] {{0, 0, 0}, {1, 1, 1}, {0, 0, 1}};
                        case 3:
                            return new[,] {{0, 1, 0}, {0, 1, 0}, {1, 1, 0}};
                    }

                    break;
                case 5:
                    //  #
                    // ###
                    if (rotationNum % 4 == 0)
                        return new[,] {{0, 1, 0}, {1, 1, 1}, {0, 0, 0}};
                    else if (rotationNum % 4 == 1)
                        return new[,] {{0, 1, 0}, {0, 1, 1}, {0, 1, 0}};
                    else if (rotationNum % 4 == 2)
                        return new[,] {{0, 0, 0}, {1, 1, 1}, {0, 1, 0}};
                    else if (rotationNum % 4 == 3)
                        return new[,] {{0, 1, 0}, {1, 1, 0}, {0, 1, 0}};
                    break;
                case 6:
                    //   #
                    // ###
                    if (rotationNum % 4 == 0)
                        return new[,] {{0, 0, 1}, {1, 1, 1}, {0, 0, 0}};
                    else if (rotationNum % 4 == 1)
                        return new[,] {{0, 1, 0}, {0, 1, 0}, {0, 1, 1}};
                    else if (rotationNum % 4 == 2)
                        return new[,] {{0, 0, 0}, {1, 1, 1}, {1, 0, 0}};
                    else if (rotationNum % 4 == 3)
                        return new[,] {{1, 1, 0}, {0, 1, 0}, {0, 1, 0}};
                    break;
                case 7:
                    //  ##
                    // ##
                    if (rotationNum % 2 == 0)
                        return new[,] {{0, 1, 1}, {1, 1, 0}, {0, 0, 0}};
                    else
                        return new[,] {{0, 1, 0}, {0, 1, 1}, {0, 0, 1}};
            }
            
            return null;
        }

        private void DrawColer(int y, int x)
        {
            const int sizeX = 30;
            const int sizeY = 30;
            using (Graphics g = _form.CreateGraphics())
            {
                int offsetX = x + _offsetX;
                int offsetY = y + _offsetY;
                switch (_tetrisBoard[y, x])
                {
                    case 1: // 떨어지고 있는 블럭
                        g.FillRectangle(Brushes.Red, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    case 2: // 떨어진 블럭
                        g.FillRectangle(Brushes.White, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    case 3: // 블럭이 떨어질 위치
                        g.FillRectangle(Brushes.Gray, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                    default: // 빈공간
                        g.FillRectangle(Brushes.Black, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        break;
                }
            }
        }

        private void NextBlockPreview()
        {
            const int sizeX = 25;
            const int sizeY = 25;
            int[,] block = BlockCreate(_saveBlock.Peek(), 0);
            using (Graphics g = _form.CreateGraphics())
            {
                int size = block.GetLength(0);
                for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    int offsetX = x + _offsetX + 4;
                    int offsetY = y + 4;
                    if (x >= size || y >= size || block[y, x] != 1)
                    {
                        g.FillRectangle(Brushes.Black, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * sizeX, offsetY * sizeY, sizeX, sizeY);
                    }
                }
            }

        }

        private bool CanMoveBlock(int[,] block, int ny, int nx) // 블럭 이동이 가능한지 체크, 게임오버 체크
        {
            int size = block.GetLength(0);

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (block[y, x] == 1)
                {
                    if (ny + y >= HEIGHT || nx + x >= WIDTH)
                        return false;
                    if (nx + x < 0)
                        return false;
                    if (ny + y < 0)
                        continue;
                    if (_tetrisBoard[y + ny, x + nx] == 2)
                    {
                        if (ny <= 0) CanGameRun = false;
                        return false;
                    }
                }

            return true;
        }

        private void RemoveRedBlock()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBoard[y, x] == 1)
                {
                    _tetrisBoard[y, x] = 0;
                    DrawColer(y, x);
                }
        }

        private void ClearLine()
        {
            var highLine = 1;

            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBoard[y, x] == 2)
                {
                    highLine = y;
                    goto LOOP_EXIT;
                }

            LOOP_EXIT:

            foreach (int i in _clearLineList.Where(i => i > 0))
                for (int y = i; y >= highLine; y--)
                for (var x = 0; x < WIDTH; x++)
                {
                    if (_tetrisBoard[y - 1, x] == 2)
                        _tetrisBoard[y, x] = 2;
                    else
                        _tetrisBoard[y, x] = 0;
                    DrawColer(y, x);
                }

            for (var x = 0; x < WIDTH; x++)
            {
                _tetrisBoard[0, x] = 0;
                DrawColer(0, x);
            }
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
                    if (_tetrisBoard[y, x] != 2)
                        return false;
                return true;
            }
        }

        private void MoveRedBlock()
        {
            int size = _block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (_block[y, x] == 1)
                {
                    if (_currentY + y < 0)
                        continue;
                    _tetrisBoard[y + _currentY, x + _currentX] = 1;
                    DrawColer(y + _currentY, x + _currentX);
                }
        }

        private void BlockDownPreview()
        {
            int size = _block.GetLength(0);
            for (int currentY = _currentY; currentY <= HEIGHT; currentY++)
            for (int y = size - 1; y >= 0; y--)
            for (var x = 0; x < size; x++)
            {
                if (currentY + y < 0)
                    continue;
                if (_block[y, x] != 1)
                    continue;
                if (y + currentY != HEIGHT && _tetrisBoard[y + currentY, x + _currentX] != 2)
                    continue;
                DrawBlockDownPreview(currentY - 1);
                goto LOOP_EXIT;
            }

            LOOP_EXIT: ;
        }

        private void DrawBlockDownPreview(int currentY)
        {
            int size = _block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (y + currentY >= 0 && _block[y, x] == 1 && _tetrisBoard[y + currentY, x + _currentX] == 0)
                {
                    _tetrisBoard[y + currentY, x + _currentX] = 3;
                    DrawColer(y + currentY, x + _currentX);
                }
        }

        private void DeleteBlockPreview()
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
            int[,] block = BlockCreate(_blockNum, _rotationNum + 1);

            if (CanMoveBlock(block, _currentY, _currentX))
                RotationBlockAction();
            else if (_currentX < 0 || _currentX + _block.GetLength(0) > WIDTH)
            {
                int currentX = _currentX < 0 ? 0 : WIDTH - _block.GetLength(0);
                if (CanMoveBlock(block, _currentY, currentX))
                {
                    _currentX = currentX;
                    RotationBlockAction();
                }
            }

            void RotationBlockAction()
            {
                _rotationNum++;
                _block = block;
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockDownPreview();
            }
        }


        private void SetDelay()
        {
            if (Delay > 250)
                Delay -= 5;
            else if (Delay > 200)
                Delay -= 2;
            else if (Delay > 100)
                Delay -= 1;
        }

        private void HardDown()
        {
            int size = _block.GetLength(0);
            for (int currentY = _currentY; currentY <= HEIGHT; currentY++)
            for (int y = size - 1; y >= 0; y--)
            for (var x = 0; x < size; x++)
            {
                if (_block[y, x] != 1)
                    continue;
                if (y + currentY < 0)
                    continue;
                if (y + currentY != HEIGHT && _tetrisBoard[y + currentY, x + _currentX] != 2)
                    continue;
                
                if (CanMoveBlock(_block, currentY - 1, _currentX))
                {
                    Score += 5 * (currentY - 1 - _currentY);
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

        private void MoveDown()
        {
            lock (Locker)
            {
                if (CanMoveBlock(_block, _currentY + 1, _currentX))
                {
                    _currentY++;
                    Score += 5;
                    RemoveRedBlock();
                    MoveRedBlock();
                    DeleteBlockPreview();
                    BlockDownPreview();
                }
                else
                {
                    if (!CanGameRun)
                        return;
                    SetDelay();
                    Score += 30;
                    for (var i = 0; i < HEIGHT; i++)
                    for (var j = 0; j < WIDTH; j++)
                        if (_tetrisBoard[i, j] == 1)
                        {
                            _tetrisBoard[i, j] = 2;
                            DrawColer(i, j);
                        }

                    if (CanClearLine())
                    {
                        Score += 500 * _clearLineList.Count;
                        ClearLine();
                    }

                    SetBlockInit();
                }

                _label.Text = Score.ToString();
            }
        }

        private void MoveRight()
        {
            if (!CanMoveBlock(_block, _currentY, _currentX + 1)) return;
            _currentX++;
            RemoveRedBlock();
            MoveRedBlock();
            DeleteBlockPreview();
            BlockDownPreview();
        }

        private void MoveLeft()
        {
            if (!CanMoveBlock(_block, _currentY, _currentX - 1)) return;
            _currentX--;
            RemoveRedBlock();
            MoveRedBlock();
            DeleteBlockPreview();
            BlockDownPreview();
        }
    }
}