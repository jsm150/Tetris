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
        private const float SIZE_X = 30;
        private const float SIZE_Y = 30;
        private static readonly object Locker = new object();
        private readonly int[] _blockNumArr = Enumerable.Range(0, 8).ToArray();
        private readonly List<int> _clearLineList = new List<int>();
        private readonly Form1 _form;
        private readonly IKeyboardSetting _keyboardSetting;
        private readonly Label _label;
        private readonly int _offsetX;
        private readonly int _offsetY;
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private readonly int[,] _tetrisBorad = new int[20, 10];
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
            _offsetY = 4;
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
            NewBlock();
            BlockCreate();
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

        private void NewBlock()
        {
            int num = _random.Next(1, _blockNumPoint);
            _blockNum = _blockNumArr[num];
            (_blockNumArr[num], _blockNumArr[_blockNumPoint - 1]) =
                (_blockNumArr[_blockNumPoint - 1], _blockNumArr[num]);
            _blockNumPoint--;
            if (_blockNumPoint <= 1)
                _blockNumPoint = 8;
            _rotationNum = 0;
        }

        private void BlockCreate()
        {
            switch (_blockNum)
            {
                case 1:
                    // ####
                    if (_rotationNum % 2 == 0)
                        _block = new[,] {{0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}};
                    else
                        _block = new[,] {{0, 0, 0, 0}, {1, 1, 1, 1}, {0, 0, 0, 0}, {0, 0, 0, 0}};
                    break;
                // ##
                // ##
                case 2:
                    _block = new[,] {{1, 1}, {1, 1}};
                    break;
                // ##
                //  ##
                case 3:
                    if (_rotationNum % 2 == 0)
                        _block = new[,] {{0, 0, 1}, {0, 1, 1}, {0, 1, 0}};
                    else
                        _block = new[,] {{1, 1, 0}, {0, 1, 1}, {0, 0, 0}};
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
                    break;
            }

            if (_rotationNum == 0)
            {
                _currentY = 0 - _block.GetLength(0);
                _currentX = _random.Next(0, 11 - _block.GetLength(0));
            }
        }

        private void DrawColer(int y, int x)
        {
            using (Graphics g = _form.CreateGraphics())
            {
                int offsetX = x + _offsetX;
                int offsetY = y + _offsetY;
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

        private bool CanMoveBlock() // 블럭 이동이 가능한지 체크, 게임오버 체크
        {
            int size = _block.GetLength(0);

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (_block[y, x] == 1)
                {
                    if (_currentY + y >= HEIGHT || _currentX + x >= WIDTH)
                        return false;
                    if (_currentX + x < 0)
                        return false;
                    if (_currentY + y < 0)
                        continue;
                    if (_tetrisBorad[y + _currentY, x + _currentX] == 2)
                    {
                        if (_currentY <= 0) CanGameRun = false;
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

            foreach (int i in _clearLineList.Where(i => i > 0))
                for (int y = i; y >= highLine; y--)
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
                    if (_tetrisBorad[y, x] != 2)
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
                    _tetrisBorad[y + _currentY, x + _currentX] = 1;
                    DrawColer(y + _currentY, x + _currentX);
                }
        }

        private void BlockPreview()
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
                if (y + currentY != HEIGHT && _tetrisBorad[y + currentY, x + _currentX] != 2)
                    continue;
                DrawBlockPreview(currentY - 1);
                goto LOOP_EXIT;
            }

            LOOP_EXIT: ;
        }

        private void DrawBlockPreview(int currentY)
        {
            int size = _block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (y + currentY >= 0 && _block[y, x] == 1 && _tetrisBorad[y + currentY, x + _currentX] == 0)
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

        private void RotationBlock()
        {
            _rotationNum++;
            BlockCreate();
            if (CanMoveBlock())
            {
                RotationBlockAction();
            }
            else if (_currentX < 0 || _currentX + _block.GetLength(0) > WIDTH)
            {
                int temp = _currentX;
                _currentX = _currentX < 0 ? 0 : WIDTH - _block.GetLength(0);
                if (CanMoveBlock())
                    RotationBlockAction();
                else
                    _currentX = temp;
            }
            else
            {
                _rotationNum--;
                BlockCreate();
            }

            void RotationBlockAction()
            {
                RemoveRedBlock();
                MoveRedBlock();
                DeleteBlockPreview();
                BlockPreview();
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
            int bak = _currentY;
            for (int currentY = _currentY; currentY <= HEIGHT; currentY++)
            for (int y = size - 1; y >= 0; y--)
            for (var x = 0; x < size; x++)
            {
                if (_block[y, x] != 1)
                    continue;
                if (y + currentY < 0)
                    continue;
                if (y + currentY != HEIGHT && _tetrisBorad[y + currentY, x + _currentX] != 2)
                    continue;
                _currentY = currentY - 1;
                if (CanMoveBlock())
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
                _currentY++;
                if (CanMoveBlock())
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
                    SetDelay();
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

        private void MoveRight()
        {
            _currentX++;
            if (CanMoveBlock())
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

        private void MoveLeft()
        {
            _currentX--;
            if (CanMoveBlock())
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