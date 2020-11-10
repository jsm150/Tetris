using System.Windows.Forms;

namespace Tetris
{
    class KeyboardPlayer2 : IKeyboardSetting
    {
        public static KeyboardPlayer2 keyboardPlayer2 = new KeyboardPlayer2();

        public static KeyboardPlayer2 GetInstence => keyboardPlayer2;
        private KeyboardPlayer2()
        {

        }

        public bool IsKeyDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Keys.S;
        }

        public bool IsKeyLeftAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Keys.A;
        }

        public bool IsKeyRightAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Keys.D;
        }

        public bool IsKeyRotationAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Keys.W;
        }

        public bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Keys.E;
        }
    }
}
