using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Core.Utils
{
    public static class Vector2Extensions
    {
        public static Vector2 Clamped(this Vector2 v, float value)
        {
            return new Vector2(Math.Clamp(v.X, -value, value), Math.Clamp(v.Y, -value, value));
        }
    }
}
