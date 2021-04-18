using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private readonly WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();
        private readonly GameController _gameController;

        public Form1()
        {
            if (File.Exists(@".\Sound\Tetris_BGM.mp3"))
                _mediaPlayer.URL = @".\Sound\Tetris_BGM.mp3";
            _mediaPlayer.settings.volume = 10;
            _mediaPlayer.controls.stop();
            _gameController = GameController.GetInstance;
            _gameController.GameEndAction = GameEnd;
            InitializeComponent();
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            Size = new Size(380, 880);
            StartSetting();
            _gameController.PlayerAdd(new Tetris(this, 1, lbl_Score, KeyboardPlayer1.GetInstance, 1));
            await _gameController.GameStart();
        }

        private void StartSetting()
        {
            timer1.Enabled = true;
            _mediaPlayer.controls.play();
            btn_GameStart.Enabled = false;
            btn_1vs1.Enabled = false;
        }

        private void GameEnd(string gameAlertMsg)
        {
            timer1.Enabled = false;
            _mediaPlayer.controls.stop();
            btn_GameStart.Enabled = true;
            btn_1vs1.Enabled = true;

            lbl_BestScore.Text = int.Parse(lbl_Score.Text) > int.Parse(lbl_BestScore.Text)
                ? lbl_Score.Text
                : lbl_BestScore.Text;

            MessageBox.Show(gameAlertMsg);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            _gameController.KeyBoardAction(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.URL == "")
                timer1.Enabled = false;
            _mediaPlayer.controls.play();
        }

        private async void btn_1vs1_Click(object sender, EventArgs e)
        {
            Size = new Size(700, 880);
            StartSetting();
            _gameController.PlayerAdd(new Tetris(this, 1, lbl_Score, KeyboardPlayer2.GetInstance, 1));
            _gameController.PlayerAdd(new Tetris(this, 12, lbl_2pScore, KeyboardPlayer1.GetInstance, 2));
            await _gameController.GameStart();
        }

        private void tBar_Volume_Scroll(object sender, EventArgs e)
        {
            _mediaPlayer.settings.volume = tBar_Volume.Value;
        }
    }
}