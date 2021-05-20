using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Tetris.Properties;
using WMPLib;

namespace Tetris
{
    public sealed partial class MainMenuForm : MetroForm
    {
        private readonly WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();
        private readonly Stack<TetrisPanel> _panels = new Stack<TetrisPanel>();

        public MainMenuForm()
        {
            InitializeComponent();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }

            return base.IsInputKey(keyData);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(@".\Sound\Tetris_BGM.mp3"))
                _mediaPlayer.URL = @".\Sound\Tetris_BGM.mp3";
            _mediaPlayer.settings.volume = Settings.Default.Volume;
            _mediaPlayer.controls.stop();
            GameController.GameEndAction = GameEnd;
        }

        private TetrisPanel NewPanel(PanelValue panelValue)
        {
            TetrisPanel p = new TetrisPanel(30)
            {
                Location = new Point(panelValue.PointX, panelValue.PointY),
                Size = new Size(panelValue.Width, panelValue.Height),
                BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                BackColor = Color.Black
            };
            Controls.Add(p);
            _panels.Push(p);
            return p;
        }

        private Weight GetWeight()
        {
            return File.Exists(@".\Weight.json")
                ? GeneticAlgorithm.WeightFileReader<Weight>(@".\Weight.json")
                : new Weight();
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            StartSetting(btn_GameStart);
            Size = new Size(690, 870);
            Tetris player1 = new Tetris(NewPanel(PanelValue.GetTetrisPanelToPlayer1()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer1()),
                lbl_Score, KeyboardPlayer1.GetInstance, 1);
            TetrisAI player2 = TetrisAI.GeneralMode(NewPanel(PanelValue.GetTetrisPanelToPlayer2()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer2()),
                lbl_2pScore, 2, GetWeight());
            GameController.PlayerAdd(player1);
            GameController.PlayerAdd(player2);
            await GameController.GameStart();
        }

        private async void btn_GeneticAlgorithm_Click(object sender, EventArgs e)
        {
            StartSetting(btn_GeneticAlgorithm);
            Size = new Size(675, 1000);
            await GeneticAlgorithm.AlgorithmStart(this, lbl_Score, lbl_BestScore, lbl_Generation, lbl_bestNum);
        }

        private async void btn_AI_Click(object sender, EventArgs e)
        {
            StartSetting(btn_AI);
            Size = new Size(360, 870);
            GameController.PlayerAdd(TetrisAI.AITestMode(NewPanel(PanelValue.GetTetrisPanelToPlayer1()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer1()),
                lbl_Score, 1, GetWeight()));
            await GameController.GameStart();
        }

        private async void btn_1vs1_Click(object sender, EventArgs e)
        {
            StartSetting(btn_1vs1);
            Size = new Size(690, 870);
            GameController.PlayerAdd(new Tetris(NewPanel(PanelValue.GetTetrisPanelToPlayer1()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer1()),
                lbl_Score, KeyboardPlayer2.GetInstance, 1));
            GameController.PlayerAdd(new Tetris(NewPanel(PanelValue.GetTetrisPanelToPlayer2()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer2()),
                lbl_2pScore, KeyboardPlayer1.GetInstance, 2));
            await GameController.GameStart();
        }


        private void StartSetting(MetroButton button)
        {
            if (GameController.GetPlayers().Count > 0)
                GameController.GameEnd();

            while (_panels.Count > 0)
                Controls.Remove(_panels.Pop());

            button.Enabled = false;
            button.Enabled = true;
            timer1.Enabled = true;
            _mediaPlayer.controls.play();
        }

        private void GameEnd(string gameAlertMsg)
        {
            timer1.Enabled = false;
            _mediaPlayer.controls.stop();

            lbl_BestScore.Text = int.Parse(lbl_Score.Text) > int.Parse(lbl_BestScore.Text)
                ? lbl_Score.Text
                : lbl_BestScore.Text;

            MessageBox.Show(gameAlertMsg);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            GameController.KeyBoardAction(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.URL == "")
                timer1.Enabled = false;
            _mediaPlayer.controls.play();
        }

        private void btn_Setting_Click(object sender, EventArgs e)
        {
            new SettingForm(_mediaPlayer).ShowDialog();
            btn_Setting.Enabled = false;
            btn_Setting.Enabled = true;
        }
    }
}