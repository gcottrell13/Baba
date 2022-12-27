using System;
using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Utils
{
    public sealed class AnimatedWobblerSprite : Sprite
    {
        private readonly SpriteValues sprite;

        private int currentWobbleFrame = 0;
        private int currentAnimateFrame = 0;

        private double? _lastWobble = null;

        public Wobbler CurrentWobbler { get; private set; }

        public AnimatedWobblerSprite(SpriteValues? sprite, int initialState) : base(new ResourceHandle<Texture2D>(sprite?.Name))
        {
            if (sprite == null) throw new ArgumentNullException(nameof(sprite));

            this.sprite = sprite;
            CurrentWobbler = sprite.GetInitial(initialState) ?? throw new NullReferenceException("sprite returned a null initial state");
            _setWobbler(CurrentWobbler);
        }

        private void _setWobbler(Wobbler wobbler)
        {
            CurrentWobbler = wobbler;
            graphicsResource.SetValue(wobbler.Texture);
        }

        public void Move(Direction d, bool isAsleep=false)
        {
            if (sprite is AnimateOnMove a)
            {
                _setWobbler(a.Move(ref currentAnimateFrame));
            }
            else if (sprite is FacingOnMove f)
            {
                _setWobbler(isAsleep ? f.Sleep(d, ref currentAnimateFrame) : f.Move(d, ref currentAnimateFrame));
            }
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
