using Core.Content;
using Core.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class ThemePickerScreen : FiltererModal<string>
    {
        public override Color HighlightColor => Selected == null ? base.HighlightColor : ThemeInfo.GetThemeBackgroundColor(Selected);

        public ThemePickerScreen(string? currentTheme = null) : base(ThemeInfo.ThemeNames(), 15, x => x, currentValue: currentTheme)
        {
            Name = "ThemePickerScreen";
            SetDisplayTypeName("theme");
        }
    }
}
