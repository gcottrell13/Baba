using Core.Utils;
using g3;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content
{
    public static class ThemeInfo
    {
        private const int shift = PaletteInfo.shift;

        public static readonly Dictionary<string, int> ColorNameMap = new() {
            { "red", (2 << shift) + 1 },
            { "blue", (3 << shift) + 3 },
            { "yellow", (2 << shift) + 4 },
            { "orange", (2 << shift) + 2 },
            { "green", (5 << shift) + 2 },
            { "cyan", (1 << shift) + 4 },
            { "lime", (5 << shift) + 3 },
            { "purple", (3 << shift) + 0 },
            { "pink", (4 << shift) + 1 },
            { "rosy", (4 << shift) + 2 },
            { "grey", (0 << shift) + 1 },
            { "black", (0 << shift) + 0 },
            { "silver", (0 << shift) + 2 },
            { "white", (0 << shift) + 3 },
            { "brown", (6 << shift) + 1 }
        };

        public static List<string> ThemeNames()
        {
            return PaletteInfo.Palettes.Keys.ToList();
        }

        public static Color GetColorByName(string theme, string name)
        {
            return PaletteInfo.Palettes[theme][ColorNameMap[name]];
        }

        public static Color GetColor(string theme, short colorId)
        {
            return PaletteInfo.Palettes[theme][colorId];
        }

        public static Color GetObjectColor(string theme, string name)
        {
            return PaletteInfo.Palettes[theme][ObjectInfo.Info[name].color_active];
        }

        public static Color GetThemeBackgroundColor(string theme)
        {
            return PaletteInfo.Palettes[theme][(6 << shift) + 4];
        }

        public static string MakeObjectString(string theme, string name, int color)
        {
            return $"{PaletteInfo.Palettes[theme][color].ToHexTriple()}[{name}]";
        }
    }
}
