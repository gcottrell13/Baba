using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Core.Utils
{
    public abstract class SpriteValues
    {
        public SpriteValues(string name)
        {
            var parts = name.Split('.', 1);
            Name = parts[0];
            RestName = string.Join(".", parts.Skip(1));
        }

        public string Name { get; }
        public string RestName { get; }

        public abstract Wobbler GetInitial(int d);
    }

    public class Wobbler : SpriteValues
    {
        public Wobbler(string name, Point[] positions, Point size, Texture2D tex) : base(name)
        {
            Rectangles = positions.Select(p => new Rectangle(p * size, size)).ToArray();
            Size = size;
            Texture = tex;
        }

        public Rectangle[] Rectangles { get; }
        public Point Size { get; }
        public Texture2D Texture { get; }

        public Rectangle GetPosition(ref int step)
        {
            step %= Rectangles.Length;
            return Rectangles[step];
        }

        public override Wobbler GetInitial(int d) => this;
    }

    public class Joinable : SpriteValues
    {
        public const int Up = 0b10;
        public const int Down = 0b1000;
        public const int Left = 0b100;
        public const int Right = 0b1;

        public Joinable(string name, Wobbler[] wobblers) : base(name)
        {
            Wobblers = wobblers;
        }

        public Wobbler[] Wobblers { get; }

        public override Wobbler GetInitial(int d) => Wobblers[d];

        public Wobbler Join(int direction)
        {
            return Wobblers[direction];
        }
    }

    public class AnimateOnMove : SpriteValues
    {
        public AnimateOnMove(string name, Wobbler[] frames) : base(name)
        {
            Frames = frames;
        }

        public Wobbler[] Frames { get; }

        public override Wobbler GetInitial(int d) => Frames.First();

        public Wobbler Move(ref int step)
        {
            step = (step + 1) % Frames.Length;
            return Frames[step];
        }
    }

    public class FacingOnMove : SpriteValues
    {
        public const int SLEEP = 16;

        public FacingOnMove(
            string name,
            AnimateOnMove up, 
            AnimateOnMove down, 
            AnimateOnMove left, 
            AnimateOnMove right,
            AnimateOnMove? sleep_up,
            AnimateOnMove? sleep_left,
            AnimateOnMove? sleep_down,
            AnimateOnMove? sleep_right
        ) : base(name)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            SleepUp = sleep_up;
            SleepLeft = sleep_left;
            SleepDown = sleep_down;
            SleepRight = sleep_right;
        }

        public AnimateOnMove Up { get; }
        public AnimateOnMove Down { get; }
        public AnimateOnMove Left { get; }
        public AnimateOnMove Right { get; }
        public AnimateOnMove? SleepUp { get; }
        public AnimateOnMove? SleepLeft { get; }
        public AnimateOnMove? SleepDown { get; }
        public AnimateOnMove? SleepRight { get; }

        public override Wobbler GetInitial(int d) => d switch
        {
            (int)Direction.Up => Up.GetInitial(d),
            (int)Direction.Down => Down.GetInitial(d),
            (int)Direction.Left => Left.GetInitial(d),
            (int)Direction.Right => Right.GetInitial(d),

            SLEEP + (int)Direction.Up => SleepUp?.GetInitial(d) ?? throw new NotImplementedException(),
            SLEEP + (int)Direction.Down => SleepDown?.GetInitial(d) ?? throw new NotImplementedException(),
            SLEEP + (int)Direction.Left => SleepLeft?.GetInitial(d) ?? throw new NotImplementedException(),
            SLEEP + (int)Direction.Right => SleepRight?.GetInitial(d) ?? throw new NotImplementedException(),

            _ => Right.GetInitial(d),
        };

        public Wobbler Move(Direction direction, ref int step)
        {
            var dir = direction switch
            {
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.Up => Up,
                Direction.Down => Down,
                _ => throw new NotImplementedException()
            };
            return dir.Move(ref step);
        }

        public Wobbler Sleep(Direction direction, ref int step)
        {
            var dir = direction switch
            {
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.Up => Up,
                Direction.Down => Down,
                _ => throw new NotImplementedException()
            };
            return dir.Move(ref step);
        }
    }
}
