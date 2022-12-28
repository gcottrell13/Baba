using Core.Screens;
using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapPickerScreen : FiltererModal<string>
    {
        public MapPickerScreen(List<string> items) : base(items, x => x, x => $"map: {x}")
        {
            Name = "MapPickerScreen";
            edit = true;
            maxDisplay = 8;
            Transparent = true;
            SetOffsetX(100);
            HighlightColor = Color.Blue;
        }
    }
}
