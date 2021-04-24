using System;
using System.Collections.ObjectModel;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(int[,] tetrisBoard = null, int currentX = 0, int lineClearCount = 0)
        {
            TetrisBoard = tetrisBoard;
            CurrentX = currentX;
            LineClearCount = lineClearCount;
        }

        public int[,] TetrisBoard { get; }
        public int CurrentX { get; }
        public int LineClearCount { get; }
    }
}