using Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves
{
    public static class SaveFormatSerializer
    {
        private static string serialize<T>(T obj, int indent) where T : notnull => obj switch
        {
            SaveFormatWorld s => Serialize(s, indent + 1),
            SaveMapInstance s => Serialize(s, indent + 1),
            SaveWarp s => Serialize(s, indent + 1),
            SaveMapData s => Serialize(s, indent + 1),
            SaveMapLayer s => Serialize(s, indent + 1),
            SaveRegion s => Serialize(s, indent + 1),
            SaveObjectData s => Serialize(s, indent + 1),
            string s => s,
            _ => JsonConvert.SerializeObject(obj),
        };

        public static string Serialize(SaveFormatWorld save, int indent) => Serialize(save);

        public static string Serialize(SaveFormatWorld save)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"{nameof(save.WorldLayout)}\": {slist(save.WorldLayout, 0)}";
                yield return $"\"{nameof(save.Warps)}\": {slist(save.Warps, 0)}";
                yield return $"\"{nameof(save.Regions)}\": {slist(save.Regions, 0)}";
                yield return $"\"{nameof(save.MapDatas)}\": {slist(save.MapDatas, 0)}";
                yield return $"\"{nameof(save.globalObjectInstanceIds)}\": {serialize(save.globalObjectInstanceIds, 0)}";
                yield return $"\"{nameof(save.worldName)}\": \"{save.worldName}\"";
            }
            return formatLines(lines(), 0, "{", "}");
        }

        public static string Serialize(SaveMapInstance mapInstance, int indent)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(mapInstance);
        }

        public static string Serialize(SaveWarp warp, int indent)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(warp);
        }

        public static string Serialize(SaveRegion region, int indent)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"{nameof(region.id)}\": {region.id}";
                yield return $"\"{nameof(region.name)}\": \"{region.name}\"";
                yield return $"\"{nameof(region.theme)}\": \"{region.theme}\"";
                yield return $"\"{nameof(region.musicName)}\": \"{region.musicName}\"";
                yield return $"\"{nameof(region.regionObjectInstanceIds)}\": {serialize(region.regionObjectInstanceIds, indent)}";
            }
            return formatLines(lines(), indent, "{", "}");
        }

        public static string Serialize(SaveMapLayer map, int indent)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"{nameof(map.objects)}\": {slist(map.objects, indent)}";
                yield return $"\"{nameof(map.width)}\": {map.width}";
                yield return $"\"{nameof(map.height)}\": {map.height}";
            }
            return formatLines(lines(), indent, "{", "}");
        }

        public static string Serialize(SaveMapData map, int indent)
        {
            IEnumerable<string> lines()
            {
                yield return $"\"{nameof(map.id)}\": {map.id}";
                yield return $"\"{nameof(map.name)}\": \"{map.name}\"";
                yield return $"\"{nameof(map.regionId)}\": {map.regionId}";
                yield return $"\"{nameof(map.resetWhenInactive)}\": {map.resetWhenInactive.ToString().ToLower()}";
                yield return $"\"{nameof(map.layer1)}\": {serialize(map.layer1, indent)}";
                yield return $"\"{nameof(map.layer2)}\": {serialize(map.layer2, indent)}";
            }
            return formatLines(lines(), indent, "{", "}");
        }

        public static string Serialize(SaveObjectData obj, int indent)
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
}
