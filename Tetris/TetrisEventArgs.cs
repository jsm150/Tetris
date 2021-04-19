using System;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(int[,] block, int[,] tetrisBoard)
        {
            Block = block;
            TetrisBoard = tetrisBoard;
        }

        public int[,] Block { get; }
        public int[,] TetrisBoard { get; }
    }
}