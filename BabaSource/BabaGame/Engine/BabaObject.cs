using BabaGame.Objects;
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

public record struct BabaPastState(int x, int y, Direction facing, short color, ObjectKind kind, ObjectTypeId name);

public class BabaObject : INameable
{

    public bool Deleted;
    public short Color;
    public Direction Facing;
    public int x;
    public int y;
    public int index;
    public ObjectKind Kind;

    public ObjectStatesToDisplay state;

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

    public BabaPastState CurrentState => new (x, y, Facing, Color, Kind, Name);


    public static implicit operator BabaObject(ObjectData obj)
    {
        return new BabaObject()
        {
            Name = obj.Name,
            Color = obj.Color,
            Facing = obj.Facing,
            x = obj.X,
            y = obj.Y,
            Kind = obj.Kind,
            MapOfOrigin = obj.MapOfOrigin,
            OriginX = obj.OriginX,
            OriginY = obj.OriginY,
            Present = obj.Present,
            CurrentMapId = obj.CurrentMapId,
            Text = obj.Text,

            Active = false,
            index = -1,
        };
    }

    public ObjectData ToObjectData()
    {
        return new ObjectData()
        {
            Name = Name,
            Color = Color,
            Facing = Facing,
            MapOfOrigin = MapOfOrigin,
            OriginX = OriginX,
            OriginY = OriginY,
            Present = Present,
            CurrentMapId = CurrentMapId,
            Text = Text,
            Kind = Kind,
            x = (byte)X,
            y = (byte)Y,
        };
    }

    public void RestoreState(BabaPastState state)
    {
        Name = state.name;
        Color = state.color;
        Facing = state.facing;
        x = state.x;
        y = state.y;
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
