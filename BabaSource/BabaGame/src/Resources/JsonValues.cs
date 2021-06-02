using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src.Resources
{
    using ColorStructure = Dictionary<string, JsonValues.ColorItem>;
    using AnimationStructure = Dictionary<string, Dictionary<string, int[][][]>>;
    using TilesetMapStructure = Dictionary<string, string>;
    using PaletteStructure = Dictionary<string, Color[][]>;

    public static class JsonValues
    {
        public static ColorStructure Colors = Load.loadJson<List<ColorItem>>("COLORS").ToDictionary(item => item.name);
        public static AnimationStructure Animations = Load.loadJson<AnimationStructure>("OUTPUT");
        public static TilesetMapStructure Tileset = Load.loadJson<TilesetMapStructure>("TILESET");
        public static PaletteStructure Palettes = Load.loadJson<Dictionary<string, int[][][]>>("PALETTES")
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(i => i.Select(c => new Color(c[0], c[1], c[2])).ToArray()).ToArray());


        public static Color TryGetPaletteColor(string theme, int[] coord)
        {
            return Palettes.TryGetValue(theme, out var arr) ? arr[coord[1]][coord[0]] : Color.White;
        }


        public struct ColorItem
        {
            public string name;
            public int[] colour;
        }
    }
}
