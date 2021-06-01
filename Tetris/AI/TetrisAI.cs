using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using Tetris.Properties;

namespace Tetris
{
    public class TetrisAI : Tetris
    {
        private static readonly object _labelLocker = new object();
        private readonly Label lbl_BestNum;

        private Func<int, int, Task> DoBlockMoveAsyncFunc;
        private Action<int, int> DrawColorAction;
        private Func<Task> LoopDownAsyncFunc;
        private Action ReDrawBlockAction;
        private Action ReSetBlockAction;
        private Action SetScoreTextAction;

        public Weight Weight { get; }

        private TetrisAI(TetrisPanel tetrisPanel, MetroPanel nextBlockPanel, Label lblScore, Label lblBestNum, int id,
            Weight weight)
            : base(tetrisPanel, nextBlockPanel, lblScore, null, id)
        {
            Weight = weight;
            lbl_BestNum = lblBestNum;
            DoBlockMoveAsyncFunc = DoBlockMoveAsyncNormal;
            SetScoreTextAction = base.SetScoreText;
            DrawColorAction = base.DrawColer;
            ReSetBlockAction = base.ReSetBlock;
            LoopDownAsyncFunc = base.LoopDownAsync;
            ReDrawBlockAction = base.ReDrawBlock;
        }

        protected override void SetScoreText()
        {
            SetScoreTextAction.Invoke();
        }

        protected override void ReDrawBlock()
        {
            ReDrawBlockAction.Invoke();
        }

        protected override void ReSetBlock()
        {
            ReSetBlockAction.Invoke();
        }

        protected override void DrawColer(int y, int x)
        {
            DrawColorAction.Invoke(y, x);
        }

        protected override async Task LoopDownAsync()
        {
            await LoopDownAsyncFunc.Invoke();
        }

        private async Task DoBlockMove(int optimalX, int optimalRotation)
        {
            await DoBlockMoveAsyncFunc.Invoke(optimalX, optimalRotation);
        }

        public override async Task GameStart()
        {
#pragma warning disable 4014
            base.GameStart();
#pragma warning restore 4014
            await AutoPlaying();
        }

        private async Task AutoPlaying()
        {
            while (GamePlaying)
            {
                (int x, int rotationNum) = FindOptimalPosAsync();
                await DoBlockMove(x, rotationNum);
            }
        }

        private static bool CanPutBlock(int currentX, int[,] block)
        {
            int size = block.GetLength(0);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                if (block[y, x] == 1 && (currentX + x < 0 || currentX + x >= 10))
                    return false;

            return true;
        }

        private (int, int) FindOptimalPosAsync()
        {
            (int X, int RotationNum) pos = (0, 0);
            int width = _tetrisBoard.GetLength(1);
            double max = int.MinValue;
            int[,] board = _tetrisBoard.Clone() as int[,];

            for (int i = 0; i < _block.BlockRotationCount[_block.BlockNum]; i++)
            {
                int[,] block = _block.BlockCreate(_block.BlockNum, i);
                for (int x = -block.GetLength(0) + 1; x < width; x++)
                {
                    if (!CanPutBlock(x, block)) continue;

                    int y = DownLocationCalc(-block.GetLength(0), x, block);
                    SetBoard(y, x, board, block, _block.BlockNum + 10);
                    double temp = GetBestCaseValue(y, x, board, block);

                    SetBoard(y, x, board, block, 0);
                    if (max >= temp) continue;
                    max = temp;
                    pos = (x, i);
                }
            }

            return pos;
        }

        private void SetBoard(int currentY, int currentX, int[,] board, int[,] block, int value)
        {
            for (int y = 0; y < block.GetLength(0); y++)
            for (int x = 0; x < block.GetLength(0); x++)
                if (block[y, x] == 1 && currentY + y >= 0)
                    board[currentY + y, currentX + x] = value;
        }

        private double GetBestCaseValue(int currentY, int currentX, int[,] board, int[,] block)
        {
            double value = 0;
            value += GetValue1(board, block, currentY);
            value += GetValue2(board);
            value += GetValue3(currentY, currentX, board, block);
            return value;
        }

        private double GetValue1(int[,] board, int[,] block, int currentY)
        {
            double value = 0;
            double blockHeightValue = 0;
            double lineClearValue = 0;
            int width = board.GetLength(1);
            int height = board.GetLength(0);
            for (int y = 0; y < height; y++)
            {
                if (HasLineClear(y))
                    lineClearValue++;

                for (int x = 0; x < width; x++)
                    if (board[y, x] > 10)
                        blockHeightValue += height - y;
            }

            for (int y = 0; y < -currentY; y++)
            for (int x = 0; x < block.GetLength(1); x++)
                if (block[y, x] == 1)
                    blockHeightValue += height + (-currentY - y);

            bool HasLineClear(int y)
            {
                for (int x = 0; x < width; x++)
                    if (board[y, x] <= 10)
                        return false;
                return true;
            }

            value += blockHeightValue * Weight.BlockHeightValue;
            value += lineClearValue * Weight.LineClearValue;
            return value;
        }

        private double GetValue2(int[,] board)
        {
            double value = 0;
            double holeValue = 0;
            double blockedValue = 0;
            int height = board.GetLength(0);
            int width = board.GetLength(1);
            for (int x = 0; x < width; x++)
            for (int y = height - 1; y >= 0; y--)
            {
                if (board[y, x] > 10) continue;
                int cnt = 0;

                while (y >= 0 && board[y, x] <= 10)
                {
                    cnt++;
                    y--;
                }

                if (y < 0) break;

                holeValue += cnt;

                while (y >= 0 && board[y, x] > 10)
                {
                    blockedValue++;
                    y--;
                }
            }

            value += holeValue * Weight.HoleValue;
            value += blockedValue * Weight.BlockedValue;
            return value;
        }

