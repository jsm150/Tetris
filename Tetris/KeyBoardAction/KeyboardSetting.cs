using System.Windows.Forms;

namespace Tetris
{
    public abstract class KeyboardSetting
    {
        public abstract Keys DownCode { get; }
        public abstract Keys LeftCode { get; }
        public abstract Keys RightCode { get; }
        public abstract Keys RotationCode { get; }
        public abstract Keys HardDownCode { get; }

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