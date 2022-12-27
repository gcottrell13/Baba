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

        public WorldEditorScreen()
        {
            AddChild(new Text("[blue]World editor"));

            SetCommands(new()
            {
                { "[text_escape]", "go back" },
                { "q", "zoom out" },
                { "e", "zoom in" },
                { "[arrow:1][arrow:2][arrow:4][arrow:8]", "move cursor" },
                { "m", "Map Picker" },
            });

            var rect = new RectangleSprite()
            {
                x = -0.5f, y = -0.5f,
            };
            rect.SetColor(Color.Red);
            r.AddChild(rect);
            Graphics.AddChild(r);

        }

        protected override void OnUpdate(GameTime gameTime)
        {
            r.rotation += 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