        private double GetValue3(int currentY, int currentX, int[,] board, int[,] block)
        {
            double value = 0;
            double sideValue = 0;
            double floorValue = 0;
            double blockValue = 0;

            int size = block.GetLength(0);
            int height = board.GetLength(0);
            int width = board.GetLength(1);

            int[] dy = { -1, 0, 1, 0 };
            int[] dx = { 0, 1, 0, -1 };

            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                if (block[y, x] != 1) continue;
                for (int i = 0; i < dy.Length; i++)
                    if (currentX + x + dx[i] < 0 || currentX + x + dx[i] >= width)
                        sideValue++;
                    else if (currentY + y + dy[i] >= height)
                        floorValue++;
                    else if (currentY + y + dy[i] >= 0 && board[currentY + y + dy[i], currentX + x + dx[i]] > 10)
                        if (y + dy[i] >= size || y + dy[i] < 0 || x + dx[i] >= size || x + dx[i] < 0
                            || block[y + dy[i], x + dx[i]] != 1)
                            blockValue++;
            }

            value += sideValue * Weight.SideValue;
            value += blockValue * Weight.BlockValue;
            value += floorValue * Weight.FloorValue;
            return value;
        }

        #region SetMode

        private async Task DoBlockMoveAsyncNormal(int optimalX, int optimalRotation)
        {
            int delay = GetDelay(_tetrisBoard);
            Action direction = _currentX < optimalX ? MoveRight : new Action(MoveLeft);
            int end = Math.Abs(_currentX - optimalX);
            for (int i = 0; i < optimalRotation; i++)
            {
                await Task.Delay(delay);
                RotationBlock();
            }

            for (int i = 0; i < end; i++)
            {
                await Task.Delay(delay);
                direction.Invoke();
            }

            await Task.Delay(delay);
            HardDown();

            int GetDelay(int[,] board)
            {
                for (int y = 0; y < board.GetLength(0); y++)
                for (int x = 0; x < board.GetLength(1); x++)
                    if (board[y, x] > 10)
                    {
                        int h = Math.Min(board.GetLength(0) - y, 18);
                        return (18 - h) * 19;
                    }

                return 0;
            }
        }

        private Task DoBlockMoveAsyncAtGenetic(int optimalX, int optimalRotation)
        {
            _block.SetRotationBlock(optimalRotation);
            _currentX = optimalX;
            HardDown();
            return Task.CompletedTask;
        }

        private void SetScoreTextAtGenetic()
        {
            lock (_labelLocker)
            {
                if (!long.TryParse(_lblScore.Text, out long i)) return;
                if (i > Score) return;
                if (!_lblScore.InvokeRequired)
                {
                    _lblScore.Text = Score.ToString();
                    lbl_BestNum.Text = $@"현재 번호: {PlayerId}번";
                    return;
                }
            }

            _lblScore.Invoke((MethodInvoker) SetScoreText);
        }

        private void DrawColerAtNone(int y, int x)
        {
        }

        private void ReSetBlockAtGenetic()
        {
            _block.NewBlock();
            _currentY = 0 - _block.Block.GetLength(0);
            _currentX = Random.Next(0, 11 - _block.Block.GetLength(0));
        }

        private Task LoopDownAsyncAtGenetic()
        {
            return Task.CompletedTask;
        }

        private void ReDrawBlockAtGenetic()
        {
            RemoveRedBlock();
            MoveRedBlock();
        }

        private void SetAiTestMode()
        {
            ReDrawBlockAction = ReDrawBlockAtGenetic;
            DoBlockMoveAsyncFunc = DoBlockMoveAsyncAtGenetic;
            LoopDownAsyncFunc = LoopDownAsyncAtGenetic;
        }

        private void SetGeneticMode()
        {
            ReDrawBlockAction = ReDrawBlockAtGenetic;
            DoBlockMoveAsyncFunc = DoBlockMoveAsyncAtGenetic;
            ReSetBlockAction = ReSetBlockAtGenetic;
            LoopDownAsyncFunc = LoopDownAsyncAtGenetic;
            SetScoreTextAction = SetScoreTextAtGenetic;
            if (!Settings.Default.CanRendering)
                DrawColorAction = DrawColerAtNone;
        }

        public void SetRenderingAtGenetic()
        {
            if (Settings.Default.CanRendering)
                DrawColorAction = base.DrawColer;
            else
                DrawColorAction = DrawColerAtNone;
        }

        public static TetrisAI AITestMode(TetrisPanel tetrisPanel, MetroPanel nextBlockPanel, Label lblScore, int id,
            Weight weight)
        {
            TetrisAI t = new TetrisAI(tetrisPanel, nextBlockPanel, lblScore, null, id, weight);
            t.SetAiTestMode();
            return t;
        }

        public static TetrisAI GeneticMode(TetrisPanel tetrisPanel, Label lblScore, Label lblBestNum, int id,
            Weight weight)
        {
            TetrisAI t = new TetrisAI(tetrisPanel, null, lblScore, lblBestNum, id, weight);
            t.SetGeneticMode();
            return t;
        }

        public static TetrisAI GeneralMode(TetrisPanel tetrisPanel, MetroPanel nextBlockPanel, Label lblScore, int id,
            Weight weight)
        {
            return new TetrisAI(tetrisPanel, nextBlockPanel, lblScore, null, id, weight);
        }

        #endregion
    }
}