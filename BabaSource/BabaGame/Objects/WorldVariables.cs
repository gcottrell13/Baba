﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.Objects
{
    public static class WorldVariables
    {
        public static float CameraX = 0f;
        public static float CameraY = 0f;
        public static int Padding = 0;
        public static int Margin = 1;
        public static float Scale = BaseScale;
        public static int TileWidth = 24;
        public static int TileHeight = 24;

        public const float BaseScale = 2f;

        public static float InputDelaySeconds = 0.02f;
        public static float MoveAnimationSeconds = 0.2f;

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
