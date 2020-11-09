using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    class Tetris
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 20;
        private const float SIZE_X = 30; const float SIZE_Y = 30;
        private Form1 _form;
        private Random _random = new Random();
        private List<int> _clearLineList = new List<int>();
        private int[,] _tetrisBorad = new int[20, 10];
        private int[,] _block;
        private int _currentX = 0;
        private int _currentY = -1;
        private int _blockState = 0;
        private int _rotationNum = 0;
        private int _delay = 450;
        private bool CanGameRun { get; set; } = false;
        public int Score { get; private set; } = 0;
        

        public Tetris(Form1 f)
        {
            _form = f;
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    DrawColer(y, x);
                }
            }
            NewBlock(); BlockCreate();
        }
        public async Task LoopDownAsync(Label l)
        {
            while (true)
            {
                await Task.Delay(_delay);
                MoveDown(l);
                if (CanGameRun)
                    break;
            }
        }

        public void NewBlock()
        {
            _blockState = _random.Next(1, 8);
            _currentY = -1;
            _rotationNum = 0;
        }

        public void BlockCreate()
        {
            switch (_blockState)
            {
                case 1:
                    // ####
                    if (_rotationNum % 2 == 0)
                        _block = new int[4, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                    else
                        _block = new int[4, 4] { { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 } };
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                // ##
                // ##
                case 2:
                    _block = new int[2, 2] { { 1, 1 }, { 1, 1 } };
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                // ##
                //  ##
                case 3:
                    if (_rotationNum % 2 == 0)
                        _block = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                    else
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 0, 1 }, { 0, 1, 1 }, { 0, 1, 0 }
                        };
                    }
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 4:
                    // #
                    // ###
                    if (_rotationNum % 4 == 0)
                    {
                        _block = new int[3, 3]
                        {
                            { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 1)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 2)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 }
                        };
                    }
                    else if (_rotationNum % 4 == 3)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 0 }, { 1, 1, 0 }
                        };
                    }
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 5:
                    //  #
                    // ###
                    if (_rotationNum % 4 == 0)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 1)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 1 }, { 0, 1, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 2)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 0, 0 }, { 1, 1, 1 }, { 0, 1, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 3)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 1, 1, 0 }, { 0, 1, 0 }
                        };
                    }
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 6:
                    //   #
                    // ###
                    if (_rotationNum % 4 == 0)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 1)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 1 }
                        };
                    }
                    else if (_rotationNum % 4 == 2)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 0 }
                        };
                    }
                    else if (_rotationNum % 4 == 3)
                    {
                        _block = new int[3, 3]
                        {
                            { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 }
                        };
                    }
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 7:
                    //  ##
                    // ##
                    if (_rotationNum % 2 == 0)
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 }
                        };
                    }
                    else
                    {
                        _block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 1 }
                        };
                    }
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
            }
        }

        void DrawColer(int y, int x)
        {
            using (Graphics g = _form.CreateGraphics())
            {
                switch (_tetrisBorad[y, x])
                {
                    case 1:
                        g.FillRectangle(Brushes.Red, (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        g.DrawRectangle(new Pen(Brushes.Black), (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                    case 2:
                        g.FillRectangle(Brushes.White, (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        g.DrawRectangle(new Pen(Brushes.Black), (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                    case 3:
                        g.FillRectangle(Brushes.Gray, (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        g.DrawRectangle(new Pen(Brushes.Black), (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                    default:
                        g.FillRectangle(Brushes.Black, (x + 1) * SIZE_X, (y + 2) * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                }
            }
        }
        bool IsCheak() // 블럭 이동이 가능한지 체크, 게임오버 체크
        {
            int size = _block.GetLength(0);

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    if (_block[y, x] == 1)
                    {
                        if (_currentY + y >= HEIGHT || _currentX + x >= WIDTH)
                            return false;
                        if (_currentX + x < 0 || _currentY + y < 0)
                            return false;
                        if (_tetrisBorad[y + _currentY, x + _currentX] == 2)
                        {
                            if (_currentY == 0)
                            {
                                CanGameRun = true;
                            }
                            return false;
                        }
                    }

                }
            return true;
        }

        void RemoveRedBlock()
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (_tetrisBorad[y, x] == 1)
                    {
                        _tetrisBorad[y, x] = 0;
                        DrawColer(y, x);
                    }
                }
            }
        }

        void ClearLine()
        {
            int highLine = 1;

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (_tetrisBorad[y, x] == 2)
                    {
                        highLine = y;
                        goto LOOP_EXIT;
                    }
                }
            }
        LOOP_EXIT:;

            foreach (var i in _clearLineList)
            {
                if (i > 0)
                {
                    for (int y = i; y >= highLine; y--)
                    {
                        for (int x = 0; x < WIDTH; x++)
                        {
                            if (_tetrisBorad[y - 1, x] == 2)
                                _tetrisBorad[y, x] = 2;
                            else
                                _tetrisBorad[y, x] = 0;
                            DrawColer(y, x);
                        }
                    }
                }
            }

            for (int x = 0; x < WIDTH; x++)
            {
                _tetrisBorad[0, x] = 0;
                DrawColer(0, x);
            }
        }

        bool IsClearLineCheak()
        {
            int size = _block.GetLength(0);
            _clearLineList.Clear();
            bool b = false;

            for (int y = _currentY; y < HEIGHT; y++)
            {
                if (LineCheak(y))
                {
                    _clearLineList.Add(y);
                    b = true;
                }
            }
            return b;

            bool LineCheak(int y)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (_tetrisBorad[y, x] != 2)
                        return false;
                }
                return true;
            }
        }

        void MoveRedBlock()
        {
            int size = _block.GetLength(0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (_block[y, x] == 1)
                    {
                        _tetrisBorad[y + _currentY, x + _currentX] = 1;
                        DrawColer(y + _currentY, x + _currentX);
                    }
                }
            }
        }

        private void BlockPreview()
        {
            var size = _block.GetLength(0);
            for (int currentY = _currentY; currentY < HEIGHT; currentY++)
            {
                for (int y = size - 1; y >= 0; y--)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (_block[y, x] != 1)
                            continue;
                        if ((y + currentY == HEIGHT) || (_tetrisBorad[y + currentY, x + _currentX] == 2))
                        {
                            DrawBlockPreview(currentY - 1);
                            goto LOOP_EXIT;
                        }
                    }
                }
            }
        LOOP_EXIT:;
        }

        private void DrawBlockPreview(int currentY)
        {
            var size = _block.GetLength(0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (_block[y, x] == 1 && _tetrisBorad[y + currentY, x + _currentX] == 0)
                    {
                        _tetrisBorad[y + currentY, x + _currentX] = 3;
                        DrawColer(y + currentY, x + _currentX);
                    }
                }
            }
        }

        private void DeleteBlockPreview()
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (_tetrisBorad[y, x] == 3)
                    {
                        _tetrisBorad[y, x] = 0;
                        DrawColer(y, x);
                    }
                }
            }

        }

        public void RotationBlock()
        {
            _rotationNum++;
            BlockCreate();
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockPreview();
            }
            else
            {
                _rotationNum--;
                BlockCreate();
            }
        }

        void DelayAdjustment()
        {
            if (_delay > 250)
                _delay -= 8;
            else if (_delay > 200)
                _delay -= 3;
            else if (_delay > 100)
                _delay -= 1;
        }

        public void MoveDown(Label label)
        {
            lock (this)
            {
                _currentY++;
                if (IsCheak())
                {
                    Score += 5;
                    RemoveRedBlock();
                    MoveRedBlock();
                    DeleteBlockPreview();
                    BlockPreview();
                }
                else
                {
                    if (CanGameRun)
                        return;
                    DelayAdjustment();
                    _currentY--;
                    Score += 50;
                    for (int i = 0; i < HEIGHT; i++)
                    {
                        for (int j = 0; j < WIDTH; j++)
                            if (_tetrisBorad[i, j] == 1)
                            {
                                _tetrisBorad[i, j] = 2;
                                DrawColer(i, j);
                            }
                    }
                    if (IsClearLineCheak())
                    {
                        Score += 500 * _clearLineList.Count;
                        ClearLine();
                    }
                    NewBlock(); BlockCreate();
                }
                label.Text = Score.ToString();
            }
        }

        public void MoveRight()
        {
            int size = _block.GetLength(0);
            _currentX++;
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockPreview();
            }
            else
                _currentX--;
        }
        public void MoveLeft()
        {
            int size = _block.GetLength(0);
            _currentX--;
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockPreview();
            }
            else
                _currentX++;
        }
    }
}
