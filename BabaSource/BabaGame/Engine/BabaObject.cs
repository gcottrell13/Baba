using Core.Content;
using Core.Engine;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;

public class BabaObject : INameable
{

    public bool Deleted;
    public short Color;
    public Direction Facing;
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
    public bool Present = true;

    public short CurrentMapId;

    public string? Text;

    public bool Active;

    public static implicit operator BabaObject(ObjectData obj)
    {
        return new BabaObject()
        {
            Name = obj.Name,
            Color = obj.Color,
            Facing = obj.Facing,
            Active = false,
            x = obj.X,
            y = obj.Y,
            Kind = obj.Kind,
            MapOfOrigin = obj.MapOfOrigin,
            OriginX = obj.OriginX,
            OriginY = obj.OriginY,
            Present = obj.Present,
            CurrentMapId = obj.CurrentMapId,
            Text = obj.Text,
            index = -1,
        };
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is BabaObject w)
        {
            return w.Deleted == Deleted && w.Color == Color && w.Facing == Facing && w.x == x && w.y == y && w.Name == Name && w.Kind == Kind;
        }
        return base.Equals(obj);
    }

    private string textToString => string.IsNullOrEmpty(Text) ? "\"\"" : "\"\"\"\n\t" + Text.Indent(1) + "\n\t\"\"\"";

    public override string ToString() => $$"""
        new ObjectData() {
            {{nameof(Deleted)}} = {{Deleted.ToString().ToLower()}}, {{nameof(Color)}} = {{Color}}, {{nameof(Facing)}} = {{nameof(Direction)}}.{{Facing}}, 
            {{nameof(x)}} = {{x}}, {{nameof(y)}} = {{y}}, {{nameof(Name)}} = {{nameof(ObjectTypeId)}}.@{{Enum.GetName(Name)}}, {{nameof(Kind)}} = {{nameof(ObjectKind)}}.{{Kind}},
            {{nameof(Text)}} = {{textToString}},
        }
        """;

    public override int GetHashCode() => ToString().GetHashCode();
}
