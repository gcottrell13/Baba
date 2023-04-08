using System;
using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Utils
{
    public sealed class AnimatedWobblerSprite : Sprite
    {
        private int currentWobbleFrame = 0;

        private double? _lastWobble = null;

        public Wobbler CurrentWobbler { get; private set; }

        public AnimatedWobblerSprite(SpriteValues? sprite, Direction initialState) : base(new ResourceHandle<Texture2D>(sprite?.Name))
        {
            if (sprite == null) throw new ArgumentNullException(nameof(sprite));

            CurrentWobbler = sprite.GetInitial(initialState) ?? throw new NullReferenceException("sprite returned a null initial state");
            _setWobbler(CurrentWobbler);
        }

        private void _setWobbler(Wobbler wobbler)
        {
            CurrentWobbler = wobbler;
            graphicsResource = wobbler.Texture;
        }

        public void Update(GameTime gameTime)
        {
            if (_lastWobble == null)
                _lastWobble = gameTime.TotalGameTime.TotalMilliseconds;
            else if (gameTime.TotalGameTime.TotalMilliseconds - _lastWobble > 300)
            {
                currentWobbleFrame++;
                _lastWobble = gameTime.TotalGameTime.TotalMilliseconds;
            }
            
        }

        public override void Draw()
        {
            draw(CurrentWobbler.GetPosition(ref currentWobbleFrame));
        }
    }
}
