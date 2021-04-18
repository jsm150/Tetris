using System.Windows.Forms;

namespace Tetris
{
    public interface IKeyboardSetting
    {
        bool IsKeyDownAction(KeyEventArgs e);
        bool IsKeyLeftAction(KeyEventArgs e);
        bool IsKeyRightAction(KeyEventArgs e);
        bool IsKeyRotationAction(KeyEventArgs e);
        bool IsKeyHardDownAction(KeyEventArgs e);
    }
}