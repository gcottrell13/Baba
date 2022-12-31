using Core.Utils;
using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Editor.SaveFormats
{
    internal interface Serializable
    {
        string Serialize(int indent);
    }

    internal class SaveFormat : Serializable
    {
        public List<MapInstance> WorldLayout { get; set; } = new();

        public List<Warp> Warps { get; set; } = new();

        public List<Region> Regions { get; set; } = new();

        public List<MapData> MapDatas { get; set; } = new();

        public List<ObjectData> globalObjectLayer { get; set; } = new();

        public int startMapX = 0;
        public int startMapY = 0;

        public string worldName = string.Empty;

        public string? fileName = null;

        public string Serialize(int indent) => Serialize();

        public string Serialize()
        {
            IEnumerable<string> lines()
            {
                yield return $"\"WorldLayout\": {slist(WorldLayout, 1)}";
                yield return $"\"Warps\": {slist(Warps, 1)}";
                yield return $"\"Regions\": {slist(Regions, 1)}";
                yield return $"\"MapDatas\": {slist(MapDatas, 1)}";
                yield return $"\"globalObjectLayer\": {slist(globalObjectLayer, 1)}";
                yield return $"\"startMapX\": {startMapX}";
                yield return $"\"startMapY\": {startMapY}";
                yield return $"\"worldName\": \"{worldName}\"";
            }
            return "{\n" + string.Join(",\n", lines().Select(x => "\t" + x)) + "\n}";
        }
        public static string slist<T>(List<T> alist, int indent) where T : Serializable
            => "[" + string.Join(",\n", alist.Select(x => x.Serialize(indent + 1))) + "]";
    }

    internal class MapInstance : Serializable
    {
        public int x = 0;
        public int y = 0;
        public string mapDataName = string.Empty;

        public string Serialize(int indent)
        {
            return "\t".Repeat(indent) + Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    internal class Warp : Serializable
    {
        public int x1 = 0;
        public int y1 = 0;
        public int x2 = 0;
        public int y2 = 0;
        public string Serialize(int indent)
        {
            return "\t".Repeat(indent) + Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    internal class Region : Serializable
    {
        public string name = string.Empty;
        public string theme = string.Empty;
        public List<ObjectData> regionObjectLayer { get; set; } = new();

        public string Serialize(int indent)
        {
            var t = "\t".Repeat(indent);
            IEnumerable<string> lines()
            {
                yield return $"\"name\": \"{name}\"";
                yield return $"\"theme\": \"{theme}\"";
                yield return $"\"regionObjectLayer\": {SaveFormat.slist(regionObjectLayer, indent)}";
            }
            return t + "{" + string.Join(",\n", lines().Select(x => t + x)) + t + "}";
        }
    }

    internal class MapData : Serializable
    {
        public string name = string.Empty;
        public string regionName = string.Empty;
        public List<ObjectData> layer1 { get; set; } = new();
        public List<ObjectData> layer2 { get; set; } = new();

        public string Serialize(int indent)
        {
            var t = "\t".Repeat(indent);
            IEnumerable<string> lines()
            {
                yield return $"\"name\": \"{name}\"";
                yield return $"\"regionName\": \"{regionName}\"";
                yield return $"\"layer1\": {SaveFormat.slist(layer1, indent)}";
                yield return $"\"layer2\": {SaveFormat.slist(layer2, indent)}";
            }
            return t + "{" + string.Join(",\n", lines().Select(x => t + x)) + t + "}";
        }
    }

    internal class ObjectData : Serializable
    {
        public int x = 0;
        public int y = 0;
        public string name = string.Empty;
        public int state = 0;
        public string color = string.Empty;
        public string text = string.Empty;

        public string Serialize(int indent)
        {
            return "\t".Repeat(indent) + Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
