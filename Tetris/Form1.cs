using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tetris
{
    public partial class Form1 : Form
    {
        Tetris tetris = new Tetris();
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void btn_GameStart_Click(object sender, EventArgs e)
        {
            tetris.TetrisInit(this);
            tetris.NewBlock();
            tetris.BlockCreate();
            btn_GameStart.Click -= new EventHandler(btn_GameStart_Click);
            btn_GameStart.Enabled = false;
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
            {
                tetris.MoveDown();
            }
        }

    }
}
