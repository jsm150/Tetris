using System;
using System.Collections.ObjectModel;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(int[,] tetrisBoard = null, int currentX = 0, int delay = 0, int lineClearCount = 0)
        {
            TetrisBoard = tetrisBoard;
            CurrentX = currentX;
            Delay = delay;
            LineClearCount = lineClearCount;
        }

        public int[,] TetrisBoard { get; }
        public int CurrentX { get; }
        public int Delay { get; }
        public int LineClearCount { get; }
    }
}