using System;

namespace Tetris
{
    public class AutoPlay
    {
        private Func<int, int, int[,]> BlockCreate;
        private Func<int, int, int> DownLocationCalc;

        public AutoPlay(Tetris tetris)
        {
            tetris.AutoPlayer += FindOptimalPos;
            tetris.DeliveryToMethod += ConnetingToTetrisMethod;
        }

        private void ConnetingToTetrisMethod(object sender, TetrisEventArgs e)
        {
            DownLocationCalc = e.DownLocationCalc;
            BlockCreate = e.BlockCreate;
        }

        private void FindOptimalPos(object sender, TetrisEventArgs e)
        {
            int width = e.TetrisBoard.GetLength(1);
            int[,] block = BlockCreate(e.BlockNum, 0);
            for (int x = -block.GetLength(0) + 1; x < width; x++)
            {
                if (!CanPutBlock(x, block)) break;
                int optimalY = e.DownLocationCalc(0, x);
            }
        }

        private static bool CanPutBlock(int currentX, int[,] block)
        {
            int size = block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
            {
                if (block[y, x] == 1 && x + currentX >= 0 && x + currentX < 10) continue;
                return false;
            }

            return true;
        }
    }
}