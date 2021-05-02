using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class TetrisAI : Tetris
    {
        public TetrisAI(Form1 f, int offsetX, Label label, int id, Weight weight)
            : base(f, offsetX, label, null, id)
        {
            Weight = weight;
        }

        public Weight Weight { get; }

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
        

        protected virtual async Task DoBlockMove(int optimalX, int optimalRotation)
        {
            int delay = GetDelay(_tetrisBoard);
            Action direction = _currentX < optimalX ? MoveRight : new Action(MoveLeft);
            int end = Math.Abs(_currentX - optimalX);
            for (var i = 0; i < optimalRotation; i++)
            {
                await Task.Delay(delay);
                RotationBlock();
            }

            for (var i = 0; i < end; i++)
            {
                await Task.Delay(delay);
                direction.Invoke();
            }

            HardDown();

            int GetDelay(int[,] board)
            {
                for (var y = 0; y < board.GetLength(0); y++)
                for (var x = 0; x < board.GetLength(1); x++)
                    if (board[y, x] > 10)
                    {
                        int h = Math.Min(board.GetLength(0) - y, 16);
                        return GameController.GetPlayers().Count <= 1 ? 0 : (16 - h) * 19;
                    }

                return 0;
            }
        }

        private static bool CanPutBlock(int currentX, int[,] block)
        {
            int size = block.GetLength(0);
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                if (block[y, x] == 1 && (currentX + x < 0 || currentX + x >= 10))
                    return false;

            return true;
        }

        private (int, int) FindOptimalPosAsync()
        {
            (int X, int RotationNum) pos = (0, 0);
            int width = _tetrisBoard.GetLength(1);
            double max = int.MinValue;

            for (var i = 0; i < _block.BlockRotationCount[_block.BlockNum]; i++)
            {
                int[,] block = _block.BlockCreate(_block.BlockNum, i);
                for (int x = -block.GetLength(0) + 1; x < width; x++)
                {
                    if (!CanPutBlock(x, block)) continue;

                    int y = DownLocationCalc(-block.GetLength(0), x, block);
                    int[,] board = AttachToBoard(y, x, (int[,]) _tetrisBoard.Clone(), block);
                    double temp = GetBestCaseValue(y, x, board, block);

                    if (max >= temp) continue;
                    max = temp;
                    pos = (x, i);
                }
            }

            return pos;
        }

        private int[,] AttachToBoard(int currentY, int currentX, int[,] board, int[,] block)
        {
            for (var y = 0; y < block.GetLength(0); y++)
            for (var x = 0; x < block.GetLength(0); x++)
                if (block[y, x] == 1 && currentY + y >= 0)
                    board[currentY + y, currentX + x] = _block.BlockNum + 10;

            return board;
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
            for (var y = 0; y < height; y++)
            {
                if (HasLineClear(y))
                    lineClearValue++;

                for (var x = 0; x < width; x++)
                    if (board[y, x] > 10)
                        blockHeightValue += height - y;
            }

            for (var y = 0; y < -currentY; y++)
            for (var x = 0; x < block.GetLength(1); x++)
                if (block[y, x] == 1)
                    blockHeightValue += height + (-currentY - y);

            bool HasLineClear(int y)
            {
                for (var x = 0; x < width; x++)
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
            for (var x = 0; x < width; x++)
            for (int y = height - 1; y >= 0; y--)
            {
                if (board[y, x] > 10) continue;
                var cnt = 0;

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

            int height = block.GetLength(0);
            int width = block.GetLength(1);
            int size = block.GetLength(0);

            int[] dy = {-1, 0, 1, 0};
            int[] dx = {0, 1, 0, -1};

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                if (block[y, x] != 1) continue;
                for (var i = 0; i < dy.Length; i++)
                    if (currentX + x + dx[i] < 0 || currentX + x + dx[i] >= board.GetLength(1))
                        sideValue++;
                    else if (currentY + y + dy[i] >= board.GetLength(0))
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
    }
}