using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class TetrisAI
    {
        private static Dictionary<int, TetrisAI> TetrisAi { get; } = new Dictionary<int, TetrisAI>();

        private Func<int, int, int[,], int> DownLocationCalc;
        private IKeyboardSetting _keyboardSetting;
        private TetrisBlock _block;

        private TetrisAI(Tetris tetris)
        {
            tetris.ReSetBlockEvent += FindOptimalPosAsync;
            tetris.ConnectingToAi += ConnectingToTetris;
            tetris.GameEndEvent += DisconnectToTetris;
        }

        private static void DeBugging(int[,] board, Func<bool> condition)
        {
            if (!condition.Invoke()) return;
            Console.Clear();
            int width = board.GetLength(1);
            int height = board.GetLength(0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write($"{board[y, x],2} ");
                }

                Console.WriteLine();
            }
        }

        public static void AutoPlay(Tetris tetris)
        {
            TetrisAi.Add(tetris.PlayerId, new TetrisAI(tetris));
        }

        private void DisconnectToTetris(object sender, EventArgs e)
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
            for (int y = 0; y < board.GetLength(0); y++)
            for (int x = 0; x < board.GetLength(1); x++)
            {
                if (board[y, x] > 10)
                    return Math.Min(board.GetLength(0) - y, 16);
            }

            return -1;
        }

        private async Task AutoPlaying(Tetris tetris, TetrisEventArgs e, int optimalX, int optimalRotation)
        {
            int delay = (16 - GetCurrentHeight(e.TetrisBoard)) * 19;
            Keys keyData = e.CurrentX < optimalX ? _keyboardSetting.RightCode : _keyboardSetting.LeftCode;
            int end = Math.Abs(e.CurrentX - optimalX);
            for (int i = 0; i < optimalRotation; i++)
            {
                await Task.Delay(delay);
                tetris.KeyBoardAction(new KeyEventArgs(_keyboardSetting.RotationCode));
            }

            for (int i = 0; i < end; i++)
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
            {
                if (block[y, x] == 1 && (currentX + x < 0 || currentX + x >= 10))
                    return false;
            }

            return true;
        }

        private async void FindOptimalPosAsync(object sender, TetrisEventArgs e)
        {
            await Task.Delay(1);
            (int X, int RotationNum) pos = (0, 0);
            int width = e.TetrisBoard.GetLength(1);
            double max = int.MinValue;

            for (int i = 0; i < _block.BlockRotationCount[_block.BlockNum]; i++)
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
            for (int y = 0; y < block.GetLength(0); y++)
            for (int x = 0; x < block.GetLength(0); x++)
            {
                if (block[y, x] == 1 && currentY + y >= 0)
                    newBoard[currentY + y, currentX + x] = _block.BlockNum + 10;
            }

            return newBoard;
        }

        private static double GetBestCaseValue(int currentY, int currentX, int[,] board, int[,] block)
        {
            double value = 0;
            value += GetValue1(board, block, currentY);
            value += GetHoleValue(board);


            return value;
        }

        private static double GetValue1(int[,] board, int[,] block, int currentY)
        {
            double totalValue = 0;
            double blockHeightValue = 0;
            double lineClearValue = 0;
            int width = board.GetLength(1);
            int height = board.GetLength(0);
            for (int y = 0; y < height; y++)
            {
                if (HasLineClear(y))
                    lineClearValue++;

                for (int x = 0; x < width; x++)
                {
                    if (board[y, x] > 10)
                        blockHeightValue += height - y;
                }
            }

            for (int y = 0; y < -currentY; y++)
            for (int x = 0; x < block.GetLength(1); x++)
            {
                if (block[y, x] == 1)
                    blockHeightValue += height + (-currentY - y);
            }

            bool HasLineClear(int y)
            {
                for (var x = 0; x < width; x++)
                    if (board[y, x] <= 10)
                        return false;
                return true;
            }

            totalValue += blockHeightValue * -3.78;
            totalValue += lineClearValue * 8.2;
            return totalValue;
        }

        private static double GetHoleValue(int[,] board)
        {
            double value = 0;
            int height = board.GetLength(0);
            int width = board.GetLength(1);
            for (var x = 0; x < width; x++)
            {
                int cnt = 0;
                for (int y = height - 1; y >= 0; y--)
                {
                    if (board[y, x] > 10) continue;

                    while (y >= 0 && board[y, x] <= 10)
                    {
                        cnt++; y--;
                    }
                    if (y < 0)
                        break;
                    value += cnt;
                    cnt = 0;
                }
            }

            return value * -8.8;
        }
    }
}