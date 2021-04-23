using System.Windows.Forms;

namespace Tetris
{
    internal class KeyboardPlayer2 : IKeyboardSetting
    {
        private KeyboardPlayer2()
        {
        }

        public override Keys DownCode => Keys.S;
        public override Keys LeftCode => Keys.A;
        public override Keys RightCode => Keys.D;
        public override Keys RotationCode => Keys.W;
        public override Keys HardDownCode => Keys.E;

        public static KeyboardPlayer2 GetInstance { get; } = new KeyboardPlayer2();

        public override bool IsKeyDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == DownCode;
        }

        public override bool IsKeyLeftAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == LeftCode;
        }

        public override bool IsKeyRightAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == RightCode;
        }

        public override bool IsKeyRotationAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == RotationCode;
        }

        public override bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == HardDownCode;
        }
    }
}