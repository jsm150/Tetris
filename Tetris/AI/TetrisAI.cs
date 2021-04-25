using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class TetrisAI
    {
        private TetrisBlock _block;
        private IKeyboardSetting _keyboardSetting;

        private Func<int, int, int[,], int> DownLocationCalc;

        private TetrisAI(Tetris tetris)
        {
            tetris.ReSetBlockEvent += FindOptimalPosAsync;
            tetris.ConnectingToAi += ConnectingToTetris;
            tetris.GameEndEvent += DisconnectToTetris;
        }

        private static Dictionary<int, TetrisAI> TetrisAi { get; } = new Dictionary<int, TetrisAI>();

        private static void DeBugging(int[,] board, Func<bool> condition)
        {
            if (!condition.Invoke()) return;
            Console.Clear();
            int width = board.GetLength(1);
            int height = board.GetLength(0);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++) Console.Write($"{board[y, x],2} ");

                Console.WriteLine();
            }
        }

        public static void AutoPlay(Tetris tetris)
        {
            TetrisAi.Add(tetris.PlayerId, new TetrisAI(tetris));
        }

        private static void DisconnectToTetris(object sender, EventArgs e)
        {
            var tetris = sender as Tetris;
            TetrisAi.Remove(tetris.PlayerId);
        }

        private void ConnectingToTetris(object sender, AiInitEventArgs e)
        {
            DownLocationCalc = e.DownLocationCalc;
            _block = e.TetrisBlock;
            _keyboardSetting = e.KeyboardSetting;
        }

        private int GetCurrentHeight(int[,] board)
        {
            for (var y = 0; y < board.GetLength(0); y++)
            for (var x = 0; x < board.GetLength(1); x++)
                if (board[y, x] > 10)
                    return Math.Min(board.GetLength(0) - y, 16);

            return -1;
        }

        private async Task AutoPlaying(Tetris tetris, TetrisEventArgs e, int optimalX, int optimalRotation)
        {
            int delay = GameController.GetPlayers().Count <= 1 ? 0 : (16 - GetCurrentHeight(e.TetrisBoard)) * 19;
            Keys keyData = e.CurrentX < optimalX ? _keyboardSetting.RightCode : _keyboardSetting.LeftCode;
            int end = Math.Abs(e.CurrentX - optimalX);
            for (var i = 0; i < optimalRotation; i++)
            {
                await Task.Delay(delay);
                tetris.KeyBoardAction(new KeyEventArgs(_keyboardSetting.RotationCode));
            }

            for (var i = 0; i < end; i++)
            {
                await Task.Delay(delay);
                tetris.KeyBoardAction(new KeyEventArgs(keyData));
            }

            tetris.KeyBoardAction(new KeyEventArgs(_keyboardSetting.HardDownCode));
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

        private async void FindOptimalPosAsync(object sender, TetrisEventArgs e)
        {
            await Task.Delay(1);
            (int X, int RotationNum) pos = (0, 0);
            int width = e.TetrisBoard.GetLength(1);
            double max = int.MinValue;

            for (var i = 0; i < _block.BlockRotationCount[_block.BlockNum]; i++)
            {
                int[,] block = _block.BlockCreate(_block.BlockNum, i);
                for (int x = -block.GetLength(0) + 1; x < width; x++)
                {
                    if (!CanPutBlock(x, block)) continue;
                    int y = DownLocationCalc(-block.GetLength(0), x, block);
                    int[,] board = AttachToBoard(y, x, e.TetrisBoard, block);
                    //DeBugging(board, () => _block.BlockNum == 1 && y < 2);
                    double temp = GetBestCaseValue(y, x, board, block);

                    if (max >= temp) continue;
                    max = temp;
                    pos = (x, i);
                }
            }

            await AutoPlaying(sender as Tetris, e, pos.X, pos.RotationNum);
        }

        private int[,] AttachToBoard(int currentY, int currentX, int[,] board, int[,] block)
        {
            var newBoard = (int[,]) board.Clone();
            for (var y = 0; y < block.GetLength(0); y++)
            for (var x = 0; x < block.GetLength(0); x++)
                if (block[y, x] == 1 && currentY + y >= 0)
                    newBoard[currentY + y, currentX + x] = _block.BlockNum + 10;

            return newBoard;
        }

        private static double GetBestCaseValue(int currentY, int currentX, int[,] board, int[,] block)
        {
            double value = 0;
            value += GetValue1(board, block, currentY);
            value += GetValue2(board);
            value += GetValue3(currentY, currentX, board, block);
            return value;
        }

        private static double GetValue1(int[,] board, int[,] block, int currentY)
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

            value += blockHeightValue * -3.4;
            value += Math.Pow(lineClearValue, 2) * 8.2;
            return value;
        }

        private static double GetValue2(int[,] board)
        {
            double value = 0;
            double holeValue = 0;
            double blockedValue = 0;
            int height = board.GetLength(0);
            int width = board.GetLength(1);
            for (var x = 0; x < width; x++)
            {
                var cnt = 0;
                for (int y = height - 1; y >= 0; y--)
                {
                    if (board[y, x] > 10) continue;

                    while (y >= 0 && board[y, x] <= 10)
                    {
                        cnt++;
                        y--;
                    }

                    if (y < 0) break;

                    holeValue += cnt;
                    cnt = 0;

                    while (y >= 0 && board[y, x] > 10)
                    {
                        blockedValue++;
                        y--;
                    }
                }
            }

            value += holeValue * -20;
            value += blockedValue * -1;
            return value;
        }

        private static double GetValue3(int currentY, int currentX, int[,] board, int[,] block)
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

            value += sideValue * 2.5;
            value += blockValue * 3.7;
            value += floorValue * 4.0;
            return value;
        }
    }
}