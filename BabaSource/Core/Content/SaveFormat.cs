using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content
{
    public class SaveFormat
    {
        public List<MapInstance> WorldLayout { get; set; } = new();

        public List<Warp> Warps { get; set; } = new();

        public List<Region> Regions { get; set; } = new();

        public List<MapData> MapDatas { get; set; } = new();

        public MapLayer globalObjectLayer { get; set; } = new();

        public uint startMapX = 0;
        public uint startMapY = 0;

        public uint width = 30;
        public uint height = 30;

        public string worldName = string.Empty;

        public string? fileName = null;

    }

    public class MapInstance
    {
        public uint x = 0;
        public uint y = 0;
        public int mapDataId = 0;

    }

    public class Warp
    {
        public uint x1 = 0;
        public uint y1 = 0;
        public uint x2 = 0;
        public uint y2 = 0;
        public string name = string.Empty;

        public int r;
        public int g;
        public int b;
    }

    public class Region
    {
        public int id = 0;
        public string name = string.Empty;
        public string theme = "default";
        public MapLayer regionObjectLayer { get; set; } = new();

        public string musicName = "default";
    }

    public class MapData
    {
        public int id = 0;
        public string name = string.Empty;
        public int regionId = 0;
        public MapLayer layer1 { get; set; } = new();
        public MapLayer layer2 { get; set; } = new();

        public bool resetWhenInactive = false;

    }

    public class MapLayer
    {
        public List<ObjectData> objects { get; set; } = new();
        public uint width = 15;
        public uint height = 15;
    }

    public class ObjectData
    {
        public uint x = 0;
        public uint y = 0;
        public string name = string.Empty;
        public uint state = 1;
        public int color;
        public string text = string.Empty;
        public OriginalObjectData? original = null;

        public ObjectData copy()
        {
            return new ObjectData { x = x, y = y, color = color, state = state, name = name, original = original, text = text };
        }
    }

    public class OriginalObjectData
    {
        public string name = string.Empty;
        public uint state = 0;
        public int color;
    }
}
