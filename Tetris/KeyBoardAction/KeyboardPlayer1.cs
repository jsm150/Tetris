using System.Windows.Forms;

namespace Tetris
{
    class KeyboardPlayer1 : IKeyboardSetting
    {
        public static KeyboardPlayer1 keyboardPlayer1 = new KeyboardPlayer1();

        public static KeyboardPlayer1 GetInstence => keyboardPlayer1;
        private KeyboardPlayer1()
        {

        }

        public bool IsKeyDownAction(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Down;
        }

        public bool IsKeyLeftAction(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Left;
        }

        public bool IsKeyRightAction(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Right;
        }

        public bool IsKeyRotationAction(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Up;
        }

        public bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Keys.NumPad0;
        }
    }
}
