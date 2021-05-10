using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace Tetris
{
    public class TetrisBlock
    {
        private static readonly Random _random = new Random();
        private readonly int[] _blockNumArr = Enumerable.Range(0, 8).ToArray();
        private readonly Queue<int> _saveBlock = new Queue<int>();
        private int _blockNumPoint = 8;

        public int[,] Block { get; private set; }
        public ReadOnlyCollection<int> BlockRotationCount { get; }
        public ReadOnlyCollection<Brush> BlockColor { get; }
        public int BlockNum { get; private set; }
        public int RotationNum { get; private set; }

        public TetrisBlock()
        {
            BlockRotationCount = Array.AsReadOnly(SetBlockRotationCountInit());
            BlockColor = Array.AsReadOnly(SetBlockColorInit());
        }

        private static int[] SetBlockRotationCountInit()
        {
            var arr = new int[8];
            arr[1] = 2;
            arr[2] = 1;
            arr[3] = 2;
            arr[4] = 4;
            arr[5] = 4;
            arr[6] = 4;
            arr[7] = 2;
            return arr;
        }

        private static Brush[] SetBlockColorInit()
        {
            var arr = new Brush[9];
            arr[1] = Brushes.Aqua;
            arr[2] = Brushes.Yellow;
            arr[3] = Brushes.Red;
            arr[4] = Brushes.Orange;
            arr[5] = Brushes.PaleVioletRed;
            arr[6] = Brushes.CornflowerBlue;
            arr[7] = Brushes.GreenYellow;
            arr[8] = Brushes.Gray;
            return arr;
        }

        public int NextBlockNum()
        {
            return _saveBlock.Peek();
        }

        public void SetRotationBlock()
        {
            Block = BlockCreate(BlockNum, ++RotationNum);
        }

        public void SetRotationBlock(int rotationNum)
        {
            RotationNum = rotationNum;
            Block = BlockCreate(BlockNum, RotationNum);
        }

        public void NewBlock()
        {
            int num = _random.Next(1, _blockNumPoint);
            _saveBlock.Enqueue(_blockNumArr[num]);

            (_blockNumArr[num], _blockNumArr[_blockNumPoint - 1]) =
                (_blockNumArr[_blockNumPoint - 1], _blockNumArr[num]);
            _blockNumPoint--;
            if (_blockNumPoint <= 1)
                _blockNumPoint = 8;

            if (_saveBlock.Count <= 1)
            {
                NewBlock();
                return;
            }

            BlockNum = _saveBlock.Dequeue();
            RotationNum = 0;
            Block = BlockCreate(BlockNum, RotationNum);
        }

        public int[,] BlockCreate(int blockNum, int rotationNum)
        {
            switch (blockNum)
            {
                case 1:
                    // ####
                    return rotationNum % 2 == 0
                        ? new[,] {{0, 0, 0, 0}, {1, 1, 1, 1}, {0, 0, 0, 0}, {0, 0, 0, 0}}
                        : new[,] {{0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}};
                // ##
                // ##
                case 2:
                    return new[,] {{1, 1}, {1, 1}};
                // ##
                //  ##
                case 3:
                    return rotationNum % 2 == 0
                        ? new[,] {{1, 1, 0}, {0, 1, 1}, {0, 0, 0}}
                        : new[,] {{0, 0, 1}, {0, 1, 1}, {0, 1, 0}};
                case 4:
                    switch (rotationNum % 4)
                    {
                        // #
                        // ###
                        case 0:
                            return new[,] {{1, 0, 0}, {1, 1, 1}, {0, 0, 0}};
                        case 1:
                            return new[,] {{0, 1, 1}, {0, 1, 0}, {0, 1, 0}};
                        case 2:
                            return new[,] {{0, 0, 0}, {1, 1, 1}, {0, 0, 1}};
                        case 3:
                            return new[,] {{0, 1, 0}, {0, 1, 0}, {1, 1, 0}};
                    }

                    break;
                case 5:
                    //  #
                    // ###
                    if (rotationNum % 4 == 0)
                        return new[,] {{0, 1, 0}, {1, 1, 1}, {0, 0, 0}};
                    else if (rotationNum % 4 == 1)
                        return new[,] {{0, 1, 0}, {0, 1, 1}, {0, 1, 0}};
                    else if (rotationNum % 4 == 2)
                        return new[,] {{0, 0, 0}, {1, 1, 1}, {0, 1, 0}};
                    else if (rotationNum % 4 == 3)
                        return new[,] {{0, 1, 0}, {1, 1, 0}, {0, 1, 0}};
                    break;
                case 6:
                    //   #
                    // ###
                    if (rotationNum % 4 == 0)
                        return new[,] {{0, 0, 1}, {1, 1, 1}, {0, 0, 0}};
                    else if (rotationNum % 4 == 1)
                        return new[,] {{0, 1, 0}, {0, 1, 0}, {0, 1, 1}};
                    else if (rotationNum % 4 == 2)
                        return new[,] {{0, 0, 0}, {1, 1, 1}, {1, 0, 0}};
                    else if (rotationNum % 4 == 3)
                        return new[,] {{1, 1, 0}, {0, 1, 0}, {0, 1, 0}};
                    break;
                case 7:
                    //  ##
                    // ##
                    if (rotationNum % 2 == 0)
                        return new[,] {{0, 1, 1}, {1, 1, 0}, {0, 0, 0}};
                    else
                        return new[,] {{0, 1, 0}, {0, 1, 1}, {0, 0, 1}};
            }

            return null;
        }
    }
}