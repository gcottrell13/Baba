using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public class AnimatedPackedSprite : Sprite
    {
        private int[][] animation = Array.Empty<int[]>();
        private uint index;

        public AnimatedPackedSprite(ResourceHandle<Texture2D> graphicsResource) : base(graphicsResource)
        {
        }

        protected void SetAnimation(int[][] anim)
        {
            animation = anim;
            index = 0;
        }

        protected void Step()
        {
            index = (uint)((index + 1) % animation.Length);
        }

        public override void Draw()
        {
            int width = 24;
            int height = 24;
            int padding = 2;
            int margin = 2;
            int x = animation[index][0];
            int y = animation[index][1];

            var sourceRectangle = new Rectangle(margin + (width + padding) * x, margin + (height + padding) * y, width, height);

            draw(sourceRectangle);
        }
    }
}
