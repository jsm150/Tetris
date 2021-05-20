using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace Tetris
{
    public class TetrisPanel : MetroPanel
    {
        public static Semaphore DrawLocker { get; } = new Semaphore(1, 1);
        private readonly int _blockSize;
        private int[,] _tetrisBoard;
        private TetrisBlock _block;

        public TetrisPanel(int blockSize)
        {
            _blockSize = blockSize;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_tetrisBoard is null) return;

            for (var y = 0; y < _tetrisBoard.GetLength(0); y++)
            for (var x = 0; x < _tetrisBoard.GetLength(1); x++)
                DrawColer(y, x, e.Graphics);
        }

        public void SetValue(int[,] tetrisBoard, TetrisBlock block)
        {
            _tetrisBoard = tetrisBoard;
            _block = block;
        }

        public void DrawColer(int y, int x, Graphics g)
        {
            DrawLocker.WaitOne();

            switch (_tetrisBoard[y, x])
            {
                case 0:
                    g.FillRectangle(Brushes.Black, x * _blockSize, y * _blockSize, _blockSize, _blockSize);
                    g.DrawRectangle(new Pen(Brushes.Black), x * _blockSize, y * _blockSize, _blockSize, _blockSize);
                    break;
                case 1:
                    g.FillRectangle(_block.BlockColor[_block.BlockNum], x * _blockSize, y * _blockSize, _blockSize,
                        _blockSize);
                    g.DrawRectangle(new Pen(Brushes.Black), x * _blockSize, y * _blockSize, _blockSize, _blockSize);
                    break;
                case 3:
                    g.DrawRectangle(new Pen(Brushes.DarkGray), x * _blockSize, y * _blockSize, _blockSize, _blockSize);
                    break;
                case 18:
                    g.FillRectangle(_block.BlockColor[_tetrisBoard[y, x] - 10], x * _blockSize,
                        y * _blockSize,
                        _blockSize, _blockSize);
                    g.DrawRectangle(new Pen(Brushes.Black), x * _blockSize, y * _blockSize, _blockSize, _blockSize);
                    break;
                default:
                {
                    if (_tetrisBoard[y, x] > 10 && _tetrisBoard[y, x] <= 17)
                    {
                        g.FillRectangle(_block.BlockColor[_tetrisBoard[y, x] - 10], x * _blockSize,
                            y * _blockSize, _blockSize, _blockSize);
                        g.DrawRectangle(new Pen(Brushes.Black), x * _blockSize, y * _blockSize, _blockSize, _blockSize);
                    }

                    break;
                }
            }

            DrawLocker.Release();
        }
    }
}