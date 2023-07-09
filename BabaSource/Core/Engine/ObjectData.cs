using Core.Content;
using Core.Utils;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Engine;

public enum ObjectKind
{
    Object,
    Text,
}

public class ObjectData
{
    public short Color;
    public Direction Facing;
    public ObjectKind Kind;
    public long PositionId;
    
    public ObjectTypeId Name { get; set; }

    // if the object's map of origin
    public short MapOfOrigin;

    // if the object is deleted or not present, act like it doesn't exist
    public bool Present = true;
    public bool Deleted = false;

    public string? Text;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ObjectData w)
        {
            return w.Deleted == Deleted && w.Color == Color && w.Facing == Facing && w.PositionId == PositionId && w.Name == Name && w.Kind == Kind;
        }
        return base.Equals(obj);
    }

    private string textToString => string.IsNullOrEmpty(Text) ? "\"\"" : "\"\"\"\n\t" + Text.Indent(1) + "\n\t\"\"\"";

    public override string ToString() => $$"""
        new ObjectData() {
            {{nameof(Deleted)}} = {{Deleted.ToString().ToLower()}}, {{nameof(Color)}} = {{Color}}, {{nameof(Facing)}} = {{nameof(Direction)}}.{{Facing}}, 
            {{nameof(PositionId)}} = {{PositionId}}, {{nameof(Name)}} = {{nameof(ObjectTypeId)}}.@{{Enum.GetName(Name)}}, {{nameof(Kind)}} = {{nameof(ObjectKind)}}.{{Kind}},
            {{nameof(Text)}} = {{textToString}},
        }
        """;

    public override int GetHashCode() => ToString().GetHashCode();
}
