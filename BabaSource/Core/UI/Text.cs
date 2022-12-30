using Core;
using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;

namespace Core.UI
{
    [DebuggerDisplay("Text: {EffectiveName,nq}")]
    public class Text : GameObject
    {
        private Dictionary<string, AnimatedWobblerSprite> _currentLetters = new();
        private Dictionary<string, AnimatedWobblerSprite> _cache = new();

        public const int DEFAULT_BLOCK_WIDTH = 24;
        public const int DEFAULT_LINE_HEIGHT = 24;
        public const int DEFAULT_PADDING = 0;

        public TextOptions? CurrentOptions { get; private set; }

        public RectangleSprite? background { get; private set; }

        public string CurrentText { get; private set; } = string.Empty;
        public string EffectiveName => Name ?? CurrentText;

        public Text(string text = "", TextOptions? options = null)
        {
            SetText(text, options);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (_currentLetters == null) return;

            foreach (var s in _currentLetters.Values)
            {
                s?.Update(gameTime);
            }
        }

        private static bool _tryGetObject(string name, out string compoundName, out int state, out SpriteValues? textSprite)
        {
            compoundName = "";
            state = 0;
            textSprite = null;
            if (ContentLoader.LoadedContent == null) return false;


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
                "+" => "text_plus",
                "_" => "text_underscore",
                _ => name.Length == 1 ? $"text_{name}" : name,
            };
            compoundName = $"{entityName}:{state}";

            return ContentLoader.LoadedContent.SpriteValues.TryGetValue(entityName, out textSprite);
        }

        private bool _tryGetLetter(string name, out AnimatedWobblerSprite sprite, out string compoundName)
        {
            if (_tryGetObject(name, out compoundName, out var state, out var textSprite))
            {
                if (!_cache.ContainsKey(compoundName))
                {
                    _cache[compoundName] = new AnimatedWobblerSprite(textSprite, state);
                }
            }
            else
            {
                sprite = null!;
                return false;
            }

            sprite = _cache[compoundName];
            return true;
        }

        private static Color? _getColor(string str)
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

        /// <summary>
        /// Parse the text into objects
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ListOfTextChar ParseText(string str)
        {
            string compoundName;
            int state;
            SpriteValues? textSprite;
            string? parsingItem = null;

            IEnumerable<TextChar> forceOutputOfParsing(bool closeBracket)
            {
                if (parsingItem == null) yield break;

                yield return "[";
                var t = parsingItem;
                parsingItem = null;
                if (closeBracket)
                {
                    yield return "]";
                }
            }

            IEnumerable<TextChar> addCharacters(string text)
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
                                yield return _color;
                            }
                            else if (_tryGetObject(parsingItem, out compoundName, out state, out textSprite))
                            {
                                yield return compoundName;
                            }
                            else
                            {
                                foreach (var item in forceOutputOfParsing(true)) yield return item;
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
                        yield return new NewlineSelect();
                        continue;
                    }

                    if (c == '\t')
                    {
                        yield return " ";
                        yield return " ";
                        continue;
                    }

                    if (c == '[')
                    {
                        parsingItem = "";
                        continue;
                    }

                    yield return character;
                }
            }

            var alist = new ListOfTextChar();
            foreach (var c in addCharacters(str)) alist.Add(c);
            foreach (var item in forceOutputOfParsing(false)) alist.Add(item);
            return alist;
        }

        /// <summary>
        /// Using the result from <see cref="ParseText"/>, set the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="padding"></param>
        /// <param name="lineHeight"></param>
        /// <param name="blockWidth"></param>
        public void SetText(ListOfTextChar text, TextOptions? options = null)
        {
            options ??= new();

            Graphics.RemoveAllChildren();

            _currentLetters = new();

            var x = 0;
            var y = 0;
            AnimatedWobblerSprite letter;
            string compoundName;
            var color = Color.White;
            var name = new List<string>();

            void addCharacter()
            {
                _currentLetters[compoundName] = letter;
                var sprite = new SpriteContainer()
                {
                    x = x - (letter.CurrentWobbler.Size.X - options.blockWidth) / 2,
                    y = y - (letter.CurrentWobbler.Size.Y - options.lineHeight) / 2,
                    Name = compoundName + "-textcontainer",
                };
                sprite.SetColor(color);
                sprite.AddChild(letter);
                Graphics.AddChild(sprite);
                x += options.blockWidth + options.padding;
            }

            foreach (var t in text)
            {
                if (t is ColorSelect c)
                {
                    color = c.Color;
                }
                else if (t is ObjectSelect o)
                {
                    name.Add(o.Name);
                    if (_tryGetLetter(o.Name, out letter, out compoundName))
                    {
                        addCharacter();
                    }
                    else
                    {
                        x += options.blockWidth;
                    }
                }
                else if (t is NewlineSelect)
                {
                    x = 0;
                    y += options.lineHeight;
                }
            }

            if (options.background != null && Graphics.children.Count > 0)
            {
                background = new RectangleSprite()
                {
                    xscale = Graphics.children.Select(x => x.x + options.blockWidth).Max(),
                    yscale = Graphics.children.Select(x => x.y + options.lineHeight).Max(),
                };
                background.SetColor(options.background);
                Graphics.children.Insert(0, background);
            }

            CurrentText = text.ToString();
            Graphics.Name = "text display";
            CurrentOptions = options;
        }

        /// <summary>
        /// Set the text based on a string value
        /// </summary>
        /// <param name="str"></param>
        /// <param name="padding"></param>
        /// <param name="lineHeight"></param>
        /// <param name="blockWidth"></param>
        public void SetText(string str, TextOptions? options = null)
        {
            SetText(ParseText(str), options);
        }

        public class TextOptions
        {
            public int padding = DEFAULT_PADDING;
            public int lineHeight = DEFAULT_LINE_HEIGHT;
            public int blockWidth = DEFAULT_BLOCK_WIDTH;
            public Color? background = null;
        }

        public abstract class TextChar
        {
            public abstract int width { get; }
            public static implicit operator TextChar(Color color) { return new ColorSelect(color); }
            public static implicit operator TextChar(string name) { return new ObjectSelect(name); }
        }
        public class ColorSelect : TextChar
        {
            public override int width => 0;
            public ColorSelect(Color c)
            {
                Color = c;
            }
            public Color Color;
            public override string ToString()
            {
                return $"[{Color.R:x2},{Color.G:x2},{Color.B:x2}]";
            }
        }
        public class ObjectSelect : TextChar
        {
            public override int width => 1;
            public ObjectSelect(string n)
            {
                Name = n;
            }
            public string Name;
            public override string ToString()
            {
                if (Name.Length > 1)
                    return $"[{Name}]";
                return Name;
            }
        }
        public class NewlineSelect : TextChar
        {
            public override int width => 0;
        }

        public class ListOfTextChar : List<TextChar>
        {
            public int TextCharLength() => this.Sum(t => t.width);
            public override string ToString()
            {
                return string.Join("", this);
            }

            private readonly static ListOfTextChar space = new() { new ObjectSelect(" ") };

            public ListOfTextChar PadRight(int count)
            {
                var newlist = new ListOfTextChar();
                newlist.AddRange(this);
                newlist.AddRange(space.Repeat(count - TextCharLength()));
                return newlist;
            }
        }
    }
}
