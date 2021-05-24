using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Tetris
{
    public class Keyboard
    {
        private static readonly IReadOnlyList<Keyboard>
            _keyboards = new List<Keyboard> {new Keyboard(), new Keyboard()};

        public Keys DownCode { get; set; }
        public Keys LeftCode { get; set; }
        public Keys RightCode { get; set; }
        public Keys RotationCode { get; set; }
        public Keys HardDownCode { get; set; }

        public static Keyboard GetPlayer1 => _keyboards[0];
        public static Keyboard GetPlayer2 => _keyboards[1];

        private Keyboard()
        {
        }

        public static void SetKeyboard(Keyboard source, Keyboard destination)
        {
            destination.DownCode = source.DownCode;
            destination.LeftCode = source.LeftCode;
            destination.RightCode = source.RightCode;
            destination.RotationCode = source.RotationCode;
            destination.HardDownCode = source.HardDownCode;
        }

        public static void SetDefault()
        {
            _keyboards[0].LeftCode = Keys.Left;
            _keyboards[0].RightCode = Keys.Right;
            _keyboards[0].RotationCode = Keys.Up;
            _keyboards[0].DownCode = Keys.Down;
            _keyboards[0].HardDownCode = Keys.NumPad0;

            _keyboards[1].LeftCode = Keys.A;
            _keyboards[1].RightCode = Keys.D;
            _keyboards[1].RotationCode = Keys.W;
            _keyboards[1].DownCode = Keys.S;
            _keyboards[1].HardDownCode = Keys.E;
        }

        public static void KeySettingFileSave()
        {
            string di = Path.GetDirectoryName(FilePath.KeySetting);
            if (!Directory.Exists(di))
                Directory.CreateDirectory(di);

            using (StreamWriter stream = new StreamWriter(FilePath.KeySetting))
            using (JsonTextWriter writer = new JsonTextWriter(stream))
            {
                new JsonSerializer().Serialize(writer, _keyboards);
            }
        }

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