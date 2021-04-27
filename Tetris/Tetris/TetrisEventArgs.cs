using System;

namespace Tetris
{
    public class TetrisEventArgs : EventArgs
    {
        private TetrisEventArgs(int[,] tetrisBoard, int currentX, int lineClearCount, int combo,
            Func<int, int, int[,], int> downLocationCalc, TetrisBlock tetrisBlock,
            IKeyboardSetting keyboardSetting)
        {
            TetrisBoard = tetrisBoard;
            CurrentX = currentX;
            LineClearCount = lineClearCount;
            Combo = combo;
            DownLocationCalc = downLocationCalc;
            TetrisBlock = tetrisBlock;
            KeyboardSetting = keyboardSetting;
        }

        public int[,] TetrisBoard { get; }
        public int CurrentX { get; }
        public int LineClearCount { get; }
        public int Combo { get; }
        public Func<int, int, int[,], int> DownLocationCalc { get; }
        public TetrisBlock TetrisBlock { get; }
        public IKeyboardSetting KeyboardSetting { get; }

        public static TetrisEventArgs GameStartEvent(Func<int, int, int[,], int> downLocationCalc, TetrisBlock tetrisBlock,
            IKeyboardSetting keyboardSetting, Action setAiPlaying)
        {
            setAiPlaying.Invoke();
            return new TetrisEventArgs(null, 0, 0, 0,
                downLocationCalc, tetrisBlock, keyboardSetting);
        }

        public static TetrisEventArgs ReSetBlockEvent(int[,] tetrisBoard, int currentX)
        {
            return new TetrisEventArgs(tetrisBoard, currentX, 0, 0, null, null, null);
        }

        public static TetrisEventArgs LineClearEvent(int lineClearCount, int combo)
        {
            return new TetrisEventArgs(null, 0, lineClearCount, combo, null, null, null);
        }
    }
}