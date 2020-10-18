using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    class Tetris
    {
        Form1 form;
        Random random = new Random();
        List<int> clearLineList = new List<int>();
        int[,] tetrisBorad = new int[20, 10];
        int[,] block;
        const int width = 10;
        const int height = 20;
        const float sizeX = 30; const float sizeY = 30;
        int currentX = 0;
        int currentY = -1;
        int blockState = 0;
        int rotationNum = 0;
        int delay = 450;
        public bool GameStop { get; private set; } = false;
        public int Score { get; private set; } = 0;
        

        public Tetris(Form1 f)
        {
            form = f;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
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
                await Task.Delay(delay);
                MoveDown(l);
                if (GameStop)
                    break;
            }
        }

        public void NewBlock()
        {
            blockState = random.Next(1, 8);
            currentY = -1;
            rotationNum = 0;
        }

        public void BlockCreate()
        {
            switch (blockState)
            {
                case 1:
                    // ####
                    if (rotationNum % 2 == 0)
                        block = new int[4, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
                    else
                        block = new int[4, 4] { { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 } };
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
                // ##
                // ##
                case 2:
                    block = new int[2, 2] { { 1, 1 }, { 1, 1 } };
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
                // ##
                //  ##
                case 3:
                    if (rotationNum % 2 == 0)
                        block = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
                    else
                    {
                        block = new int[3, 3]
                        {
                            { 0, 0, 1 }, { 0, 1, 1 }, { 0, 1, 0 }
                        };
                    }
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
                case 4:
                    // #
                    // ###
                    if (rotationNum % 4 == 0)
                    {
                        block = new int[3, 3]
                        {
                            { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 1)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 2)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 }
                        };
                    }
                    else if (rotationNum % 4 == 3)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 0 }, { 1, 1, 0 }
                        };
                    }
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
                case 5:
                    //  #
                    // ###
                    if (rotationNum % 4 == 0)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 1)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 1 }, { 0, 1, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 2)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 0, 0 }, { 1, 1, 1 }, { 0, 1, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 3)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 1, 1, 0 }, { 0, 1, 0 }
                        };
                    }
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
                case 6:
                    //   #
                    // ###
                    if (rotationNum % 4 == 0)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 1)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 1 }
                        };
                    }
                    else if (rotationNum % 4 == 2)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 0 }
                        };
                    }
                    else if (rotationNum % 4 == 3)
                    {
                        block = new int[3, 3]
                        {
                            { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 }
                        };
                    }
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
                case 7:
                    //  ##
                    // ##
                    if (rotationNum % 2 == 0)
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 }
                        };
                    }
                    else
                    {
                        block = new int[3, 3]
                        {
                            { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 1 }
                        };
                    }
                    if (rotationNum == 0)
                        currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
            }
        }

        void DrawColer(int y, int x)
        {
            using (Graphics g = form.CreateGraphics())
            {
                switch (tetrisBorad[y, x])
                {
                    case 1:
                        g.FillRectangle(Brushes.Red, (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY);
                        break;
                    case 2:
                        g.FillRectangle(Brushes.White, (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY);
                        g.DrawRectangle(new Pen(Brushes.Black), (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY);
                        break;
                    default:
                        g.FillRectangle(Brushes.Black, (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY);
                        break;
                }
            }
        }
        bool IsCheak() // 블럭 이동이 가능한지 체크, 게임오버 체크
        {
            int size = block.GetLength(0);

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    if (block[y, x] == 1)
                    {
                        if (currentY + y >= height || currentX + x >= width)
                            return false;
                        if (currentX + x < 0 || currentY + y < 0)
                            return false;
                        if (tetrisBorad[y + currentY, x + currentX] == 2)
                        {
                            if (currentY == 0)
                            {
                                GameStop = true;
                            }
                            return false;
                        }
                    }

                }
            return true;
        }

        void RemoveRedBlock()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tetrisBorad[y, x] == 1)
                    {
                        tetrisBorad[y, x] = 0;
                        DrawColer(y, x);
                    }
                }
            }
        }

        void ClearLine()
        {
            int highLine = 1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tetrisBorad[y, x] == 2)
                    {
                        highLine = y;
                        goto LOOP_EXIT;
                    }
                }
            }
        LOOP_EXIT:;

            foreach (var i in clearLineList)
            {
                if (i > 0)
                {
                    for (int y = i; y >= highLine; y--)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (tetrisBorad[y - 1, x] == 2)
                                tetrisBorad[y, x] = 2;
                            else
                                tetrisBorad[y, x] = 0;
                            DrawColer(y, x);
                        }
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                tetrisBorad[0, x] = 0;
                DrawColer(0, x);
            }
        }

        bool IsClearLineCheak()
        {
            int size = block.GetLength(0);
            clearLineList.Clear();
            bool b = false;

            for (int y = currentY; y < height; y++)
            {
                if (LineCheak(y))
                {
                    b = true; clearLineList.Add(y);
                }
            }
            return b;

            bool LineCheak(int y)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tetrisBorad[y, x] != 2)
                        return false;
                }
                return true;
            }
        }

        void MoveRedBlock()
        {
            int size = block.GetLength(0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (block[y, x] == 1)
                    {
                        tetrisBorad[y + currentY, x + currentX] = 1;
                        DrawColer(y + currentY, x + currentX);
                    }
                }
            }
        }


        public void RotationBlock()
        {
            rotationNum++;
            BlockCreate();
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
            }
            else
            {
                rotationNum--;
                BlockCreate();
            }
        }

        void DelayAdjustment()
        {
            if (delay > 250)
                delay -= 8;
            else if (delay > 200)
                delay -= 3;
            else if (delay > 100)
                delay -= 1;
        }

        public void MoveDown(Label label)
        {
            lock (this)
            {
                currentY++;
                if (IsCheak())
                {
                    Score += 5;
                    RemoveRedBlock();
                    MoveRedBlock();
                }
                else
                {
                    if (GameStop)
                        return;
                    DelayAdjustment();
                    currentY--;
                    Score += 50;
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                            if (tetrisBorad[i, j] == 1)
                            {
                                tetrisBorad[i, j] = 2;
                                DrawColer(i, j);
                            }
                    }
                    if (IsClearLineCheak())
                    {
                        Score += 500 * clearLineList.Count;
                        ClearLine();
                    }
                    NewBlock(); BlockCreate();
                }
                label.Text = Score.ToString();
            }
        }

        public void MoveRight()
        {
            int size = block.GetLength(0);
            currentX++;
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
            }
            else
                currentX--;
        }
        public void MoveLeft()
        {
            int size = block.GetLength(0);
            currentX--;
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
            }
            else
                currentX++;
        }
    }
}
