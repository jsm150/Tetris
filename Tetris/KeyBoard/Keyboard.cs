using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Tetris
{
    public class Keyboard
    {
        public enum Hotkey
        {
            DownCode,
            LeftCode,
            RightCode,
            RotationCode,
            HardDownCode
        }

        private static readonly IReadOnlyList<Keyboard>
            _keyboards = new List<Keyboard> { new Keyboard(), new Keyboard() };

        public Dictionary<Hotkey, Keys> Properties { get; } = new Dictionary<Hotkey, Keys>();

        public static Keyboard GetPlayer1 => _keyboards[0];
        public static Keyboard GetPlayer2 => _keyboards[1];

        private Keyboard()
        {
        }

        public static void SetKeyboard(Keyboard source, Keyboard destination)
        {
            foreach (Hotkey hotkey in Enum.GetValues(typeof(Hotkey)))
            {
                destination.Properties[hotkey] = source.Properties[hotkey];
            }
        }

        public static void SetDefault()
        {
            _keyboards[0].Properties[Hotkey.LeftCode] = Keys.Left;
            _keyboards[0].Properties[Hotkey.RightCode] = Keys.Right;
            _keyboards[0].Properties[Hotkey.RotationCode] = Keys.Up;
            _keyboards[0].Properties[Hotkey.DownCode] = Keys.Down;
            _keyboards[0].Properties[Hotkey.HardDownCode] = Keys.NumPad0;

            _keyboards[1].Properties[Hotkey.LeftCode] = Keys.A;
            _keyboards[1].Properties[Hotkey.RightCode] = Keys.D;
            _keyboards[1].Properties[Hotkey.RotationCode] = Keys.W;
            _keyboards[1].Properties[Hotkey.DownCode] = Keys.S;
            _keyboards[1].Properties[Hotkey.HardDownCode] = Keys.E;
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
            return e.KeyCode == Properties[Hotkey.DownCode];
        }

        public bool IsKeyLeftAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Properties[Hotkey.LeftCode];
        }

        public bool IsKeyRightAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Properties[Hotkey.RightCode];
        }

        public bool IsKeyRotationAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Properties[Hotkey.RotationCode];
        }

        public bool IsKeyHardDownAction(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            return e.KeyCode == Properties[Hotkey.HardDownCode];
        }
    }
}