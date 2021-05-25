using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Newtonsoft.Json;
using Tetris.Properties;
using WMPLib;

namespace Tetris
{
    public sealed partial class MainForm : MetroForm
    {
        private readonly WindowsMediaPlayer _mediaPlayer = new WindowsMediaPlayer();
        private readonly Stack<TetrisPanel> _panels = new Stack<TetrisPanel>();

        public MainForm()
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
            BgmSetting();
            KeyboardSetting();
            GameController.GameEndAction = GameEnd;
        }

        private void BgmSetting()
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath.Sound);
                string name = Path.GetFileName(FilePath.Sound);
                string path = Directory.GetFiles(dir, name).First();
                _mediaPlayer.URL = path;
            }
            catch
            {
                MessageBox.Show(@"음악 파일을 찾을수 없습니다!");
            }
            finally
            {
                _mediaPlayer.settings.volume = Settings.Default.Volume;
                _mediaPlayer.controls.stop();
            }
        }

        private void KeyboardSetting()
        {
            try
            {
                List<Keyboard> list = FileLoad<List<Keyboard>>(FilePath.KeySetting);
                Keyboard.SetKeyboard(list[0], Keyboard.GetPlayer1);
                Keyboard.SetKeyboard(list[1], Keyboard.GetPlayer2);
            }
            catch
            {
                Keyboard.SetDefault();
                Keyboard.KeySettingFileSave();
            }
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

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            try
            {
                StartSetting(btn_GameStart);
                Tetris player1 = new Tetris(NewPanel(PanelValue.GetTetrisPanelToPlayer1()),
                    NewPanel(PanelValue.GetNextBlockPanelToPlayer1()),
                    lbl_Score, Keyboard.GetPlayer1, 1);
                TetrisAI player2 = TetrisAI.GeneralMode(NewPanel(PanelValue.GetTetrisPanelToPlayer2()),
                    NewPanel(PanelValue.GetNextBlockPanelToPlayer2()),
                    lbl_2pScore, 2, FileLoad<Weight>(FilePath.Weight));
                GameController.PlayerAdd(player1);
                GameController.PlayerAdd(player2);
                Size = new Size(690, 870);
                await GameController.GameStart();
            }
        #pragma warning disable 168
            catch (DirectoryNotFoundException _)
        #pragma warning restore 168
            {
                MessageBox.Show(@"학습된 AI가 없습니다!");
            }
        #pragma warning disable 168
            catch (FileNotFoundException _)
        #pragma warning restore 168
            {
                MessageBox.Show(@"학습된 AI가 없습니다!");
            }
        }

        private async void btn_GeneticAlgorithm_Click(object sender, EventArgs e)
        {
            StartSetting(btn_GeneticAlgorithm);
            Size = new Size(675, 1000);
            await GeneticAlgorithm.AlgorithmStart(this, lbl_Score, lbl_BestScore, lbl_Generation, lbl_bestNum);
        }

        private async void btn_AI_Click(object sender, EventArgs e)
        {
            try
            {
                StartSetting(btn_AI);
                GameController.PlayerAdd(TetrisAI.AITestMode(NewPanel(PanelValue.GetTetrisPanelToPlayer1()),
                    NewPanel(PanelValue.GetNextBlockPanelToPlayer1()),
                    lbl_Score, 1, FileLoad<Weight>(FilePath.Weight)));
                Size = new Size(360, 870);
                await GameController.GameStart();
            }
            #pragma warning disable 168
            catch (DirectoryNotFoundException _)
            #pragma warning restore 168
            {
                MessageBox.Show(@"학습된 AI가 없습니다!");
            }
            #pragma warning disable 168
            catch (FileNotFoundException _)
            #pragma warning restore 168
            {
                MessageBox.Show(@"학습된 AI가 없습니다!");
            }
        }

        private async void btn_1vs1_Click(object sender, EventArgs e)
        {
            StartSetting(btn_1vs1);
            Size = new Size(690, 870);
            GameController.PlayerAdd(new Tetris(NewPanel(PanelValue.GetTetrisPanelToPlayer1()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer1()),
                lbl_Score, Keyboard.GetPlayer2, 1));
            GameController.PlayerAdd(new Tetris(NewPanel(PanelValue.GetTetrisPanelToPlayer2()),
                NewPanel(PanelValue.GetNextBlockPanelToPlayer2()),
                lbl_2pScore, Keyboard.GetPlayer1, 2));
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

        private void btn_Setting_Click(object sender, EventArgs e)
        {
            new OptionForm(_mediaPlayer).ShowDialog();
            btn_Setting.Enabled = false;
            btn_Setting.Enabled = true;
        }

        public static void FileSave<T>(T obj, string path) where T : class
        {
            string di = Path.GetDirectoryName(path);
            if (!Directory.Exists(di))
                Directory.CreateDirectory(di);

            using (StreamWriter stream = new StreamWriter(path))
            using (JsonTextWriter writer = new JsonTextWriter(stream))
            {
                new JsonSerializer().Serialize(writer, obj);
            }
        }

        public static T FileLoad<T>(string path)
        {
            using (StreamReader stream = new StreamReader(path))
            using (JsonTextReader reader = new JsonTextReader(stream))
            {
                return new JsonSerializer().Deserialize<T>(reader);
            }
        }
    }
}