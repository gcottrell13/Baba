using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Engine
{
    public enum WorldObjectKind
    {
        Text,
        Object,
    }

    public enum WorldObjectColor
    {
        White = 0x00,
        Grey = 0x01,
        LightGrey = 0x02,

        Green = 0x03,
        LightGreen = 0x0c,

        Blue = 0x04,
        LightBlue = 0x09,

        Red = 0x06,
        Pink = 0x05,

        Yellow = 0x07,

        Brown = 0x08,
        Orange = 0x0a,

        Purple = 0x0b,

        /// <summary>
        /// combined with the other colors, usually will be a darker tone
        /// </summary>
        Faded = 0x10,
    }

    public struct WorldObject
    {
        public bool Occupied; // does this instance represent an object yet?
        public WorldObjectColor Color;
        public WorldObjectKind Kind;
        public int Name; // 1 for baba, 2 for wall, etc.
        public Direction Facing;
        public uint x;
        public uint y;
        public uint index;
    }
}
