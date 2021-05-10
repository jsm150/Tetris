using System.Windows.Forms;

namespace Tetris
{
    internal class KeyboardPlayer2 : KeyboardSetting
    {
        public override Keys DownCode => Keys.S;
        public override Keys LeftCode => Keys.A;
        public override Keys RightCode => Keys.D;
        public override Keys RotationCode => Keys.W;
        public override Keys HardDownCode => Keys.E;

        public static KeyboardPlayer2 GetInstance { get; } = new KeyboardPlayer2();

        private KeyboardPlayer2()
        {
        }
    }
}