using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public class Sprite : BaseSprite
    {
        protected ResourceHandle<Texture2D> graphicsResource;

        public Sprite(ResourceHandle<Texture2D> graphicsResource)
        {
            this.graphicsResource = graphicsResource;
        }

        protected void draw(Rectangle? sourceRectangle = null)
        {
            RecalculateTransform();
            using var t = new SceneContextManager(this);
            var totalTransform = Scene.GetCurrentTransformation();
            if (totalTransform.Transform.Decompose(out var scale, out var rot, out var pos))
            {
                //var pos = Vector4.Transform(trans, Matrix.CreateRotationZ((float)totalRotation));
                Scene.spriteBatch.Draw(graphicsResource.Value,
                    position: new Vector2(pos.X, pos.Y),
                    sourceRectangle: sourceRectangle,
                    color: new Color(color, totalTransform.Alpha),
                    origin: new Vector2(xanchor, yanchor),
                    rotation: (float)totalTransform.Rotation,
                    scale: new Vector2(scale.X, scale.Y),
                    effects: SpriteEffects.None,
                    layerDepth: 0
                    );
            }
        }

        public override void Draw()
        {
            draw();
        }
    }
}
