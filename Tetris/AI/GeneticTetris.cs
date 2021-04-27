using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    // 유전 알고리즘을 돌릴때 쓰는 테트리스 클래스
    public class GeneticTetris : Tetris
    {
        private readonly int _offsetY;

        public GeneticTetris(Form1 f, int offsetX, int offsetY, IKeyboardSetting key, int id) 
            : base(f, offsetX, new Label(), key, id)
        {
            _offsetY = offsetY;
        }

        protected override void DrawColer(int y, int x, int offsetY, int sizeX, int sizeY)
        {
            base.DrawColer(y, x, _offsetY, 10, 10);
        }

        protected override void NextBlockPreview()
        {
        }

        protected override async Task LoopDownAsync()
        {
            while (true)
            {
                await Task.Delay(500);
                if (GamePlaying) continue;

                OnGameEnd();
                break;
            }
        }

        protected override void ReDrawBlock()
        {
            RemoveRedBlock();
            MoveRedBlock();
        }
    }
}