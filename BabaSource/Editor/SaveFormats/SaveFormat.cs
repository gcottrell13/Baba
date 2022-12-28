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

        public int startMapX;
        public int startMapY;

        public string worldName;
    }

    internal class MapInstance
    {
        public int x;
        public int y;
        public string mapDataName;
    }

    internal class Warp
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;
    }

    internal class Region
    {
        public string name;
        public string theme;
        public List<ObjectData> regionObjectLayer { get; set; } = new();
    }

    internal class MapData
    {
        public string name;
        public string regionName;
        public List<ObjectData> layer1 { get; set; } = new();
        public List<ObjectData> layer2 { get; set; } = new();
    }

    internal class ObjectData
    {
        public int x;
        public int y;
        public string name;
        public int state;
        public string color;
        public string text;
    }
}
