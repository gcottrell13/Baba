using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.SaveFormats
{
    internal class SaveFormat
    {
        public List<MapInstance> WorldLayout { get; set; } = new();

        public List<Warp> Warps { get; set; } = new();

        public List<Region> Regions { get; set; } = new();

        public List<MapData> MapDatas { get; set; } = new();

        public List<ObjectData> globalObjectLayer { get; set; } = new();

        public int startMapX = 0;
        public int startMapY = 0;

        public string worldName = string.Empty;
    }

    internal class MapInstance
    {
        public int x = 0;
        public int y = 0;
        public string mapDataName = string.Empty;
    }

    internal class Warp
    {
        public int x1 = 0;
        public int y1 = 0;
        public int x2 = 0;
        public int y2 = 0;
    }

    internal class Region
    {
        public string name = string.Empty;
        public string theme = string.Empty;
        public List<ObjectData> regionObjectLayer { get; set; } = new();
    }

    internal class MapData
    {
        public string name = string.Empty;
        public string regionName = string.Empty;
        public List<ObjectData> layer1 { get; set; } = new();
        public List<ObjectData> layer2 { get; set; } = new();
    }

    internal class ObjectData
    {
        public int x = 0;
        public int y = 0;
        public string name = string.Empty;
        public int state = 0;
        public string color = string.Empty;
        public string text = string.Empty;
    }
}
