using Core.Content;
using Core.Utils;
using Core;
using Microsoft.Xna.Framework;
using Editor.Saves;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UI;

namespace Editor.Screens
{
    internal class MapLayerDisplay : GameObject
    {
        public string title;
        public readonly SaveMapLayer mapLayer;
        public string theme;
        private Text objectsDisplay = new();
        private RectangleSprite background = new() { xscale = ScreenWidth, yscale = ScreenHeight };

        public bool showObjects = true;

        public MapLayerDisplay(string title, SaveMapLayer mapLayer, string? theme, bool showName = true)
        {
            this.title = title;
            this.mapLayer = mapLayer;
            this.theme = theme ?? "default";

            Graphics.AddChild(background);
            background.SetColor(ThemeInfo.GetThemeBackgroundColor(theme ?? "default"));
            background.xscale = (mapLayer.width * 24);
            background.yscale = (mapLayer.height * 24);

            if (showName)
            {
                AddChild(new Text(title) { Name="title" });
                background.y = 25;
                objectsDisplay.Graphics.y = 25;
            }

            AddChild(objectsDisplay);

        }

        protected override void OnUpdate(GameTime gameTime)
        {
            (ChildByName("title") as Text)?.SetText(title);

            if (showObjects)
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
            }
            else
            {
                objectsDisplay.SetText("");
            }

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
