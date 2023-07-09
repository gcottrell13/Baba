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


    public BabaMap(ScreenData map)
    {
        MapId = map.ScreenId;
        northNeighbor = map.northNeighborId;
        eastNeighbor = map.eastNeighborId;
        southNeighbor = map.southNeighborId;
        westNeighbor = map.westNeighborId;
        region = map.RegionId;
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
        obj.CurrentMapId = MapId;
        obj.Deleted = false;
        obj.Present = true;
        WorldObjects.Add(obj);
    }

    public void RemoveObject(BabaObject obj)
    {
        if (WorldObjects.RemoveAll(x => x.index == obj.index) > 0)
        {
            obj.Deleted = true;
            obj.CurrentMapId = -1;
        }
    }

    public static implicit operator BabaMap(ScreenData map) => new BabaMap(map);

    public void ResetToOriginalState()
    {
        if (originalState == null) return;
        WorldObjects.Clear();
        foreach (var original in originalState)
        {
            AddObject(original);
        }
    }

    public ScreenData ToMapData()
    {
        return new ScreenData(WorldObjects.Select(x => x.ToObjectData())) 
        {
            ScreenId = MapId,
            northNeighborId = northNeighbor,
            eastNeighborId = eastNeighbor,
            southNeighborId = southNeighbor,
            westNeighborId = westNeighbor,
            RegionId = region,
            width = width,
            height = height,
            Name = Name,
            upLayer = upLayer,
            visited = Visited,
        };
    }
}
