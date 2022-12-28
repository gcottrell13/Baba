using Core.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
using Microsoft.Xna.Framework;

namespace Editor.Screens
{
    internal class WorldEditorScreen : BaseScreen
    {
        SpriteContainer r = new() { xscale = 100, yscale = 200, x =100, y = 100 };
        string pickedMap = string.Empty;
        private Text t;

        public WorldEditorScreen()
        {
            t = new Text("[blue]World editor");
            AddChild(t);

            SetCommands(new()
            {
                { CommonStrings.ESCAPE, "go back" },
                { CommonStrings.ALL_ARROW, "move cursor" },
                { "q", "zoom out" },
                { "e", "zoom in" },
                { "m", "Map Picker" },
                { "r", "regions" },
            });

            var rect = new RectangleSprite()
            {
                x = -0.5f, y = -0.5f,
            };
            rect.SetColor(Color.Red);
            r.AddChild(rect);
            Graphics.AddChild(r);

        }

        public void SetPickedMap(string? name)
        {
            t.SetText($"World editor, picked [green]{name}[white] to place");
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            r.rotation += 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
