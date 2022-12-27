using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public enum Direction
    {
        None = 0,
        Right = 0b1,
        Up = 0b10,
        Left = 0b100,
        Down = 0b1000,
    }

    public static class DirectionExtensions
    {
        public static Direction FromString(string? direction) => direction?.ToLower() switch
        {
            "up" => Direction.Up,
            "down" => Direction.Down,
            "left" => Direction.Left,
            "right" => Direction.Right,
            _ => Direction.None,
        };

        public static Direction? Opposite(Direction? direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => direction,
        };

        public static Direction Opposite(Direction direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => direction,
        };

        public static (int x, int y) DeltaFromDirection(Direction direction) => direction switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => (0, 0),
        };

        public static Direction DirectionFromDelta((int x, int y) delta) => delta switch
        {
            (0, -1) => Direction.Up,
            (0, 1) => Direction.Down,
            (-1, 0) => Direction.Left,
            (1, 0) => Direction.Right,
            _ => Direction.None,
        };
    }
}
