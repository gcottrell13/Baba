using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace Content.UI
{
    public class Text : GameObject
    {
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

        private Color _getColor(string str)
        {
            str = str.ToLower();
            var namedColorProps = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
            var namedColorProp = namedColorProps.FirstOrDefault(x => x?.Name.ToLower() == str, null);
            if (namedColorProp?.GetValue(null) is Color c)
            {
                return c;
            }

            var rgb = str.Split(',').Select(c => float.TryParse(c, out var r) ? r : -1).ToArray();
            if (rgb.Length == 3 && rgb.All(x => x > 0f)) 
            {
                return new Color(rgb[0] / 255f, rgb[1] / 255f, rgb[2] / 255f);
            }

            throw new ArgumentException($"Invalid color string: \"{str}\". Must be a named color, or a triplet of numbers 0 to 255");
        }

        public void SetText(string text, Color? color = null, int padding = 0, int lineHeight = 24)
        {
            Graphics.RemoveAllChildren();
            Name = $"Text: {text}";
            Graphics.Name = Name;

            _currentLetters = new();

            string? parsingColor = null;
            

            var x = 0;
            var y = 0;
            foreach (var c in text) {
                if (parsingColor != null)
                {
                    if (c == ']')
                    {
                        color = _getColor(parsingColor);
                        parsingColor = null;
                        continue;
                    }

                    parsingColor += c;
                    continue;
                }

                if (c == '\n')
                {
                    y += lineHeight;
                    x = 0;
                    continue;
                }

                if (c == '[')
                {
                    parsingColor = "";
                    continue;
                }

                if (_tryGetLetter(c, out var letter))
                {
                    _currentLetters[c] = letter;
                    var sprite = new SpriteContainer()
                    {
                        x = x,
                        y = y - (letter.CurrentWobbler.Size.Y - lineHeight) / 2,
                        Name = c.ToString() + "-container",
                    };
                    sprite.SetColor(color);
                    sprite.AddChild(letter);
                    Graphics.AddChild(sprite);
                    x += letter.CurrentWobbler.Size.X + padding;
                    continue;
                }

                x += 24;
            }
        }
    }
}
