using Core.Screens;
using Editor.Saves;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Editor.Screens
{
    internal class MapPickerScreen : FiltererModal<SaveScreenData>
    {
        public MapPickerScreen(List<SaveScreenData> items) : base(items, 15, x => x.name, x => $"map: {x.name}")
        {
            Name = "MapPickerScreen";
            Transparent = true;
            SetOffsetX(100);
            SetDisplayTypeName("map");
            listDisplay.SetHighlightColor(Color.Blue);
        }
    }
}
