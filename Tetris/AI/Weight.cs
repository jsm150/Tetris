namespace Tetris
{
    public class Weight
    {
        public Weight(float blockHeightValue = -3.4f,
            float lineClearValue = 9,
            float holeValue = -20,
            float blockedValue = -3,
            float sideValue = 3.7f,
            float blockValue = 3.7f,
            float floorValue = 4)
        {
            BlockHeightValue = blockHeightValue;
            LineClearValue = lineClearValue;
            HoleValue = holeValue;
            BlockedValue = blockedValue;
            SideValue = sideValue;
            BlockValue = blockValue;
            FloorValue = floorValue;
        }

        public float BlockHeightValue { get; }
        public float LineClearValue { get; }
        public float HoleValue { get; }
        public float BlockedValue { get; }
        public float SideValue { get; }
        public float BlockValue { get; }
        public float FloorValue { get; }
    }
}