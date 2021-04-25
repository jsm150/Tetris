﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tetris.Properties;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private readonly WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(@".\Sound\Tetris_BGM.mp3"))
                _mediaPlayer.URL = @".\Sound\Tetris_BGM.mp3";
            _mediaPlayer.settings.volume = Settings.Default.Volume;
            tBar_Volume.Value = Settings.Default.Volume;
            _mediaPlayer.controls.stop();
            GameController.GameEndAction = GameEnd;
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            Size = new Size(380, 880);
            StartSetting();
            var player1 = new Tetris(this, 1, lbl_Score, KeyboardPlayer1.GetInstance, 1);
            GameController.PlayerAdd(player1);
            //TetrisAI.AutoPlay(player1);
            await GameController.GameStart();
        }

        private void StartSetting()
        {
            timer1.Enabled = true;
            _mediaPlayer.controls.play();
            btn_GameStart.Enabled = false;
            btn_1vs1.Enabled = false;
            btn_VsAI.Enabled = false;
        }

        private void GameEnd(string gameAlertMsg)
        {
            timer1.Enabled = false;
            _mediaPlayer.controls.stop();
            btn_GameStart.Enabled = true;
            btn_1vs1.Enabled = true;
            btn_VsAI.Enabled = true;

            lbl_BestScore.Text = int.Parse(lbl_Score.Text) > int.Parse(lbl_BestScore.Text)
                ? lbl_Score.Text
                : lbl_BestScore.Text;

            MessageBox.Show(gameAlertMsg);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            GameController.KeyBoardAction(e);
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
            GameController.PlayerAdd(new Tetris(this, 1, lbl_Score, KeyboardPlayer2.GetInstance, 1));
            GameController.PlayerAdd(new Tetris(this, 12, lbl_2pScore, KeyboardPlayer1.GetInstance, 2));
            await GameController.GameStart();
        }

        private void tBar_Volume_Scroll(object sender, EventArgs e)
        {
            _mediaPlayer.settings.volume = tBar_Volume.Value;
            Settings.Default.Volume = tBar_Volume.Value;
            Settings.Default.Save();
        }

        private async void btn_VsAI_Click(object sender, EventArgs e)
        {
            Size = new Size(700, 880);
            StartSetting();
            var player1 = new Tetris(this, 1, lbl_Score, KeyboardPlayer1.GetInstance, 1);
            var player2 = new Tetris(this, 12, lbl_2pScore, KeyboardPlayer2.GetInstance, 2);
            GameController.PlayerAdd(player1);
            GameController.PlayerAdd(player2);
            TetrisAI.AutoPlay(player2);
            await GameController.GameStart();
        }
    }
}