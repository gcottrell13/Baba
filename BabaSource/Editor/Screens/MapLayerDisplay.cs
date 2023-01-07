using Core.Content;
using Core.Utils;
using Core;
using Editor.SaveFormats;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UI;

namespace Editor.Screens
{
    internal class MapLayerDisplay : GameObject
    {
        private readonly MapLayer mapLayer;
        public string theme;
        private Text objectsDisplay = new();
        private RectangleSprite background = new() { xscale = ScreenWidth, yscale = ScreenHeight };

        public MapLayerDisplay(string name, MapLayer mapLayer, string? theme, bool showName = true)
        {
            this.mapLayer = mapLayer;
            this.theme = theme ?? "default";
            Name = name;

            Graphics.AddChild(background);
            background.SetColor(ThemeInfo.GetThemeBackgroundColor(theme ?? "default"));
            background.xscale = (mapLayer.width * 24);
            background.yscale = (mapLayer.height * 24);

            if (showName)
            {
                AddChild(new Text(name));
                background.y = 25;
                objectsDisplay.Graphics.y = 25;
            }

            AddChild(objectsDisplay);

        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var objects = new Dictionary<uint, Dictionary<uint, string>>();

            foreach (var obj in mapLayer.objects)
            {
                var c = PaletteInfo.Palettes[theme][obj.color];
                objects.ConstructDefaultValue(obj.x)[obj.y] = $"{c.ToHexTriple()}[{obj.name}:{obj.state}]";
            }

            var lines = new List<string>();

            for (uint y = 0; y < mapLayer.height; y++)
            {
                var line = new List<string>();

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
                }
                lines.Add(string.Join("", line));
            }

            objectsDisplay.SetText(string.Join("\n", lines));

            if (ThemeInfo.GetThemeBackgroundColor(theme) is Color bgColor)
            {
                background.SetColor(bgColor);
                background.xscale = (mapLayer.width * 24);
                background.yscale = (mapLayer.height * 24);
            }
            else
            {
                background.SetColor(null);
            }
        }
    }
}
