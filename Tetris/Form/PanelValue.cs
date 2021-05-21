namespace Tetris
{
    internal struct PanelValueData
    {
        public const int TetrisPanelWidth = 300;
        public const int TetrisPanelHeight = 603;
        public const int NextBlockPanelWidth = 120;
        public const int NextBlockPanelHeight = 63;

        public struct Player1
        {
            public const int TetrisPanelPointX = 1 * 30;
            public const int TetrisPanelPointY = 8 * 30;
            public const int NextBlockPanelPointX = 4 * 30;
            public const int NextBlockPanelPointY = 5 * 30;
        }

        public struct Player2
        {
            public const int TetrisPanelPointX = 12 * 30;
            public const int TetrisPanelPointY = 8 * 30;
            public const int NextBlockPanelPointX = 15 * 30;
            public const int NextBlockPanelPointY = 5 * 30;
        }
    }

    internal readonly struct PanelValue
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int PointX;
        public readonly int PointY;

        public PanelValue(int width, int height, int pointX, int pointY)
        {
            Width = width;
            Height = height;
            PointX = pointX;
            PointY = pointY;
        }

        public static PanelValue GetTetrisPanelToPlayer1()
        {
            return new PanelValue(PanelValueData.TetrisPanelWidth, PanelValueData.TetrisPanelHeight,
                PanelValueData.Player1.TetrisPanelPointX, PanelValueData.Player1.TetrisPanelPointY);
        }

        public static PanelValue GetTetrisPanelToPlayer2()
        {
            return new PanelValue(PanelValueData.TetrisPanelWidth, PanelValueData.TetrisPanelHeight,
                PanelValueData.Player2.TetrisPanelPointX, PanelValueData.Player2.TetrisPanelPointY);
        }

        public static PanelValue GetNextBlockPanelToPlayer1()
        {
            return new PanelValue(PanelValueData.NextBlockPanelWidth, PanelValueData.NextBlockPanelHeight,
                PanelValueData.Player1.NextBlockPanelPointX, PanelValueData.Player1.NextBlockPanelPointY);
        }

        public static PanelValue GetNextBlockPanelToPlayer2()
        {
            return new PanelValue(PanelValueData.NextBlockPanelWidth, PanelValueData.NextBlockPanelHeight,
                PanelValueData.Player2.NextBlockPanelPointX, PanelValueData.Player2.NextBlockPanelPointY);
        }
    }
}