using System;
using System.Media;
using System.Windows.Forms;
using System.Windows.Media;
using WMPLib;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Tetris tetris;
        SoundPlayer player = new SoundPlayer();
        public Form1()
        {
            player.SoundLocation = "Tetris_BGM.wav";
            InitializeComponent();
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            tetris = new Tetris(this);
            player.Play();
            timer1.Enabled = true;
            btn_GameStart.Click -= new EventHandler(btn_GameStart_Click);
            btn_GameStart.Enabled = false;
            await tetris.LoopDownAsync(lbl_Score);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Left)
                tetris.MoveLeft();
            if (e.KeyCode == Keys.Right)
                tetris.MoveRight();
            if (e.KeyCode == Keys.Down)
                tetris.MoveDown(lbl_Score);
            if (e.KeyCode == Keys.Up)
                tetris.RotationBlock();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            player.Play();
        }
    }
}
