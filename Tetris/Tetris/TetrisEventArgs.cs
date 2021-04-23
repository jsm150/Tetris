using System;
using System.Collections.ObjectModel;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(Func<int, int, int[,], int> downLocationCalc = null,
            TetrisBlock tetrisBlock = null,
            IKeyboardSetting k = null,
            int[,] tetrisBoard = null,
            int currentX = 0)
        {
            DownLocationCalc = downLocationCalc;
            TetrisBlock = tetrisBlock;
            KeyboardSetting = k;
            TetrisBoard = tetrisBoard;
            CurrentX = currentX;
        }

        public Func<int, int, int[,], int> DownLocationCalc { get; }
        public TetrisBlock TetrisBlock { get; }
        public IKeyboardSetting KeyboardSetting { get; }
        public int[,] TetrisBoard { get; }
        public int CurrentX { get; }
    }
}