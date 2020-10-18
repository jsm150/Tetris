using System;
using System.Windows.Forms;



namespace Tetris
{
    public partial class Form1 : Form
    {
        Tetris tetris;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btn_GameStart_Click(object sender, EventArgs e)
        {
            tetris = new Tetris(this);
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

    }
}
