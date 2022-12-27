using Core;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection;

namespace Content.UI
{
    public class Text : GameObject
    {
        private Dictionary<string, AnimatedWobblerSprite> _currentLetters;
        private Dictionary<string, AnimatedWobblerSprite> _cache = new();

        public Text(string text = "", int padding = 0, int lineHeight = 24)
        {
            SetText(text, padding, lineHeight);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (_currentLetters == null) return;

            foreach (var s in _currentLetters.Values)
            {
                s?.Update(gameTime);
            }
        }

        private bool _tryGetLetter(string name, out AnimatedWobblerSprite sprite, out string compoundName)
        {
            sprite = null;
            compoundName = "";
            if (ContentLoader.LoadedContent == null) return false;

            var state = 0;

            if (name != ":" && name.Contains(':'))
            {
                var items = name.Split(':');
                name = items[0];
                state = int.TryParse(items[1], out var _s) ? _s : 0;
            }

            var entityName = name switch
            {
                "?" => "what",
                "/" => "text_fwslash",
                "-" => "text_hyphen",
                "\"" => "text_quote",
                "'" => "text_apos",
                ":" => "text_colon",
                "[" => "text_lsqbr",
                "]" => "text_rsqbr",
                "," => "text_comma",
                _ => name.Length == 1 ? $"text_{name}" : name,
            };
            compoundName = $"{entityName}.{state}";

            if (!_cache.ContainsKey(compoundName))
            {
                if (ContentLoader.LoadedContent.SpriteValues.TryGetValue(entityName, out var textSprite))
                {
                    _cache[compoundName] = new AnimatedWobblerSprite(textSprite, state);
                }
                else
                {
                    return false;
                }
            }

            sprite = _cache[compoundName];
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

            var rgb = str.Split(',').Select(c => int.TryParse(c.Trim(), NumberStyles.HexNumber, null, out var r) ? r : -1).ToArray();
            if (rgb.Length == 3 && rgb.All(x => x > 0f)) 
            {
                return new Color(rgb[0] / 255f, rgb[1] / 255f, rgb[2] / 255f);
            }

            return null;
        }

        public void SetText(string str, int padding = 0, int lineHeight = 24)
        {
            Graphics.RemoveAllChildren();
            Name = $"Text: {str}";
            Graphics.Name = Name;

            _currentLetters = new();

            string? parsingItem = null;

            var x = 0;
            var y = 0;
            AnimatedWobblerSprite letter;
            string compoundName;
            var color = Color.White;

            void addCharacter()
            {
                _currentLetters[compoundName] = letter;
                var sprite = new SpriteContainer()
                {
                    x = x,
                    y = y - (letter.CurrentWobbler.Size.Y - lineHeight) / 2,
                    Name = compoundName + "-textcontainer",
                };
                sprite.SetColor(color);
                sprite.AddChild(letter);
                Graphics.AddChild(sprite);
                x += letter.CurrentWobbler.Size.X + padding;
            }

            void forceOutputOfParsing(bool closeBracket)
            {
                if (parsingItem == null) return;

                if (_tryGetLetter("[", out letter, out compoundName))
                    addCharacter();
                var t = parsingItem;
                parsingItem = null;
                addCharacters(t);
                if (closeBracket)
                {
                    if (_tryGetLetter("]", out letter, out compoundName))
                        addCharacter();
                }
            }

            void addCharacters(string text)
            {
                foreach (var c in text)
                {
                    var character = c.ToString().ToLower();

                    if (parsingItem != null)
                    {
                        if (c == ']')
                        {
                            if (_getColor(parsingItem) is Color _color)
                            {
                                color = _color;
                            }
                            else if (_tryGetLetter(parsingItem, out letter, out compoundName))
                            {
                                addCharacter();
                            }
                            else
                            {
                                forceOutputOfParsing(true);
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

                    if (c == '\t')
                    {
                        x += 48;
                        continue;
                    }

                    if (c == '[')
                    {
                        parsingItem = "";
                        continue;
                    }

                    if (_tryGetLetter(character, out letter, out compoundName))
                    {
                        addCharacter();
                        continue;
                    }

                    x += 24;
                }
            }

            addCharacters(str);
            forceOutputOfParsing(false);
        }
    }
}
