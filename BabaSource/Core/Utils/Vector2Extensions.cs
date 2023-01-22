using System;

namespace Core.Utils
{
    public static class Vector2Extensions
    {
        public static Microsoft.Xna.Framework.Vector2 Clamped(this Microsoft.Xna.Framework.Vector2 v, float value)
        {
            return new Microsoft.Xna.Framework.Vector2(Math.Clamp(v.X, -value, value), Math.Clamp(v.Y, -value, value));
        }

        public static string ToRowColString(this Microsoft.Xna.Framework.Vector2 dims)
        {
            return $"{EnumerableExtensions.ToColString((int)dims.X)}{dims.Y + 1}";
        }
    }
}
