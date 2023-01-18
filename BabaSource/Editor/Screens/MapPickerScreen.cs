using Core.Screens;
using Core.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Editor.Screens
{
    internal class MapPickerScreen : FiltererModal<SaveMapData>
    {
        public override Color HighlightColor => Color.Blue;
        
        public MapPickerScreen(List<SaveMapData> items) : base(items, 15, x => x.name, x => $"map: {x.name}")
        {
            Name = "MapPickerScreen";
            Transparent = true;
            SetOffsetX(100);
            SetDisplayTypeName("map");
        }
    }
}
