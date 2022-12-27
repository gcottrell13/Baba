using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    [DebuggerDisplay("RectangleSprite")]
    public class RectangleSprite : Sprite
    {
        public static Texture2D? WhiteRectangle;
        public static Rectangle rect = new(0, 0, 1, 1);

        public RectangleSprite() : base(new("white rectangle"))
        {
            graphicsResource.SetValue(WhiteRectangle);
        }

        public override void Draw()
        {
            draw(rect);
        }
    }
}
