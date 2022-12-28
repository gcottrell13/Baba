﻿using Core.Screens;
using Core.UI;
using Core.Utils;
using Editor.SaveFormats;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapPickerScreen : FiltererModal<MapData>
    {
        public MapPickerScreen(List<MapData> items) : base(items, 15, x => x.name, x => $"map: {x.name}")
        {
            Name = "MapPickerScreen";
            Edit = true;
            Transparent = true;
            SetOffsetX(100);
            HighlightColor = Color.Blue;
        }
    }
}