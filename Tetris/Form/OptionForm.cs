﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Tetris.Properties;
using WMPLib;

namespace Tetris
{
    public sealed partial class OptionForm : MetroForm
    {
        private readonly WindowsMediaPlayer _mediaPlayer;
        private readonly HashSet<Keys> _registeredKey = new HashSet<Keys>();
        private List<(MetroButton Btn, Keyboard.Hotkey hotkey, Keyboard Player)> _hotkeyList;
        private (MetroButton btn, string str) _selectedButtonMemory;

        public OptionForm(WindowsMediaPlayer mediaPlayer)
        {
            InitializeComponent();
            RegisterHotkeyList();
            _mediaPlayer = mediaPlayer;
        }

        private void RegisterHotkeyList()
        {
            Keyboard p1 = Keyboard.GetPlayer1;
            Keyboard p2 = Keyboard.GetPlayer2;
            _hotkeyList = new List<(MetroButton, Keyboard.Hotkey, Keyboard)>
            {
                (btn_RotationRightP1, Keyboard.Hotkey.RotationCode, p1),
                (btn_MoveDownP1, Keyboard.Hotkey.DownCode, p1),
                (btn_MoveLeftP1, Keyboard.Hotkey.LeftCode, p1),
                (btn_MoveRightP1, Keyboard.Hotkey.RightCode, p1),
                (btn_HardDropP1, Keyboard.Hotkey.HardDownCode, p1),
                (btn_RotationRightP2, Keyboard.Hotkey.RotationCode, p2),
                (btn_MoveDownP2, Keyboard.Hotkey.DownCode, p2),
                (btn_MoveLeftP2, Keyboard.Hotkey.LeftCode, p2),
                (btn_MoveRightP2, Keyboard.Hotkey.RightCode, p2),
                (btn_HardDropP2, Keyboard.Hotkey.HardDownCode, p2)
            };

            foreach ((MetroButton Btn, Keyboard.Hotkey hotkey, Keyboard Player) tuple in _hotkeyList)
            {
                Keys key = tuple.Player.Properties[tuple.hotkey];
                _registeredKey.Add(key);
                tuple.Btn.Text = GetHotkeyString(key);
            }
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            tBar_Volume.Value = Settings.Default.Volume;
            tog_UiRendering.Checked = Settings.Default.CanRendering;
        }

        private void tog_UiRendering_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CanRendering = tog_UiRendering.Checked;
            GeneticAlgorithm.SetRendering();
        }

        private void tBar_Volume_Scroll(object sender, ScrollEventArgs e)
        {
            Settings.Default.Volume = tBar_Volume.Value;
            _mediaPlayer.settings.volume = tBar_Volume.Value;
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
            Keyboard.KeySettingFileSave();
        }

        private void btn_SetKeyboard_Click(object sender, EventArgs e)
        {
            if (_selectedButtonMemory.btn != null)
                _selectedButtonMemory.btn.Text = _selectedButtonMemory.str;
            MetroButton btn = sender as MetroButton;
            _selectedButtonMemory = (btn, btn.Text);
            btn.Text = @". . .";
        }

        private void btn_SetKeyboard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            groupBox1.Focus();
            if (_registeredKey.TryGetValue(e.KeyCode, out _))
            {
                MessageBox.Show(@"해당 키가 이미 단축키로 등록되어 있습니다.", @"이미 등록된 단축키");
                _selectedButtonMemory.btn.Text = _selectedButtonMemory.str;
                return;
            }

            (MetroButton Btn, Keyboard.Hotkey hotkey, Keyboard Player) keyData =
                _hotkeyList.First(t => t.Btn.Text == @". . .");

            Keys key = keyData.Player.Properties[keyData.hotkey];
            _registeredKey.Remove(key);
            keyData.Player.Properties[keyData.hotkey] = e.KeyCode;
            _registeredKey.Add(e.KeyCode);

            MetroButton btn = sender as MetroButton;
            btn.Text = GetHotkeyString(e.KeyCode);
            _selectedButtonMemory.btn = null;
        }

        private static string GetHotkeyString(Keys k)
        {
            switch (k)
            {
                case Keys.Escape: return "Esc";
                case Keys.Scroll: return "Scroll Lock";
                case Keys.Oemtilde: return "` ~";
                case Keys.D1: return "1 !";
                case Keys.D2: return "2 @";
                case Keys.D3: return "3 #";
                case Keys.D4: return "4 $";
                case Keys.D5: return "5 %";
                case Keys.D6: return "6 ^";
                case Keys.D7: return "7 &&";
                case Keys.D8: return "8 *";
                case Keys.D9: return "9 (";
                case Keys.D0: return "0 )";
                case Keys.OemMinus: return "- _";
                case Keys.Oemplus: return "= +";
                case Keys.Back: return "Backspace";
                case Keys.NumLock: return "Num Lock";
                case Keys.Divide: return "키패드/";
                case Keys.Multiply: return "키패드*";
                case Keys.Subtract: return "키패드-";
                case Keys.Add: return "키패드+";
                case Keys.NumPad1: return "키패드1";
                case Keys.NumPad2: return "키패드2";
                case Keys.NumPad3: return "키패드3";
                case Keys.NumPad4: return "키패드4";
                case Keys.NumPad5: return "키패드5";
                case Keys.NumPad6: return "키패드6";
                case Keys.NumPad7: return "키패드7";
                case Keys.NumPad8: return "키패드8";
                case Keys.NumPad9: return "키패드9";
                case Keys.NumPad0: return "키패드0";
                case Keys.Decimal: return "키패드.";
                case Keys.OemOpenBrackets: return "[ {";
                case Keys.Oem6: return "] }";
                case Keys.Oem5: return "\\ |";
                case Keys.Oem1: return "; :";
                case Keys.Oem7: return "' \"";
                case Keys.Oemcomma: return ", <";
                case Keys.OemPeriod: return ". >";
                case Keys.OemQuestion: return "/ ?";
                case Keys.KanaMode: return "한/영";
                case Keys.Apps: return "메뉴";
                case Keys.HanjaMode: return "한자";
                case Keys.Left: return "←";
                case Keys.Up: return "↑";
                case Keys.Right: return "→";
                case Keys.Down: return "↓";
                case Keys.Capital: return "Caps Lock";
                case Keys.Enter: return "Enter";
            }

            return k.ToString();
        }

        private void OptionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}