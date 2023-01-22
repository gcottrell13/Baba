using System.Diagnostics.CodeAnalysis;

namespace Core.Engine
{
    public struct ObjectData : INameable
    {
        public bool Occupied; // does this instance represent an object yet?
        public short Color;
        public int ObjectId;
        public int Facing;
        public int x;
        public int y;
        public int index;

        public string Name { get; set; }

        public int X => x;

        public int Y => y;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is ObjectData w)
            {
                return w.Occupied == Occupied && w.Color == Color && w.ObjectId == ObjectId && w.Facing == Facing && w.x == x && w.y == y && w.Name == Name;
            }
            return base.Equals(obj);
        }

        public override string ToString() => $$"""
            new ObjectData() {
                Occupied = {{Occupied.ToString().ToLower()}},
                Color = {{Color}},
                ObjectId = {{ObjectId}},
                Facing = {{Facing}},
                x = {{x}},
                y = {{y}},
                Name = "{{Name}}",
            }
            """;
    }
}
