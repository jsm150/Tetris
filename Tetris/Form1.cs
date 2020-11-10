using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private Tetris tetrisPlayer1;
        private Tetris tetrisPlayer2;
        private List<Tetris> _tetrisContainer = new List<Tetris>();
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
            tetrisPlayer1 = new Tetris(this, 1, lbl_Score, KeyboardPlayer1.GetInstence);
            _tetrisContainer.Add(tetrisPlayer1);
            StartSetting();
            await _tetrisContainer[0].LoopDownAsync();
            GameEnd();
        }

        private void StartSetting()
        {
            timer1.Enabled = true;
            mediaPlayer.controls.play();
            btn_GameStart.Enabled = false;
            btn_1vs1.Enabled = false;
        }

        void GameEnd()
        {
            var multiplayer = (_tetrisContainer.Count > 1) ? true : false;
            for (int i = 0; i < _tetrisContainer.Count; i++)
            {
                _tetrisContainer[i].CanGameRun = false;
                _tetrisContainer[i] = null;
            }
            _tetrisContainer.Clear();
            lbl_BestScore.Text = int.Parse(lbl_Score.Text) > int.Parse(lbl_BestScore.Text) ? lbl_Score.Text : lbl_BestScore.Text;
            lbl_2pBestScore.Text = int.Parse(lbl_2pScore.Text) > int.Parse(lbl_2pBestScore.Text) ? lbl_2pScore.Text : lbl_2pBestScore.Text;
            timer1.Enabled = false;
            mediaPlayer.controls.stop();
            btn_GameStart.Enabled = true;
            btn_1vs1.Enabled = true;
            if (multiplayer)
            {
                var p1win = (int.Parse(lbl_Score.Text) > int.Parse(lbl_2pScore.Text)) ? true : false;
                if (p1win)
                    MessageBox.Show("플레이어1 승리!", $"Score: {lbl_Score.Text}");
                else
                    MessageBox.Show("플레이어2 승리!", $"Score: {lbl_2pScore.Text}");
            }
            else
            {
                MessageBox.Show($"Game Over!", $"Score: {lbl_Score.Text}");
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            foreach (var item in _tetrisContainer)
            {
                item?.KeyBoardAction(e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.URL == "")
                timer1.Enabled = false;
            mediaPlayer.controls.play();
        }

        private async Task AllLoopDownAsync()
        {
            List<Task> tasks = new List<Task>();
            foreach (var item in _tetrisContainer)
            {
                var t = item.LoopDownAsync();
                tasks.Add(t);
            }
            await Task.WhenAny(tasks.ToArray());
        }

        private async void btn_1vs1_Click(object sender, EventArgs e)
        {
            Size = new System.Drawing.Size(700, 730);
            StartSetting();
            tetrisPlayer1 = new Tetris(this, 1, lbl_Score, KeyboardPlayer2.GetInstence);
            tetrisPlayer2 = new Tetris(this, 12, lbl_2pScore, KeyboardPlayer1.GetInstence);
            _tetrisContainer.Add(tetrisPlayer1);
            _tetrisContainer.Add(tetrisPlayer2);
            await AllLoopDownAsync();
            GameEnd();
        }
    }
}
