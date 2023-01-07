using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Editor.SaveFormats
{
    internal static class SaveFormatSerializer
    {
        private static string serialize<T>(T obj, int indent) where T : notnull => obj switch
        {
            SaveFormat s => Serialize(s, indent + 1),
            MapInstance s => Serialize(s, indent + 1),
            Warp s => Serialize(s, indent + 1),
            MapData s => Serialize(s, indent + 1),
            MapLayer s => Serialize(s, indent + 1),
            Region s => Serialize(s, indent + 1),
            ObjectData s => Serialize(s, indent + 1),
            string s => s,
            _ => obj?.ToString() ?? "",
        };

        public static string Serialize(SaveFormat save, int indent) => Serialize(save);

        public static string Serialize(SaveFormat save)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"WorldLayout\": {slist(save.WorldLayout, 0)}";
                yield return $"\"Warps\": {slist(save.Warps, 0)}";
                yield return $"\"Regions\": {slist(save.Regions, 0)}";
                yield return $"\"MapDatas\": {slist(save.MapDatas, 0)}";
                yield return $"\"globalObjectLayer\": {serialize(save.globalObjectLayer, 0)}";
                yield return $"\"startMapX\": {save.startMapX}";
                yield return $"\"startMapY\": {save.startMapY}";
                yield return $"\"worldName\": \"{save.worldName}\"";
            }
            return formatLines(lines(), 0, "{", "}");
        }

        public static string Serialize(MapInstance mapInstance, int indent)
        {
            return "\t".Repeat(indent) + Newtonsoft.Json.JsonConvert.SerializeObject(mapInstance);
        }

        public static string Serialize(Warp warp, int indent)
        {
            return "\t".Repeat(indent) + Newtonsoft.Json.JsonConvert.SerializeObject(warp);
        }

        public static string Serialize(Region region, int indent)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"id\": {region.id}";
                yield return $"\"name\": \"{region.name}\"";
                yield return $"\"theme\": \"{region.theme}\"";
                yield return $"\"regionObjectLayer\": {serialize(region.regionObjectLayer, indent)}";
            }
            return formatLines(lines(), indent, "{", "}");
        }

        public static string Serialize(MapLayer map, int indent)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"objects\": {slist(map.objects, indent)}";
                yield return $"\"width\": {map.width}";
                yield return $"\"height\": {map.height}";
            }
            return formatLines(lines(), indent, "{", "}");
        }

        public static string Serialize(MapData map, int indent)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"id\": {map.id}";
                yield return $"\"name\": \"{map.name}\"";
                yield return $"\"regionId\": {map.regionId}";
                yield return $"\"resetWhenInactive\": {map.resetWhenInactive.ToString().ToLower()}";
                yield return $"\"layer1\": {serialize(map.layer1, indent)}";
                yield return $"\"layer2\": {serialize(map.layer2, indent)}";
            }
            return formatLines(lines(), indent, "{", "}");
        }

        public static string Serialize(ObjectData obj, int indent)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string slist<T>(List<T> alist, int indent)
            where T : notnull
        {
            if (alist.Count == 0) return "[]";
            var t = "\t".Repeat(indent + 1);
            return formatLines(alist.Select(x => serialize(x, indent + 1)), indent + 1, "[", "]");
        }

        private static string formatLines(IEnumerable<string> lines, int indent, string open, string close)
        {
            var t = "\t".Repeat(indent + 1);
            return open + "\n" + t + string.Join(",\n" + t, lines) + "\n" + t[..^1] + close;
        }
    }

    internal class SaveFormat
    {
        public List<MapInstance> WorldLayout { get; set; } = new();

        public List<Warp> Warps { get; set; } = new();

        public List<Region> Regions { get; set; } = new();

        public List<MapData> MapDatas { get; set; } = new();

        public MapLayer globalObjectLayer { get; set; } = new();

        public uint startMapX = 0;
        public uint startMapY = 0;

        public string worldName = string.Empty;

        public string? fileName = null;

    }

    internal class MapInstance
    {
        public uint x = 0;
        public uint y = 0;
        public int mapDataId = 0;

    }

    internal class Warp
    {
        public uint x1 = 0;
        public uint y1 = 0;
        public uint x2 = 0;
        public uint y2 = 0;
    }

    internal class Region
    {
        public int id = 0;
        public string name = string.Empty;
        public string theme = "default";
        public MapLayer regionObjectLayer { get; set; } = new();
    }

    internal class MapData
    {
        public int id = 0;
        public string name = string.Empty;
        public int regionId = 0;
        public MapLayer layer1 { get; set; } = new();
        public MapLayer layer2 { get; set; } = new();

        public bool resetWhenInactive = false;

    }

    internal class MapLayer
    {
        public List<ObjectData> objects { get; set; } = new();
        public uint width = 15;
        public uint height = 15;
    }

    internal class ObjectData
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

    internal class OriginalObjectData
    {
        public string name = string.Empty;
        public uint state = 0;
        public int color;
    }
}
