using System.Windows.Forms;

namespace Tetris
{
    internal class KeyboardPlayer2 : IKeyboardSetting
    {
        private KeyboardPlayer2()
        {
        }

        public Keys DownCode => Keys.S;
        public Keys LeftCode => Keys.A;
        public Keys RightCode => Keys.D;
        public Keys RotationCode => Keys.W;
        public Keys HardDownCode => Keys.E;

        public static KeyboardPlayer2 GetInstance { get; } = new KeyboardPlayer2();

        public bool IsKeyDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == DownCode;
        }

        public bool IsKeyLeftAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == LeftCode;
        }

        public bool IsKeyRightAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == RightCode;
        }

        public bool IsKeyRotationAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == RotationCode;
        }

        public bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == HardDownCode;
        }
    }
}