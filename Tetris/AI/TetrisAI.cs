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
        private readonly Action<KeyEventArgs> KeyboardAction;
        private IKeyboardSetting _keyboardSetting;
        private TetrisBlock _block;
        private int _playerId;

        public TetrisAI(Tetris tetris)
        {
            tetris.AutoPlayer += FindOptimalPosAsync;
            tetris.ConnectingToAi += ConnectingToTetris;
            tetris.GameEnd += DisconnectToTetris;
            KeyboardAction = tetris.KeyBoardAction;
            _playerId = tetris.PlayerId;
        }

        private static void DeBugging(int[,] board)
        {
            Console.Clear();
            int width = board.GetLength(1);
            int height = board.GetLength(0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write($"{board[y, x], 2} ");
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
            TetrisAi.Remove(_playerId);
        }

        private async Task AutoPlaying(TetrisEventArgs e, int optimalX, int optimalRotation)
        {
            Keys keyData = e.CurrentX < optimalX ? _keyboardSetting.RightCode : _keyboardSetting.LeftCode;
            int end = Math.Abs(e.CurrentX - optimalX);
            await Task.Delay(1);
            for (int i = 0; i < optimalRotation; i++)
            {
                await Task.Delay(1);
                KeyboardAction(new KeyEventArgs(_keyboardSetting.RotationCode));
            }

            for (int i = 0; i < end; i++)
            {
                await Task.Delay(1);
                KeyboardAction(new KeyEventArgs(keyData));
            }

            KeyboardAction(new KeyEventArgs(_keyboardSetting.HardDownCode));
        }

        private void ConnectingToTetris(object sender, AiInitEventArgs e)
        {
            DownLocationCalc = e.DownLocationCalc;
            _block = e.TetrisBlock;
            _keyboardSetting = e.KeyboardSetting;
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
                    //DeBugging(board);
                    double temp = GetBestCaseValue(y, x, board);

                    if (max >= temp) continue;
                    max = temp;
                    pos = (x, i);
                }
            }

            await AutoPlaying(e, pos.X, pos.RotationNum);
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

        private static double GetBestCaseValue(int currentY, int currentX, int[,] board)
        {
            double value = 0;
            value += GetValue1(board);
            value += GetHoleValue(board);


            return value;
        }

        private static double GetValue1(int[,] board)
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

                var cnt = 0;
                for (int x = 0; x < width; x++)
                {
                    if (board[y, x] > 10)
                        cnt++;
                }

                blockHeightValue += (height - y) * cnt;
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