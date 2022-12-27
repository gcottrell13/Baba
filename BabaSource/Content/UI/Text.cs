using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Content.UI
{
    public class Text : GameObject
    {
        private AnimatedWobblerSprite[]? _chars;

        public Text(string text = "")
        {
            SetText(text);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (_chars == null) return;

            foreach (var s in _chars)
            {
                s?.Update(gameTime);
            }
        }

        public void SetText(string text)
        {
            Graphics.RemoveAllChildren();

            if (ContentLoader.LoadedContent == null) return;

            _chars = new AnimatedWobblerSprite[text.Length];
            var x = 0;
            foreach (var (c, i) in text.ToLower().Select((v, i) => (v, i)))
            {
                if (ContentLoader.LoadedContent.SpriteValues.TryGetValue($"text_{c}", out var textSprite))
                {
                    var sprite = new AnimatedWobblerSprite(textSprite, 0)
                    {
                        x = x
                    };

                    x += textSprite?.GetInitial(0).Size.X ?? 0;

                    _chars[i] = sprite;
                    Graphics.AddChild(sprite);
                }
                else
                {
                    x += 24;
                }
            }
        }
    }
}
