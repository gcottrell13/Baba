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

    private List<BabaObject>? originalState;

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

    private int counter = 0;

    // has the player seen this map yet?
    public bool Visited = false;


    public BabaMap(MapData map)
    {
        MapId = map.MapId;
        northNeighbor = map.northNeighbor;
        eastNeighbor = map.eastNeighbor;
        southNeighbor = map.southNeighbor;
        westNeighbor = map.westNeighbor;
        region = map.region;
        width = map.width;
        height = map.height;
        Name = map.Name;
        upLayer = map.upLayer;
        Visited = map.visited;
        foreach (var obj in map.WorldObjects)
        {
            AddObject(obj);
        }

        originalState = new();
        foreach (var obj in map.WorldObjects)
        {
            originalState.Add(obj);
        }
    }

    public void AddObject(BabaObject obj)
    {
        obj.index = counter++;
        WorldObjects.Add(obj);
    }

    public void RemoveObject(BabaObject obj)
    {
        WorldObjects.RemoveAll(x => x.index == obj.index);
        obj.Deleted = true;
    }

    public static implicit operator BabaMap(MapData map) => new BabaMap(map);

    public void ResetToOriginalState()
    {
        if (originalState == null) return;
        WorldObjects.Clear();
        foreach (var original in originalState)
        {
            AddObject(original);
        }
    }

    public MapData ToMapData()
    {
        return new MapData((originalState ?? WorldObjects).Select(x => x.ToObjectData())) 
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
            upLayer = upLayer,
            visited = Visited,
        };
    }
}
