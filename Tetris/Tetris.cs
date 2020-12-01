using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    internal class Tetris
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 20;
        private const float SIZE_X = 30;
        private const float SIZE_Y = 30;
        private static readonly object Locker = new object();
        private readonly List<int> _clearLineList = new List<int>();
        private readonly Form1 _form;
        private readonly IKeyboardSetting _keyboardSetting;
        private readonly Label _label;
        private readonly int _offsetX;
        private readonly int _offsetY;
        private readonly Random _random = new Random();
        private readonly int[,] _tetrisBorad = new int[20, 10];
        private int[,] _block;
        private int _blockNum;
        private int _currentX;
        private int _currentY = -1;
        private int _rotationNum;


        public Tetris(Form1 f, int offsetX, Label label, IKeyboardSetting key)
        {
            _offsetX = offsetX;
            _offsetY = 2;
            _form = f;
            _label = label;
            _keyboardSetting = key;
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                DrawColer(y, x);
            NewBlock();
            BlockCreate();
        }

        public int Delay { get; private set; } = 450;
        public bool CanGameRun { get; set; } = true;
        public int Score { get; private set; }


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

        public void NewBlock()
        {
            _blockNum = _random.Next(1, 8);
            _currentY = -1;
            _rotationNum = 0;
        }

        public void BlockCreate()
        {
            switch (_blockNum)
            {
                case 1:
                    // ####
                    if (_rotationNum % 2 == 0)
                        _block = new[,] {{1, 1, 1, 1}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}};
                    else
                        _block = new[,] {{0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}};
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                // ##
                // ##
                case 2:
                    _block = new[,] {{1, 1}, {1, 1}};
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                // ##
                //  ##
                case 3:
                    if (_rotationNum % 2 == 0)
                        _block = new[,] {{1, 1, 0}, {0, 1, 1}, {0, 0, 0}};
                    else
                        _block = new[,]
                        {
                            {0, 0, 1}, {0, 1, 1}, {0, 1, 0}
                        };
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 4:
                    switch (_rotationNum % 4)
                    {
                        // #
                        // ###
                        case 0:
                            _block = new[,]
                            {
                                {1, 0, 0}, {1, 1, 1}, {0, 0, 0}
                            };
                            break;
                        case 1:
                            _block = new[,]
                            {
                                {0, 1, 1}, {0, 1, 0}, {0, 1, 0}
                            };
                            break;
                        case 2:
                            _block = new[,]
                            {
                                {0, 0, 0}, {1, 1, 1}, {0, 0, 1}
                            };
                            break;
                        case 3:
                            _block = new[,]
                            {
                                {0, 1, 0}, {0, 1, 0}, {1, 1, 0}
                            };
                            break;
                    }

                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 5:
                    //  #
                    // ###
                    if (_rotationNum % 4 == 0)
                        _block = new[,]
                        {
                            {0, 1, 0}, {1, 1, 1}, {0, 0, 0}
                        };
                    else if (_rotationNum % 4 == 1)
                        _block = new[,]
                        {
                            {0, 1, 0}, {0, 1, 1}, {0, 1, 0}
                        };
                    else if (_rotationNum % 4 == 2)
                        _block = new[,]
                        {
                            {0, 0, 0}, {1, 1, 1}, {0, 1, 0}
                        };
                    else if (_rotationNum % 4 == 3)
                        _block = new[,]
                        {
                            {0, 1, 0}, {1, 1, 0}, {0, 1, 0}
                        };
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 6:
                    //   #
                    // ###
                    if (_rotationNum % 4 == 0)
                        _block = new[,]
                        {
                            {0, 0, 1}, {1, 1, 1}, {0, 0, 0}
                        };
                    else if (_rotationNum % 4 == 1)
                        _block = new[,]
                        {
                            {0, 1, 0}, {0, 1, 0}, {0, 1, 1}
                        };
                    else if (_rotationNum % 4 == 2)
                        _block = new[,]
                        {
                            {0, 0, 0}, {1, 1, 1}, {1, 0, 0}
                        };
                    else if (_rotationNum % 4 == 3)
                        _block = new[,]
                        {
                            {1, 1, 0}, {0, 1, 0}, {0, 1, 0}
                        };
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
                case 7:
                    //  ##
                    // ##
                    if (_rotationNum % 2 == 0)
                        _block = new[,]
                        {
                            {0, 1, 1}, {1, 1, 0}, {0, 0, 0}
                        };
                    else
                        _block = new[,]
                        {
                            {0, 1, 0}, {0, 1, 1}, {0, 0, 1}
                        };
                    if (_rotationNum == 0)
                        _currentX = _random.Next(0, 11 - _block.GetLength(0));
                    break;
            }
        }

        private void DrawColer(int y, int x)
        {
            using (var g = _form.CreateGraphics())
            {
                var offsetX = x + _offsetX;
                var offsetY = y + _offsetY;
                switch (_tetrisBorad[y, x])
                {
                    case 1:
                        g.FillRectangle(Brushes.Red, offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                    case 2:
                        g.FillRectangle(Brushes.White, offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                    case 3:
                        g.FillRectangle(Brushes.Gray, offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        g.DrawRectangle(new Pen(Brushes.Black), offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                    default:
                        g.FillRectangle(Brushes.Black, offsetX * SIZE_X, offsetY * SIZE_Y, SIZE_X, SIZE_Y);
                        break;
                }
            }
        }

        private bool CanMoveDown() // 블럭 이동이 가능한지 체크, 게임오버 체크
        {
            var size = _block.GetLength(0);

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (_block[y, x] == 1)
                {
                    if (_currentY + y >= HEIGHT || _currentX + x >= WIDTH)
                        return false;
                    if (_currentX + x < 0 || _currentY + y < 0)
                        return false;
                    if (_tetrisBorad[y + _currentY, x + _currentX] == 2)
                    {
                        if (_currentY == 0) CanGameRun = false;
                        return false;
                    }
                }

            return true;
        }

        private void RemoveRedBlock()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBorad[y, x] == 1)
                {
                    _tetrisBorad[y, x] = 0;
                    DrawColer(y, x);
                }
        }

        private void ClearLine()
        {
            var highLine = 1;

            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBorad[y, x] == 2)
                {
                    highLine = y;
                    goto LOOP_EXIT;
                }

            LOOP_EXIT:

            foreach (var i in _clearLineList.Where(i => i > 0))
                for (var y = i; y >= highLine; y--)
                for (var x = 0; x < WIDTH; x++)
                {
                    if (_tetrisBorad[y - 1, x] == 2)
                        _tetrisBorad[y, x] = 2;
                    else
                        _tetrisBorad[y, x] = 0;
                    DrawColer(y, x);
                }

            for (var x = 0; x < WIDTH; x++)
            {
                _tetrisBorad[0, x] = 0;
                DrawColer(0, x);
            }
        }

        private bool CanClearLine()
        {
            _clearLineList.Clear();
            var b = false;

            for (var y = _currentY; y < HEIGHT; y++)
                if (LineCheck(y))
                {
                    _clearLineList.Add(y);
                    b = true;
                }

            return b;

            bool LineCheck(int y)
            {
                for (var x = 0; x < WIDTH; x++)
                    if (_tetrisBorad[y, x] != 2)
                        return false;
                return true;
            }
        }

        private void MoveRedBlock()
        {
            var size = _block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (_block[y, x] == 1)
                {
                    _tetrisBorad[y + _currentY, x + _currentX] = 1;
                    DrawColer(y + _currentY, x + _currentX);
                }
        }

        private void BlockPreview()
        {
            var size = _block.GetLength(0);
            for (var currentY = _currentY; currentY <= HEIGHT; currentY++)
            for (var y = size - 1; y >= 0; y--)
            for (var x = 0; x < size; x++)
            {
                if (_block[y, x] != 1)
                    continue;
                if (y + currentY != HEIGHT && _tetrisBorad[y + currentY, x + _currentX] != 2)
                    continue;
                DrawBlockPreview(currentY - 1);
                goto LOOP_EXIT;
            }

            LOOP_EXIT: ;
        }

        private void DrawBlockPreview(int currentY)
        {
            var size = _block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (_block[y, x] == 1 && _tetrisBorad[y + currentY, x + _currentX] == 0)
                {
                    _tetrisBorad[y + currentY, x + _currentX] = 3;
                    DrawColer(y + currentY, x + _currentX);
                }
        }

        private void DeleteBlockPreview()
        {
            for (var y = 0; y < HEIGHT; y++)
            for (var x = 0; x < WIDTH; x++)
                if (_tetrisBorad[y, x] == 3)
                {
                    _tetrisBorad[y, x] = 0;
                    DrawColer(y, x);
                }
        }

        public void RotationBlock()
        {
            _rotationNum++;
            BlockCreate();
            if (CanMoveDown())
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

        private void DelayAdjustment()
        {
            if (Delay > 250)
                Delay -= 5;
            else if (Delay > 200)
                Delay -= 2;
            else if (Delay > 100)
                Delay -= 1;
        }

        public void HardDown()
        {
            var size = _block.GetLength(0);
            var bak = _currentY;
            for (var currentY = _currentY; currentY <= HEIGHT; currentY++)
            for (var y = size - 1; y >= 0; y--)
            for (var x = 0; x < size; x++)
            {
                if (_block[y, x] != 1)
                    continue;
                if (y + currentY < 0)
                    continue;
                if (y + currentY != HEIGHT && _tetrisBorad[y + currentY, x + _currentX] != 2)
                    continue;
                _currentY = currentY - 1;
                if (CanMoveDown())
                {
                    Score += 5 * (_currentY - bak);
                    _label.Text = Score.ToString();
                    RemoveRedBlock();
                    MoveRedBlock();
                }
                else
                {
                    _currentY = bak;
                }

                goto LOOP_EXIT;
            }

            LOOP_EXIT: ;
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

        public void MoveDown()
        {
            lock (Locker)
            {
                _currentY++;
                if (CanMoveDown())
                {
                    Score += 5;
                    RemoveRedBlock();
                    MoveRedBlock();
                    DeleteBlockPreview();
                    BlockPreview();
                }
                else
                {
                    if (!CanGameRun)
                        return;
                    DelayAdjustment();
                    _currentY--;
                    Score += 30;
                    for (var i = 0; i < HEIGHT; i++)
                    for (var j = 0; j < WIDTH; j++)
                        if (_tetrisBorad[i, j] == 1)
                        {
                            _tetrisBorad[i, j] = 2;
                            DrawColer(i, j);
                        }

                    if (CanClearLine())
                    {
                        Score += 500 * _clearLineList.Count;
                        ClearLine();
                    }

                    NewBlock();
                    BlockCreate();
                }

                _label.Text = Score.ToString();
            }
        }

        public void MoveRight()
        {
            _currentX++;
            if (CanMoveDown())
            {
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockPreview();
            }
            else
            {
                _currentX--;
            }
        }

        public void MoveLeft()
        {
            _currentX--;
            if (CanMoveDown())
            {
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockPreview();
            }
            else
            {
                _currentX++;
            }
        }
    }
}