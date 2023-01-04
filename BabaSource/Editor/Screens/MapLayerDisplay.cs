using Core;
using Core.Content;
using Core.UI;
using Core.Utils;
using Editor.SaveFormats;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapLayerDisplay : GameObject
    {
        private readonly MapLayer mapLayer;
        private readonly ObjectData? cursor;
        private Text objectsDisplay = new();
        private Text gridDisplay = new();
        private Text cursorDisplay = new();

        public MapLayerDisplay(string name, MapLayer mapLayer, ObjectData? cursor)
        {
            this.mapLayer = mapLayer;
            this.cursor = cursor;
            Name = name;
            AddChild(new Text(name));

            gridDisplay.Graphics.y = 25;
            AddChild(gridDisplay);
            AddChild(objectsDisplay);
            AddChild(cursorDisplay);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var objects = new Dictionary<uint, Dictionary<uint, string>>();

            foreach (var obj in mapLayer.objects)
            {
                var c = PaletteInfo.GetColorByName("default", obj.color);
                objects.ConstructDefaultValue(obj.x)[obj.y] = $"{c.ToHexTriple()}[{obj.name}:{obj.state}]";
            }

            var maxYMagnitude = (int)Math.Log10(mapLayer.height + 1) + 1;
            var maxXMagnitude = (int)Math.Log(mapLayer.width, 26) + 1;

            var lines = new List<string>();
            var gridLines = " ".Repeat(mapLayer.width)
                .Select((c, i) => EnumerableExtensions.ToColString((uint)i).PadLeft(maxXMagnitude) + " ")
                .ZipMany().Select(line => " ".Repeat(maxYMagnitude + 1) + line).ToList();

            for (uint y = 0; y < mapLayer.height; y++)
            {
                var line = new List<string>();
                var gridLine = new List<string>() { (y + 1).ToString().PadLeft(maxYMagnitude) + " " };

                for (uint x = 0; x < mapLayer.width; x++)
                {
                    if (objects.ConstructDefaultValue(x).TryGetValue(y, out var obj))
                    {
                        line.Add(obj);
                    }
                    else
                    {
                        line.Add(" ");
                    }
                    gridLine.Add("[editorsquare]");
                }
                gridLines.Add(string.Join("", gridLine));
                lines.Add(string.Join("", line));
            }

            gridDisplay.SetText(string.Join("\n", gridLines));
            objectsDisplay.SetText(string.Join("\n", lines));

            objectsDisplay.Graphics.x = (objectsDisplay.CurrentOptions!.padding + objectsDisplay.CurrentOptions.blockWidth) * (maxYMagnitude + 1);
            objectsDisplay.Graphics.y = (objectsDisplay.CurrentOptions!.padding + objectsDisplay.CurrentOptions.lineHeight) * (maxXMagnitude + 1) + gridDisplay.Graphics.y;

            if (cursor != null && cursor.x < mapLayer.width && cursor.y < mapLayer.height)
            {
                cursorDisplay.SetText("\n".Repeat(cursor.y) + " ".Repeat(cursor.x) + "[pink][cursor]");
                cursorDisplay.Graphics.x = objectsDisplay.Graphics.x;
                cursorDisplay.Graphics.y = objectsDisplay.Graphics.y;
            }
            
            var yscale = 26f / (mapLayer.height + 3);
            var xscale = 36f / (mapLayer.width + 3);
            var scale = Math.Min(xscale, yscale);
            Graphics.xscale = scale;
            Graphics.yscale = scale;
        }
    }
}
