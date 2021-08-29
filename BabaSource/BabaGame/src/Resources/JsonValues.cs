using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src.Resources
{
    using ObjectInformationStructure = Dictionary<string, JsonValues.ObjectInfoItem>;
    using AnimationStructure = Dictionary<string, Dictionary<string, int[][][]>>;
    using TilesetMapStructure = Dictionary<int, string>;
    using PaletteStructure = Dictionary<string, Color[][]>;

    public static class JsonValues
    {
        public static ObjectInformationStructure ObjectInfo = Load.loadJson<ObjectInformationStructure>("OBJECTS");
        public static AnimationStructure Animations = Load.loadJson<AnimationStructure>("ANIMATIONS");
        public static TilesetMapStructure Tileset = Load.loadJson<Dictionary<string, string>>("TILESET").ToDictionary(kvp => int.Parse(kvp.Key), kvp => kvp.Value);
        public static PaletteStructure Palettes = Load.loadJson<Dictionary<string, int[][][]>>("PALETTES")
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(i => i.Select(c => new Color(c[0], c[1], c[2])).ToArray()).ToArray());
        public static Sentences GlobalSentences = Load.loadJson<Sentences>("globalSentences");


        public static Color TryGetPaletteColor(string theme, int[] coord)
        {
            return Palettes.TryGetValue(theme, out var arr) ? arr[coord[1]][coord[0]] : Color.White;
        }


        public struct ObjectInfoItem
        {
            public int[] color_inactive;
            public int[] color;
            public string sprite;
            public int layer;
            public string unittype;
        }

        public struct Sentences
        {
            public List<string> sentences;
        }
    }
}
