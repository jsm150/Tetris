using System.Windows.Forms;

namespace Tetris
{
    internal class KeyboardPlayer1 : IKeyboardSetting
    {
        private KeyboardPlayer1()
        {
        }

        public Keys DownCode => Keys.Down;
        public Keys LeftCode => Keys.Left;
        public Keys RightCode => Keys.Right;
        public Keys RotationCode => Keys.Up;
        public Keys HardDownCode => Keys.NumPad0;
        public static KeyboardPlayer1 GetInstance { get; } = new KeyboardPlayer1();

        public bool IsKeyDownAction(KeyEventArgs e)
        {
            return e.KeyCode == DownCode;
        }

        public bool IsKeyLeftAction(KeyEventArgs e)
        {
            return e.KeyCode == LeftCode;
        }

        public bool IsKeyRightAction(KeyEventArgs e)
        {
            return e.KeyCode == RightCode;
        }

        public bool IsKeyRotationAction(KeyEventArgs e)
        {
            return e.KeyCode == RotationCode;
        }

        public bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == HardDownCode;
        }
    }
}