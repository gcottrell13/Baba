using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Objects
{
    public static class World
    {
        public static float CameraX = 0f;
        public static float CameraY = 0f;
        public static float Padding = 2f;
        public static float Margin = 2f;
        public static float Scale = 2f;
        public static float TileWidth = 24f;
        public static float TileHeight = 24f;

        public static (float x, float y) GameCoordToScreenCoord(int x, int y)
        {
            return (
                (Margin + (TileWidth + Padding)) * x - CameraX,
                (Margin + (TileHeight + Padding)) * y - CameraY
            );
        }
    }
}
