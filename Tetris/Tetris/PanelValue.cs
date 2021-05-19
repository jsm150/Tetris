using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    struct PanelValueData
    {
        public const int Width = 300;
        public const int Height = 603;

        public struct Player1
        {
            public const int PointX = 1 * 30;
            public const int PointY = 8 * 30;
        }

        public struct Player2
        {
            public const int PointX = 12 * 30;
            public const int PointY = 8 * 30;
        }
    }

    readonly struct PanelValue
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

        public static PanelValue GetPlayer1()
        {
            return new PanelValue(PanelValueData.Width, PanelValueData.Height, PanelValueData.Player1.PointX,
                PanelValueData.Player1.PointY);
        }

        public static PanelValue GetPlayer2()
        {
            return new PanelValue(PanelValueData.Width, PanelValueData.Height, PanelValueData.Player2.PointX,
                PanelValueData.Player2.PointY);
        }
    }

}