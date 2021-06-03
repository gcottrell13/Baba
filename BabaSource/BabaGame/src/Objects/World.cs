﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Objects
{
    public static class World
    {
        public static float CameraX = 0f;
        public static float CameraY = 0f;
        public static int Padding = 0;
        public static int Margin = 1;
        public static float Scale = 2f;
        public static float TileWidth = 24f;
        public static float TileHeight = 24f;

        public static float InputDelaySeconds = 0.1f;

        public static string Palette = "default";

        public static (float x, float y) GameCoordToScreenCoord(int x, int y)
        {
            return (
                Margin + (TileWidth + Padding) * x - CameraX,
                Margin + (TileHeight + Padding) * y - CameraY
            );
        }

        public static (int x, int y) GetSizeInPixels(int x, int y)
        {
            var sx = Margin * 2 + TileWidth * x + Padding * (x - 1);
            var sy = Margin * 2 + TileHeight * y + Padding * (y - 1);
            return ((int)(sx * Scale), (int)(sy * Scale));
        }
    }
}
