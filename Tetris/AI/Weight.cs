using System;

namespace Tetris
{
    public class Weight
    {
        // AI 학습을 안했을때 사용하기 위한 기본값.
        public float BlockHeightValue { get; set; } = -3.4f;
        public float BlockedValue { get; set; } = -3;
        public float HoleValue { get; set; } = -20;
        public float LineClearValue { get; set; } = 9;
        public float SideValue { get; set; } = 3.7f;
        public float BlockValue { get; set; } = 3.7f;
        public float FloorValue { get; set; } = 4;

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return BlockHeightValue;
                    case 1:
                        return BlockedValue;
                    case 2:
                        return HoleValue;
                    case 3:
                        return LineClearValue;
                    case 4:
                        return SideValue;
                    case 5:
                        return BlockValue;
                    case 6:
                        return FloorValue;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        BlockHeightValue = value;
                        break;
                    case 1:
                        BlockedValue = value;
                        break;
                    case 2:
                        HoleValue = value;
                        break;
                    case 3:
                        LineClearValue = value;
                        break;
                    case 4:
                        SideValue = value;
                        break;
                    case 5:
                        BlockValue = value;
                        break;
                    case 6:
                        FloorValue = value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}