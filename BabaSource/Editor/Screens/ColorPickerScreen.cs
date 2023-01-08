using Core.Content;
using Core.Screens;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class ColorPickerScreen : FiltererModal<COLOR>
    {
        public static readonly List<COLOR> COLORS = new List<string>() {
            "red",
            "blue",
            "orange",
            "yellow",
            "lime",
            "green",
            "cyan",
            "purple",
            "pink",
            "rosy",
            "grey",
            "black",
            "silver",
            "white",
            "brown"
        }.Select(k => new COLOR() { name = k, value = ThemeInfo.ColorNameMap[k] }).ToList();

        public ColorPickerScreen(int currentColor = 0) : base(
            items: COLORS, 
            maxDisplay: COLORS.Count, 
            filterBy: x => x.name, 
            display: x => $"{PaletteInfo.Palettes["default"][x.value].ToHexTriple()}{x.name}", 
            currentValue: COLORS.FirstOrDefault(x => x.value == currentColor))
        {
            Name = "ColorPickerScreen";
            SetDisplayTypeName("color");
        }
    }

    public class COLOR
    {
        public string name = string.Empty;
        public int value;
    }
}
