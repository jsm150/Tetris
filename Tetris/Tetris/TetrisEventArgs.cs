using System;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(Func<int, int, int[,]> blockCreate = null,
            Func<int, int, int> downLocationCalc = null,
            int[,] tetrisBoard = null,
            int blockNum = 0)
        {
            DownLocationCalc = downLocationCalc;
            BlockCreate = blockCreate;
            TetrisBoard = tetrisBoard;
            BlockNum = blockNum;
        }

        public int BlockNum { get; }
        public Func<int, int, int[,]> BlockCreate { get; }
        public Func<int, int, int> DownLocationCalc { get; }
        public int[,] TetrisBoard { get; }
    }
}