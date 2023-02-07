using Core.Content;
using System.Diagnostics.CodeAnalysis;

namespace Core.Engine;

public enum ObjectKind
{
    Object,
    Text,
}

public class ObjectData : INameable
{
    public bool Deleted;
    public short Color;
    public int Facing;
    public int x;
    public int y;
    public int index;
    public ObjectKind Kind;
    
    public ObjectTypeId Name { get; set; }

    public int X => x;

    public int Y => y;

    // if the object's map of origin
    public short MapOfOrigin;
    public short OriginX;
    public short OriginY;

    // if the object is not present, act like it doesn't exist
    public bool Present;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ObjectData w)
        {
            return w.Deleted == Deleted && w.Color == Color && w.Facing == Facing && w.x == x && w.y == y && w.Name == Name && w.Kind == Kind;
        }
        return base.Equals(obj);
    }

    public override string ToString() => $$"""
        new ObjectData() {
            {{nameof(Deleted)}} = {{Deleted.ToString().ToLower()}}, {{nameof(Color)}} = {{Color}}, {{nameof(Facing)}} = {{Facing}}, {{nameof(x)}} = {{x}}, {{nameof(y)}} = {{y}}, {{nameof(Name)}} = "{{Name}}", {{nameof(Kind)}} = ObjectKind.{{Kind}},
        }
        """;

    public override int GetHashCode() => ToString().GetHashCode();
}
