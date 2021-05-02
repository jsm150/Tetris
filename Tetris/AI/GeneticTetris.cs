using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    // 유전 알고리즘을 돌릴때 쓰는 테트리스 클래스
    public class GeneticTetris : TetrisAI
    {
        private readonly int _offsetY;

        public GeneticTetris(Form1 f, int offsetX, int offsetY, int id, Weight weight)
            : base(f, offsetX, new Label(), id, weight)
        {
            _offsetY = offsetY;
        }

        protected override Task DoBlockMove(int optimalX, int optimalRotation)
        {
            _block.SetRotationBlock(optimalRotation);
            _currentX = optimalX;
            HardDown();
            return Task.CompletedTask;
        }

        protected override void DrawColer(int y, int x, int offsetY, int sizeX, int sizeY)
        {
            base.DrawColer(y, x, _offsetY, 10, 10);
        }

        protected override void NextBlockPreview()
        {
        }

        protected override Task LoopDownAsync()
        {
            return Task.CompletedTask;
        }

        protected override void ReDrawBlock()
        {
            RemoveRedBlock();
            MoveRedBlock();
        }
    }
}