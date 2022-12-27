using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;

namespace Content.UI
{
    public class Text : GameObject
    {
        private SpriteContainer?[]? _chars;
        private Dictionary<char, AnimatedWobblerSprite>? _currentLetters;
        private Dictionary<char, AnimatedWobblerSprite> _cache = new();

        public Text(string text = "", Color? color = null)
        {
            SetText(text, color);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (_currentLetters == null) return;

            foreach (var s in _currentLetters.Values)
            {
                s?.Update(gameTime);
            }
        }

        private bool _tryGetLetter(char letter, out AnimatedWobblerSprite sprite)
        {
            sprite = null;
            if (ContentLoader.LoadedContent == null) return false;

            if (!_cache.ContainsKey(letter))
            {
                if (ContentLoader.LoadedContent.SpriteValues.TryGetValue($"text_{letter}", out var textSprite))
                {
                    _cache[letter] = new AnimatedWobblerSprite(textSprite, 0);
                }
                else
                {
                    return false;
                }
            }

            sprite = _cache[letter];
            return true;
        }

        public void SetText(string text, Color? color = null, int padding = 0)
        {
            Graphics.RemoveAllChildren();

            _currentLetters = new();
            var x = 0;
            _chars = text.Select(c =>
            {
                if (_tryGetLetter(c, out var letter))
                {
                    _currentLetters[c] = letter;

                    var sprite = new SpriteContainer()
                    {
                        x = x,
                    };
                    sprite.AddChild(letter);
                    Graphics.AddChild(sprite);
                    x += letter.CurrentWobbler.Size.X + padding;
                    return sprite;
                }
                else
                {
                    x += 24;
                    return null;
                }
            }).ToArray();
        }
    }
}
