using Core.Content;
using Core.Engine;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BabaGame.Engine
{
    public enum WorldObjectKind
    {
        Text,
        Object,
    }

    public enum WorldObjectColor
    {
        Red = 0x01,
        Blue = 0x02,
        Orange = 0x03,
        Yellow = 0x04,
        Lime = 0x05,
        Green = 0x06,
        Cyan = 0x07,
        Purple = 0x08,
        Pink = 0x09,
        Rosy = 0x0a,
        Grey = 0x0b,
        Black = 0x0c,
        Silver = 0x0d,
        White = 0x0e,
        Brown = 0x0f,

        /// <summary>
        /// combined with the other colors, usually will be a darker tone
        /// </summary>
        Faded = 0x40,
    }

    public struct WorldObject : INameable
    {
        public bool Occupied; // does this instance represent an object yet?
        public WorldObjectColor Color;
        public WorldObjectKind Kind;
        public int ObjectId;
        public Direction Facing;
        public uint x;
        public uint y;
        public uint index;

        public string Name { get; set; }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is WorldObject w)
            {
                return w.Occupied == Occupied && w.Color == Color && w.Kind == Kind && w.ObjectId == ObjectId && w.Facing == Facing && w.x == x && w.y == y && w.Name == Name;
            }
            return base.Equals(obj);
        }

        public override string ToString() => $"{Occupied} {Color} {Kind} {ObjectId} {Facing} {x} {y} {Name}";
    }
}
