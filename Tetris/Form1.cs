using System;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Tetris tetris;
        WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();
        public Form1()
        {
            if (File.Exists("Tetris_BGM.mp3"))
                mediaPlayer.URL = "Tetris_BGM.mp3";
            mediaPlayer.settings.volume = 10;
            mediaPlayer.controls.stop();
            InitializeComponent();
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            mediaPlayer.controls.play();
            tetris = new Tetris(this);
            btn_GameStart.Enabled = false;
            await tetris.LoopDownAsync(lbl_Score);
            GameEnd();
        }

        void GameEnd()
        {
            timer1.Enabled = false;
            mediaPlayer.controls.stop();
            MessageBox.Show($"Game Over!\nScore: {tetris.Score}");
            btn_GameStart.Enabled = true;
            tetris = null;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (tetris != null)
            {
                if (e.KeyCode == Keys.Left)
                    tetris.MoveLeft();
                if (e.KeyCode == Keys.Right)
                    tetris.MoveRight();
                if (e.KeyCode == Keys.Down)
                    tetris.MoveDown(lbl_Score);
                if (e.KeyCode == Keys.Up)
                    tetris.RotationBlock();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mediaPlayer.controls.play();
        }

    }
}
