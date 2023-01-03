using Core.Utils;
using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
            return "{\n" + string.Join(",\n", lines().Select(x => "\t" + x)) + "\n}";
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
            var t = "\t".Repeat(indent + 1);
            IEnumerable<string> lines()
            {
                yield return $"\"name\": \"{region.name}\"";
                yield return $"\"theme\": \"{region.theme}\"";
                yield return $"\"regionObjectLayer\": {serialize(region.regionObjectLayer, indent)}";
            }
            return "{" + t[..^1] + "\n" + string.Join(",\n", lines().Select(x => t + x)) + "\n" + t[..^1] + "}";
        }

        public static string Serialize(MapLayer map, int indent)
        {
            var t = "\t".Repeat(indent + 1);
            IEnumerable<string> lines()
            {
                yield return $"\"objects\": {slist(map.objects, indent)}";
                yield return $"\"width\": {map.width}";
                yield return $"\"height\": {map.height}";
            }
            return "{" + t[..^1] + "\n" + string.Join(",\n", lines().Select(x => t + x)) + "\n" + t[..^1] + "}";
        }

        public static string Serialize(MapData map, int indent)
        {
            var t = "\t".Repeat(indent + 1);
            IEnumerable<string> lines()
            {
                yield return $"\"name\": \"{map.name}\"";
                yield return $"\"regionName\": \"{map.regionName}\"";
                yield return $"\"resetWhenInactive\": {map.resetWhenInactive.ToString().ToLower()}";
                yield return $"\"layer1\": {serialize(map.layer1, indent)}";
                yield return $"\"layer2\": {serialize(map.layer2, indent)}";
            }
            return "{" + t[..^1] + "\n" + string.Join(",\n", lines().Select(x => t + x)) + "\n" + t[..^1] + "}";
        }

        public static string Serialize(ObjectData obj, int indent)
        {
            return "\t".Repeat(indent) + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string slist<T>(List<T> alist, int indent)
            where T : notnull
        {
            if (alist.Count == 0) return "[]";
            var t = "\t".Repeat(indent + 1);
            return "[\n" + t + string.Join(",\n", alist.Select(x => serialize(x, indent + 1))) + "\n" + t + "]";
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
        public string mapDataName = string.Empty;

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
        public string name = string.Empty;
        public string theme = string.Empty;
        public MapLayer regionObjectLayer { get; set; } = new();
    }

    internal class MapData
    {
        public string name = string.Empty;
        public string regionName = string.Empty;
        public MapLayer layer1 { get; set; } = new();
        public MapLayer layer2 { get; set; } = new();

        public bool resetWhenInactive = false;

    }

    internal class MapLayer
    {
        public List<ObjectData> objects { get; set; } = new();
        public uint width = 18;
        public uint height = 18;
    }

    internal class ObjectData
    {
        public uint x = 0;
        public uint y = 0;
        public string name = string.Empty;
        public uint state = 0;
        public string color = string.Empty; // a color name like "blue"
        public string text = string.Empty;
        public string? original = null;
    }
}
