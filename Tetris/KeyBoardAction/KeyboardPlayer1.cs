using System.Windows.Forms;

namespace Tetris
{
    internal class KeyboardPlayer1 : KeyboardSetting
    {
        public override Keys DownCode => Keys.Down;
        public override Keys LeftCode => Keys.Left;
        public override Keys RightCode => Keys.Right;
        public override Keys RotationCode => Keys.Up;
        public override Keys HardDownCode => Keys.NumPad0;

        public static KeyboardPlayer1 GetInstance { get; } = new KeyboardPlayer1();

        private KeyboardPlayer1()
        {
        }
    }
}