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
        private static List<TetrisAI> TetrisAi { get; } = new List<TetrisAI>();

        private Func<int, int, int[,], int> DownLocationCalc;
        private readonly Action<KeyEventArgs> KeyboardAction;
        private IKeyboardSetting _keyboardSetting;
        private TetrisBlock _block;

        public TetrisAI(Tetris tetris)
        {
            tetris.AutoPlayer += FindOptimalPosAsync;
            tetris.ConnectingToAi += ConnectingToTetris;
            KeyboardAction = tetris.KeyBoardAction;
        }

        public static void AutoPlay(Tetris tetris)
        {
            TetrisAi.Add(new TetrisAI(tetris));
        }

        private async Task AutoPlaying(TetrisEventArgs e, int optimalX, int optimalRotation)
        {
            Keys keyData = e.CurrentX < optimalX? _keyboardSetting.RightCode : _keyboardSetting.LeftCode;
            int end = Math.Abs(e.CurrentX - optimalX);
            await Task.Delay(200);
            for (int i = 0; i < optimalRotation; i++)
            {
                await Task.Delay(100);
                KeyboardAction(new KeyEventArgs(_keyboardSetting.RotationCode));
            }

            for (int i = 0; i < end; i++)
            {
                await Task.Delay(100);
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
                if (x + currentX >= 0 && x + currentX < 10) continue;
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
                    int y = DownLocationCalc(0, x, block);
                    double temp = GetBestCaseValue(y, x, block, e.TetrisBoard);

                    if (max >= temp) continue;
                    max = temp;
                    pos = (x, i);
                }
            }

            await AutoPlaying(e, pos.X, pos.RotationNum);
        }

        private static double GetBestCaseValue(int currentY, int currentX, int[,] block, int[,] board)
        {
            double value = 0;
            value += GetValue(currentY, currentX, block, board);
            //value += GetValue2(currentY, currentX, block, board);
            value += GetValue3(currentY, currentX, block, board);
            value += GetValue4(currentY, currentX, block, board);
            value += currentY / 2.0;

            return value;
        }

        // 빈구멍 찾기
        private static int GetValue(int currentY, int currentX, int[,] block, int[,] board)
        {
            var value = 0;
            for (var y = 0; y < block.GetLength(0) + 1; y++)
            for (var x = 0; x < block.GetLength(0); x++)
            {
                if (y + currentY >= 20 || y + currentY < 0 || currentX + x >= 10 || currentX + x < 0) continue;
                if ((y < block.GetLength(0) && block[y, x] != 0) && board[currentY + y, currentX + x] > 10) continue;
                if (currentY + y - 1 < 0) continue;

                for (int i = y - 1; i >= 0; i--)
                {
                    if (block[i, x] != 1) continue;
                    value -= 100 * (y - i);
                    break;
                }
                
            }
            return value;
        }

        // 블럭이 얼마나 잘 붙어있는지
        private static double GetValue2(int currentY, int currentX, int[,] block, int[,] board)
        {
            var value = 0.0;
            int[] dy = {-1, 0, 1, 0};
            int[] dx = {0, 1, 0, -1};
            for (var y = 0; y < block.GetLength(0); y++)
            for (var x = 0; x < block.GetLength(0); x++)
            {
                var cnt = 0;
                if (block[y, x] == 0) continue;
                for (var i = 0; i < dy.Length; i++)
                {
                    if (currentY + y + dy[i] < 0 || currentX + x + dx[i] < 0 || currentX + x + dx[i] >= 10) continue;
                    if (currentY + y + dy[i] >= 20 || board[currentY + y + dy[i], currentX + x + dx[i]] > 10)
                        cnt++;
                }

                value += 20 * cnt;
            }

            return value;
        }

        // 블럭이 쌓여진 정도
        private static double GetValue3(int currentY, int currentX, int[,] block, int[,] board)
        {
            var value = 0;
            for (int y = 0; y < block.GetLength(0); y++)
            {
                if (LineIsAllZero(y)) continue;

                var cnt = 0;
                for (int x = 0; x < block.GetLength(0); x++)
                {
                    if (currentY + y >= 20 || currentY + y < 0) continue;
                    if (block[y, x] == 1 || currentX + x >= 10 || board[currentY + y, currentX + x] > 10)
                        cnt++;
                }

                value = cnt * y;
            }

            return value;

            bool LineIsAllZero(int y)
            {
                for (int x = 0; x < block.GetLength(0); x++)
                    if (block[y, x] == 1)
                        return false;
                return true;
            }
        }

        // 라인을 지우는 갯수
        private static int GetValue4(int currentY, int currentX, int[,] block, int[,] board)
        {
            var value = 0;

            for (int y = currentY; y < board.GetLength(0); y++)
            {
                if (y < 0) continue;
                if (LineCheck(y))
                    value++;
            }

            bool LineCheck(int y)
            {
                var size = block.GetLength(0);
                for (var x = 0; x < board.GetLength(1); x++)
                    if (board[y, x] <= 10)
                        if (y - currentY >= size || x - currentX < 0 || x - currentX >= size 
                            || block[y - currentY, x - currentX] == 0)
                            return false;
                return true;
            }

            return value * 5000;
        }


    }
}