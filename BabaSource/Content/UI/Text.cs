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

        public Text(string text = "", Color? color = null, int padding = 0, int lineHeight = 24)
        {
            SetText(text, color, padding, lineHeight);
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
                var name = letter switch
                {
                    '?' => "what",
                    '/' => "text_fwslash",
                    '-' => "text_hyphen",
                    _ => $"text_{letter}",
                };

                if (ContentLoader.LoadedContent.SpriteValues.TryGetValue(name, out var textSprite))
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

        public void SetText(string text, Color? color = null, int padding = 0, int lineHeight = 24)
        {
            Graphics.RemoveAllChildren();

            _currentLetters = new();
            var x = 0;
            var y = 0;
            _chars = text.Select(c =>
            {
                if (c == '\n')
                {
                    y += lineHeight;
                    x = 0;
                    return null;
                }
                else if (_tryGetLetter(c, out var letter))
                {
                    _currentLetters[c] = letter;
                    letter.SetColor(color);

                    var sprite = new SpriteContainer()
                    {
                        x = x,
                        y = y - (letter.CurrentWobbler.Size.Y - lineHeight) / 2,
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
