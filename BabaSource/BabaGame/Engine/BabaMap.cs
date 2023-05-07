using Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Engine;

public class BabaMap
{
    public List<BabaObject> WorldObjects { get; private set; } = new();

    public short MapId;
    public short northNeighbor;
    public short eastNeighbor;
    public short southNeighbor;
    public short westNeighbor;

    public short upLayer;
    public short region;

    public short width;
    public short height;

    public string Name = string.Empty;


    public BabaMap()
    {
    }

    public void AddObject(BabaObject obj)
    {
        if (obj.index != -1)
            throw new InvalidOperationException();
        obj.index = WorldObjects.Count;
        WorldObjects.Add(obj);
    }

    public void RemoveObject(BabaObject obj)
    {
        var last = WorldObjects.Last();
        WorldObjects.RemoveAt(last.index);

        obj.Deleted = true;

        if (last.index == obj.index) return;

        WorldObjects[obj.index] = last;
        last.index = obj.index;

        obj.index = -1;
    }

    public static implicit operator BabaMap(MapData map)
    {
        var babaMap = new BabaMap()
        {
            MapId = map.MapId,
            northNeighbor = map.northNeighbor,
            eastNeighbor = map.eastNeighbor,
            southNeighbor = map.southNeighbor,
            westNeighbor = map.westNeighbor,
            region = map.region,
            width = map.width,
            height = map.height,
            Name = map.Name,
        };

        foreach (var obj in map.WorldObjects)
        {
            babaMap.AddObject(obj);
        }
        return babaMap;
    }

    public MapData ToMapData()
    {
        return new MapData(WorldObjects.Select(x => x.ToObjectData())) 
        {
            MapId = MapId,
            northNeighbor = northNeighbor,
            eastNeighbor = eastNeighbor,
            southNeighbor = southNeighbor,
            westNeighbor = westNeighbor,
            region = region,
            width = width,
            height = height,
            Name = Name,
        };
    }
}
