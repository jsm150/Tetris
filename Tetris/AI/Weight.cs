namespace Tetris
{
    public class Weight
    {
        public float BlockHeightValue { get; set; } = -3.4f;
        public float LineClearValue { get; set; } = 9;
        public float HoleValue { get; set; } = -20;
        public float BlockedValue { get; set; } = -3;
        public float SideValue { get; set; } = 3.7f;
        public float BlockValue { get; set; } = 3.7f;
        public float FloorValue { get; set; } = 4;
    }
}