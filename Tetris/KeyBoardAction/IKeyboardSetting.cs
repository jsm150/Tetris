using System.Windows.Forms;

namespace Tetris
{
    public abstract class IKeyboardSetting
    {
        public abstract bool IsKeyDownAction(KeyEventArgs e);
        public abstract bool IsKeyLeftAction(KeyEventArgs e);
        public abstract bool IsKeyRightAction(KeyEventArgs e);
        public abstract bool IsKeyRotationAction(KeyEventArgs e);
        public abstract bool IsKeyHardDownAction(KeyEventArgs e);

        public abstract Keys DownCode { get; }
        public abstract Keys LeftCode { get; }
        public abstract Keys RightCode { get; }
        public abstract Keys RotationCode { get; }
        public abstract Keys HardDownCode { get; }
    }
}