using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace Content.UI
{
    public class Text : GameObject
    {
        private Dictionary<string, AnimatedWobblerSprite> _currentLetters;
        private Dictionary<string, AnimatedWobblerSprite> _cache = new();

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

        private bool _tryGetLetter(string letter, out AnimatedWobblerSprite sprite)
        {
            sprite = null;
            if (ContentLoader.LoadedContent == null) return false;

            if (!_cache.ContainsKey(letter))
            {
                var name = letter switch
                {
                    "?" => "what",
                    "/" => "text_fwslash",
                    "-" => "text_hyphen",
                    _ => letter.Length == 1 ? $"text_{letter}" : letter,
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

        private Color? _getColor(string str)
        {
            str = str.ToLower();
            var namedColorProps = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
            var namedColorProp = namedColorProps.FirstOrDefault(x => x?.Name.ToLower() == str, null);
            if (namedColorProp?.GetValue(null) is Color c)
            {
                return c;
            }

            var rgb = str.Split(',').Select(c => float.TryParse(c.Trim(), out var r) ? r : -1).ToArray();
            if (rgb.Length == 3 && rgb.All(x => x > 0f)) 
            {
                return new Color(rgb[0] / 255f, rgb[1] / 255f, rgb[2] / 255f);
            }

            return null;
        }

        public void SetText(string text, Color? color = null, int padding = 0, int lineHeight = 24)
        {
            Graphics.RemoveAllChildren();
            Name = $"Text: {text}";
            Graphics.Name = Name;

            _currentLetters = new();

            string? parsingItem = null;

            var x = 0;
            var y = 0;
            AnimatedWobblerSprite letter;

            void addCharacter(string character)
            {
                _currentLetters[character] = letter;
                var sprite = new SpriteContainer()
                {
                    x = x,
                    y = y - (letter.CurrentWobbler.Size.Y - lineHeight) / 2,
                    Name = character + "-textcontainer",
                };
                sprite.SetColor(color);
                sprite.AddChild(letter);
                Graphics.AddChild(sprite);
                x += letter.CurrentWobbler.Size.X + padding;
            }

            foreach (var c in text) {
                var character = c.ToString().ToLower();

                if (parsingItem != null)
                {
                    if (c == ']')
                    {
                        if (_getColor(parsingItem) is Color _color)
                        {
                            color = _color;
                        }
                        else if (_tryGetLetter(parsingItem, out letter))
                        {
                            addCharacter(character);
                        }
                        parsingItem = null;
                        continue;
                    }
                    else
                    {
                        parsingItem += c;
                        continue;
                    }
                }

                if (c == '\n')
                {
                    y += lineHeight;
                    x = 0;
                    continue;
                }

                if (c == '[')
                {
                    parsingItem = "";
                    continue;
                }

                if (_tryGetLetter(character, out letter))
                {
                    addCharacter(character);
                    continue;
                }

                x += 24;
            }
        }
    }
}
