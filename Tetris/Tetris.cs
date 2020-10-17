using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    class Tetris
    {
        Form1 form;
        Random random = new Random();
        int[,] tetrisBorad = new int[20, 10];
        const int width = 10;
        const int height = 20;
        const float sizeX = 30; const float sizeY = 30;
        int[,] block;
        int currentX = 0;
        int currentY = -1;
        int blockState = 0;

        enum Color
        {
            Black = 0,
            Red = 1,
            White = 2
        }

        public void TetrisInit(Form1 f) // 테트리스 판 그리기
        {
            form = f;
            // 시작위치 : x = 30, y = 60
            using (Graphics g = form.CreateGraphics())
            {
                for (int i = 2; i <= tetrisBorad.GetLength(0) + 1; i++)
                    for (int j = 1; j <= tetrisBorad.GetLength(1); j++)
                    {
                        g.FillRectangle(Brushes.Black, j * sizeX, i * sizeY, sizeX, sizeY);
                    }
            }
        }
        public void NewBlock()
        {
            blockState = random.Next(1, 2);
            currentY = -1;
        }

        public void BlockCreate()
        {
            switch (blockState)
            {
                case 1:
                    block = new int[4, 4]
                    {
                        { 1, 1, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 },
                        { 0, 0, 0, 0 }
                    };
                    currentX = random.Next(0, 11 - block.GetLength(0));
                    break;
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
                            if (y + currentY == 0)
                            {
                                MessageBox.Show("Game Over!");
                                form.Close();
                            }
                            return false;
                        }
                    }

                }
            return true;
        }

        void DrawColer(int y, int x)
        {
            using (Graphics g = form.CreateGraphics())
            {
                switch (tetrisBorad[y, x])
                {
                    case 1:
                        g.FillRectangle(Brushes.Red, (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY); g.FillRectangle(Brushes.Red, (x + 1) * sizeX, (y + 2) * sizeY, sizeX, sizeY);
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
        void RemoveRedBlock()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tetrisBorad[y, x] == 1)
                    {
                        tetrisBorad[y, x] = 0;
                    }
                }
            }
            DrawBlock(Color.Black);
        }

        void DrawBlock(Color color)
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (tetrisBorad[y, x] == (int)color)
                    {
                        DrawColer(y, x);
                    }
        }

        int BlockBottom()
        {
            int size = block.GetLength(0) - 1;
            for (int y = size; y >= 0; y--)
            {
                for (int x = size; x >= 0; x--)
                {
                    if (block[y, x] == 1)
                    {
                        return currentY + y;
                    }
                }
            }
            return 0;
        }
        void RemoveWhiteBlock()
        {
            int lowLine = BlockBottom();
            int highLine = 1;

            for (int y = 0; y < lowLine; y++)
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

            for (int y = lowLine; y >= highLine; y--)
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
            for (int x = 0; x < width; x++)
            {
                tetrisBorad[0, x] = 0;
                DrawColer(0, x);
            }
        }

        bool IsWhiteBlockCheak()
        {
            int size = block.GetLength(0) - 1;
            int line = BlockBottom();

            for (int x = 0; x < width; x++)
            {
                if (tetrisBorad[line, x] != 2)
                    return false;
            }
            return true;
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
                    }
                }
            }
        }

        public void MoveDown()
        {
            currentY++;
            if (IsCheak())
            {
                RemoveRedBlock();
                MoveRedBlock();
                DrawBlock(Color.Red);
            }
            else
            {
                currentY--;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                        if (tetrisBorad[i, j] == 1)
                        {
                            tetrisBorad[i, j] = 2;
                        }
                }
                if (IsWhiteBlockCheak())
                    RemoveWhiteBlock();
                else
                    DrawBlock(Color.White);
                NewBlock(); BlockCreate();
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
                DrawBlock(Color.Red);
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
                DrawBlock(Color.Red);
            }
            else
                currentX++;
        }
    }
}
