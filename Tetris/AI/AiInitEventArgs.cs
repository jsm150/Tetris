using System;

namespace Tetris
{
    public class AiInitEventArgs : EventArgs
    {
        public AiInitEventArgs(Func<int, int, int[,], int> downLocationCalc, TetrisBlock tetrisBlock,
            IKeyboardSetting keyboard, Action setAiPlaying)
        {
            DownLocationCalc = downLocationCalc;
            TetrisBlock = tetrisBlock;
            KeyboardSetting = keyboard;
            setAiPlaying.Invoke();
        }

        public Func<int, int, int[,], int> DownLocationCalc { get; }
        public TetrisBlock TetrisBlock { get; }
        public IKeyboardSetting KeyboardSetting { get; }
    }
}