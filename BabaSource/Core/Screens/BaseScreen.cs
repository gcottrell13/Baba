﻿using Core.UI;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core.UI.Text;

namespace Core.Screens
{
    public abstract class BaseScreen : GameObject
    {
        private Dictionary<string, string> commands = new();
        private readonly Text display = new();

        public bool Transparent { get; protected set; } = false;

        public void SetCommands(Dictionary<string, string> commands, float scale = 1f)
        {
            var width = (int)((ScreenWidth / scale) / (Text.DEFAULT_BLOCK_WIDTH + Text.DEFAULT_PADDING)) - 2;

            this.commands = commands;

            var @long = $"[line:{Joinable.LR}]".Repeat(width);
            var top = $"[line:{Joinable.DR}]" + @long + $"[line:{Joinable.DL}]";
            var bottom = $"[line:{Joinable.UR}]" + @long + $"[line:{Joinable.UL}]";
            var upDownLine = $"[line:{Joinable.UD}]";
            var lines = new List<ListOfTextChar>() { new() }; 

            foreach (var (disp, expl) in commands)
            {
                var str = Text.ParseText($"{disp}-{expl}");
                var last = lines.Last();
                if (last.TextCharLength() + str.TextCharLength() > width)
                {
                    lines.Add(str);
                }
                else
                {
                    if (last.Count > 0)
                        last.AddRange(new[] { new ObjectSelect(" ") }.Repeat(2));
                    last.AddRange(str);
                }
            }

            IEnumerable<string> getLines()
            {
                yield return top;
                foreach (var line in lines)
                {
                    var padding = " ".Repeat(width - line.TextCharLength());
                    yield return $"{upDownLine}{line}{padding}{upDownLine}";
                }
                yield return bottom;
            }

            AddChild(display);
            display.SetText(string.Join("\n", getLines()));
            display.Graphics.x = 0;
            display.Graphics.y = ScreenHeight - (lines.Count + 2) * display.CurrentLineHeight;
        }

    }
}
