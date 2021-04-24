using System;
using System.Collections.ObjectModel;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        public TetrisEventArgs(int[,] tetrisBoard, int currentX = 0)
        {
            TetrisBoard = tetrisBoard;
            CurrentX = currentX;
        }

        public int[,] TetrisBoard { get; }
        public int CurrentX { get; }
    }
}