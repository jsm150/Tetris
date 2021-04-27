﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class TetrisAI
    {
        private TetrisBlock _block;
        private IKeyboardSetting _keyboardSetting;
        private Weight _weight;

        private Func<int, int, int[,], int> DownLocationCalc;

        private TetrisAI(Tetris tetris, Weight weight)
        {
            tetris.ReSetBlockEvent += FindOptimalPosAsync;
            tetris.ConnectingToAi += ConnectingToTetris;
            tetris.GameEndEvent += DisconnectToTetris;
            _weight = weight;
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

        public static void AutoPlay(Tetris tetris, Weight weight = null)
        {
            if (weight == null)
                weight = new Weight();
            TetrisAi.Add(tetris.PlayerId, new TetrisAI(tetris, weight));
        }

        private static void DisconnectToTetris(object sender, EventArgs e)
        {
            var tetris = sender as Tetris;
            TetrisAi.Remove(tetris.PlayerId);
        }

        private void ConnectingToTetris(object sender, TetrisEventArgs e)
        {
            DownLocationCalc = e.DownLocationCalc;
            _block = e.TetrisBlock;
            _keyboardSetting = e.KeyboardSetting;
        }

        private async Task AutoPlaying(Tetris tetris, TetrisEventArgs e, int optimalX, int optimalRotation)
        {
            int delay = GetDelay(e.TetrisBoard);
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

            value += blockHeightValue * _weight.BlockHeightValue;
            value += lineClearValue * _weight.LineClearValue;
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
            {
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
            }

            value += holeValue * _weight.HoleValue;
            value += blockedValue * _weight.BlockedValue;
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

            value += sideValue * _weight.SideValue;
            value += blockValue * _weight.BlockValue;
            value += floorValue * _weight.FloorValue;
            return value;
        }
    }
}