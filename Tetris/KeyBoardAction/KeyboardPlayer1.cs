using System.Windows.Forms;

namespace Tetris
{
    internal class KeyboardPlayer1 : IKeyboardSetting
    {
        private KeyboardPlayer1()
        {
        }

        public override Keys DownCode => Keys.Down;
        public override Keys LeftCode => Keys.Left;
        public override Keys RightCode => Keys.Right;
        public override Keys RotationCode => Keys.Up;
        public override Keys HardDownCode => Keys.NumPad0;


        public static KeyboardPlayer1 GetInstance { get; } = new KeyboardPlayer1();

        public override bool IsKeyDownAction(KeyEventArgs e)
        {
            return e.KeyCode == DownCode;
        }

        public override bool IsKeyLeftAction(KeyEventArgs e)
        {
            return e.KeyCode == LeftCode;
        }

        public override bool IsKeyRightAction(KeyEventArgs e)
        {
            return e.KeyCode == RightCode;
        }

        public override bool IsKeyRotationAction(KeyEventArgs e)
        {
            return e.KeyCode == RotationCode;
        }

        public override bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == HardDownCode;
        }
    }
}