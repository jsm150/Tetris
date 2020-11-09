using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Tetris tetrisPlayer1;
        Tetris tetrisPlayer2;
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
            Size = new System.Drawing.Size(380, 730);
            timer1.Enabled = true;
            mediaPlayer.controls.play();
            tetrisPlayer2 = new Tetris(this, 1);
            btn_GameStart.Enabled = false;
            btn_1vs1.Enabled = false;
            await Task.Run(() => tetrisPlayer2.LoopDownAsync(lbl_Score));
            GameEnd();
        }

        void GameEnd()
        {
            lbl_BestScore.Text = lbl_Score.Text;
            lbl_2pBestScore.Text = lbl_2pScore.Text;
            timer1.Enabled = false;
            mediaPlayer.controls.stop();
            MessageBox.Show($"Game Over!\nScore: {tetrisPlayer1.Score}");
            btn_GameStart.Enabled = true;
            btn_1vs1.Enabled = true;
            tetrisPlayer1 = null;
            tetrisPlayer2 = null;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (tetrisPlayer2 != null)
            {
                if (e.KeyCode == Keys.Left)
                    tetrisPlayer2.MoveLeft();
                if (e.KeyCode == Keys.Right)
                    tetrisPlayer2.MoveRight();
                if (e.KeyCode == Keys.Down)
                    tetrisPlayer2.MoveDown(lbl_2pScore);
                if (e.KeyCode == Keys.Up)
                    tetrisPlayer2.RotationBlock();
            }
            if (tetrisPlayer1 != null)
            {
                if (e.KeyCode == Keys.A)
                    tetrisPlayer1.MoveLeft();
                if (e.KeyCode == Keys.D)
                    tetrisPlayer1.MoveRight();
                if (e.KeyCode == Keys.S)
                    tetrisPlayer1.MoveDown(lbl_Score);
                if (e.KeyCode == Keys.W)
                    tetrisPlayer1.RotationBlock();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mediaPlayer.controls.play();
        }

        private void btn_1vs1_Click(object sender, EventArgs e)
        {
            Size = new System.Drawing.Size(700, 730);
            timer1.Enabled = true;
            mediaPlayer.controls.play();
            tetrisPlayer1 = new Tetris(this, 1);
            tetrisPlayer2 = new Tetris(this, 12);
            btn_GameStart.Enabled = false;
            btn_1vs1.Enabled = false;
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(tetrisPlayer1.Delay);
                    tetrisPlayer1?.MoveDown(lbl_Score);
                    if (!tetrisPlayer1.CanGameRun)
                        break;
                }
                GameEnd();

            });
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(tetrisPlayer2.Delay);
                    tetrisPlayer2?.MoveDown(lbl_2pScore);
                    if (!tetrisPlayer2.CanGameRun)
                        break;
                }
                GameEnd();
            });
        }
    }
}
