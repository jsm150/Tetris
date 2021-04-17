using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private readonly WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();
        private readonly List<Tetris> _tetrisContainer = new List<Tetris>();
        private Tetris _tetrisPlayer1;
        private Tetris _tetrisPlayer2;

        public Form1()
        {
            if (File.Exists("Tetris_BGM.mp3"))
                _mediaPlayer.URL = "Tetris_BGM.mp3";
            _mediaPlayer.settings.volume = 10;
            _mediaPlayer.controls.stop();
            InitializeComponent();
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            Size = new Size(380, 730);
            _tetrisPlayer1 = new Tetris(this, 1, lbl_Score, KeyboardPlayer1.GetInstence);
            _tetrisContainer.Add(_tetrisPlayer1);
            StartSetting();
            await _tetrisContainer[0].LoopDownAsync();
            GameEnd();
        }

        private void StartSetting()
        {
            timer1.Enabled = true;
            _mediaPlayer.controls.play();
            btn_GameStart.Enabled = false;
            btn_1vs1.Enabled = false;
        }

        private void GameEnd()
        {
            bool multiplayer = _tetrisContainer.Count > 1;
            for (var i = 0; i < _tetrisContainer.Count; i++)
            {
                _tetrisContainer[i].CanGameRun = false;
                _tetrisContainer[i] = null;
            }

            _tetrisContainer.Clear();
            lbl_BestScore.Text = int.Parse(lbl_Score.Text) > int.Parse(lbl_BestScore.Text)
                ? lbl_Score.Text
                : lbl_BestScore.Text;
            lbl_2pBestScore.Text = int.Parse(lbl_2pScore.Text) > int.Parse(lbl_2pBestScore.Text)
                ? lbl_2pScore.Text
                : lbl_2pBestScore.Text;
            timer1.Enabled = false;
            _mediaPlayer.controls.stop();
            btn_GameStart.Enabled = true;
            btn_1vs1.Enabled = true;
            bool p1Win = int.Parse(lbl_Score.Text) > int.Parse(lbl_2pScore.Text);
            if (multiplayer)
            {
                if (p1Win)
                    MessageBox.Show(@"플레이어1 승리!", $@"Score: {lbl_Score.Text}");
                else
                    MessageBox.Show(@"플레이어2 승리!", $@"Score: {lbl_2pScore.Text}");
            }
            else
            {
                MessageBox.Show(@"Game Over!", $@"Score: {lbl_Score.Text}");
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            foreach (Tetris item in _tetrisContainer) item?.KeyBoardAction(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.URL == "")
                timer1.Enabled = false;
            _mediaPlayer.controls.play();
        }

        private async void btn_1vs1_Click(object sender, EventArgs e)
        {
            Size = new Size(700, 730);
            StartSetting();
            _tetrisPlayer1 = new Tetris(this, 1, lbl_Score, KeyboardPlayer2.GetInstence);
            _tetrisPlayer2 = new Tetris(this, 12, lbl_2pScore, KeyboardPlayer1.GetInstence);
            _tetrisContainer.Add(_tetrisPlayer1);
            _tetrisContainer.Add(_tetrisPlayer2);
            await Task.WhenAny(_tetrisContainer.Select(item => item.LoopDownAsync()).ToArray());
            GameEnd();
        }
    }
}