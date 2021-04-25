using System;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(int[,] tetrisBoard = null, int currentX = 0, int lineClearCount = 0, int combo = 0)
        {
            TetrisBoard = tetrisBoard;
            CurrentX = currentX;
            LineClearCount = lineClearCount;
            Combo = combo;
        }

        public int[,] TetrisBoard { get; }
        public int CurrentX { get; }
        public int LineClearCount { get; }
        public int Combo { get; }
    }
}