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

    public bool ResetOnUnload;


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
        ResetOnUnload = map.ResetOnUnload;
        foreach (var obj in map.WorldObjects)
        {
            AddObject(obj);
        }

        if (ResetOnUnload)
        {
            originalState = new();
            foreach (var obj in map.WorldObjects)
            {
                originalState.Add(obj);
            }
        }
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

    public static implicit operator BabaMap(MapData map) => new BabaMap(map);

    public void ResetToOriginalState()
    {
        if (!ResetOnUnload || originalState == null) return;
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
            ResetOnUnload = ResetOnUnload,
            upLayer = upLayer,
        };
    }
}
