using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Resources
{
    using ColorStructure = List<JsonValues.ColorItem>;
    using AnimationStructure = Dictionary<string, Dictionary<string, int[][][]>>;
    using TilesetMapStructure = Dictionary<string, string>;

    public static class JsonValues
    {
        public static ColorStructure Colors = loadJson<ColorStructure>("COLORS");
        public static AnimationStructure Animations = loadJson<AnimationStructure>("OUTPUT");
        public static TilesetMapStructure Tileset = loadJson<TilesetMapStructure>("TILESET");

        private static T loadJson<T>(string name)
        {
            var file = System.IO.File.Open($"Content/json/{name}.json", System.IO.FileMode.Open);
            var buffer = new byte[file.Length];
            file.Read(buffer);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(buffer));
        }

        public struct ColorItem
        {
            public string name;
            public int[] colour;
        }
    }
}
