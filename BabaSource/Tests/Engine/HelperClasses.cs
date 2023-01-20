using Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine;


public class Item : INameable
{
    public string Name { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public static implicit operator Item(string s) { return new() { Name = s }; }
    public override string ToString() => Name;
    public override bool Equals(object? obj)
    {
        if (obj is string s) return s == Name;
        if (obj is Item i) return i.Name == Name;
        return base.Equals(obj);
    }

    public override int GetHashCode() => Name.GetHashCode();
}

public class Grid : List<Item>
{
    public Grid(List<List<Item?>> grid) : base(grid.SelectMany((row, x) => row.Select((item, y) => 
    {
        if (item == null) return null;
        item.X = x;
        item.Y = y;
        return item;
    })).Where(item => item != null)!)
    {
    }
}