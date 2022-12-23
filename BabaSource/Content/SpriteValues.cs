using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Content
{
    public abstract class SpriteValues
    {
        public SpriteValues(string name)
        {
            var parts = name.Split('.', 1);
            Name = parts[0];
            RestName = parts[1];
        }

        public string Name { get; }
        public string RestName { get; }
    }

    public class Wobbler : SpriteValues
    {
        public Wobbler(string name, Vector2[] positions, Texture2D tex) : base(name)
        {
            Positions = positions;
            Tex = tex;
        }

        public Vector2[] Positions { get; }
        public Texture2D Tex { get; }

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

        public Wobbler Move(int step)
        {
            step = (step + 1) % Frames.Length;
            return Frames[step];
        }
    }

    public class FacingOnMove : SpriteValues
    {
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
            Sleep_Up = sleep_up;
            Sleep_Left = sleep_left;
            Sleep_Down = sleep_down;
            Sleep_Right = sleep_right;
        }

        public AnimateOnMove Up { get; }
        public AnimateOnMove Down { get; }
        public AnimateOnMove Left { get; }
        public AnimateOnMove Right { get; }
        public AnimateOnMove? Sleep_Up { get; }
        public AnimateOnMove? Sleep_Left { get; }
        public AnimateOnMove? Sleep_Down { get; }
        public AnimateOnMove? Sleep_Right { get; }

        public Wobbler Move(Direction direction, int step)
        {
            var dir = direction switch
            {
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.Up => Up,
                Direction.Down => Down,
                _ => throw new NotImplementedException()
            };
            return dir.Move(step);
        }

        public Wobbler Sleep(Direction direction)
        {
            var dir = direction switch
            {
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.Up => Up,
                Direction.Down => Down,
                _ => throw new NotImplementedException()
            };
            return dir.Move(0);
        }
    }
}
