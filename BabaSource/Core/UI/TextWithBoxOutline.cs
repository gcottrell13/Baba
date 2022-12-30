using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI
{
    public class TextWithBoxOutline : Text
    {

        public void SetText(List<ListOfTextChar> lines, TextWithBoxOutlineOptions? options = null)
        {
            options ??= new();

            var textWidth = lines.Count > 0 ? lines.Max(x => x.TextCharLength()) : 0;
            var fullInsideWidth = textWidth + options.borderPaddingLeft + options.borderPaddingRight;

            var borderColor = (options.borderColor ?? Color.White).ToHexTriple();
            var top = options.topLeftCorner + options.topBorder.Repeat(fullInsideWidth) + options.topRightCorner;
            var bottom = options.bottomLeftCorner + options.bottomBorder.Repeat(fullInsideWidth) + options.bottomRightCorner;
            var rightPad = " ".Repeat(options.borderPaddingRight);
            var leftPad = " ".Repeat(options.borderPaddingLeft);
            var topPad = string.Join("\n", new[] {
                $"{options.leftBorder}{"".Repeat(fullInsideWidth)}{options.rightBorder}",
            }.Repeat(options.borderPaddingTop));
            var bottomPad = string.Join("\n", new[] {
                $"{options.leftBorder}{"".Repeat(fullInsideWidth)}{options.rightBorder}",
            }.Repeat(options.borderPaddingBottom));

            IEnumerable<string> getLines()
            {
                yield return $"{borderColor}{top}";
                if (topPad?.Length >= 1)
                    yield return topPad;
                foreach (var line in lines)
                {
                    yield return $"{options.leftBorder}{leftPad}[white]" +
                        $"{line.PadRight(textWidth)}{rightPad}{borderColor}{options.rightBorder}";
                }
                if (bottomPad?.Length >= 1)
                    yield return bottomPad;
                yield return bottom;
            }

            base.SetText(string.Join("\n", getLines()), new() { background = options.backgroundColor });
        }

        public void SetText(string text, TextWithBoxOutlineOptions? options = null)
        {
            SetText(text.Split("\n").Select(ParseText).ToList(), options);
        }

    }
    public class TextWithBoxOutlineOptions
    {
        public string topBorder = $"[line:{Joinable.LR}]";
        public string bottomBorder = $"[line:{Joinable.LR}]";
        public string leftBorder = $"[line:{Joinable.UD}]";
        public string rightBorder = $"[line:{Joinable.UD}]";
        public string topLeftCorner = $"[line:{Joinable.DR}]";
        public string bottomLeftCorner = $"[line:{Joinable.UR}]";
        public string topRightCorner = $"[line:{Joinable.DL}]";
        public string bottomRightCorner = $"[line:{Joinable.UL}]";
        public Color? borderColor;
        public Color? backgroundColor;
        public int borderPaddingLeft = 0;
        public int borderPaddingRight = 0;
        public int borderPaddingTop = 0;
        public int borderPaddingBottom = 0;

        public int borderPaddingX { set { borderPaddingLeft = value; borderPaddingRight = value; } }
        public int borderPaddingY { set { borderPaddingTop = value; borderPaddingBottom = value; } }

        public int borderWidth { set { borderPaddingX = value; borderPaddingY = value; } }
    }
}
