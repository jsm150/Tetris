using System;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(int lineClearCount, int combo)
        {
            LineClearCount = lineClearCount;
            Combo = combo;
        }

        public int LineClearCount { get; }
        public int Combo { get; }
    }
}