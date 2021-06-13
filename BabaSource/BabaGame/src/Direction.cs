using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src
{
    public enum Direction
    {
        None = 0,
        Up,
        Left,
        Down,
        Right,
    }

    public static class DirectionExtensions
    {
        public static Direction FromString(string? direction) => direction switch
        {
            "up" => Direction.Up,
            "down" => Direction.Down,
            "left" => Direction.Left,
            "right" => Direction.Right,
            _ => Direction.None,
        };
    }
}
