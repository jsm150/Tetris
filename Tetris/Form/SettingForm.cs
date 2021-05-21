using System;
using System.Windows.Forms;
using MetroFramework.Forms;
using Tetris.Properties;
using WMPLib;

namespace Tetris
{
    public sealed partial class SettingForm : MetroForm
    {
        private readonly WindowsMediaPlayer _mediaPlayer;

        public SettingForm(WindowsMediaPlayer mediaPlayer)
        {
            InitializeComponent();
            _mediaPlayer = mediaPlayer;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            tBar_Volume.Value = Settings.Default.Volume;
            tog_UiRendering.Checked = Settings.Default.CanRendering;
        }

        private void tog_UiRendering_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CanRendering = tog_UiRendering.Checked;
            Settings.Default.Save();
            GeneticAlgorithm.SetRendering();
        }

        private void tBar_Volume_Scroll(object sender, ScrollEventArgs e)
        {
            _mediaPlayer.settings.volume = tBar_Volume.Value;
            Settings.Default.Volume = tBar_Volume.Value;
            Settings.Default.Save();
        }
    }
}