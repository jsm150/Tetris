using System.Collections.Generic;

namespace Tetris
{
    public class GeneticAlgorithm
    {

    }

    public class Weight
    {
        public int BlockHeightValue { get; set; }
        public int HoleValue { get; set; }
        public int BlockingValue { get; set; }
        public int LineClearValue { get; set; }
        public int BlockTouchValue { get; set; }
        public int WallTouchValue { get; set; }
    }
}