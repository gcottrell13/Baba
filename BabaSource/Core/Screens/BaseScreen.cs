﻿using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core.UI.Text;

namespace Core.Screens
{
    public abstract class BaseScreen : GameObject
    {
        public bool Transparent { get; protected set; }
        public abstract void HideCommands();
        public abstract void ShowCommands();
        public abstract void SetCommands(Dictionary<string, string> commands, float scale = 0.75f, Color? background = null, Color? legendColor = null);
    }

    public abstract class BaseScreen<THandleResult> : BaseScreen
    {
        private Dictionary<string, string> commands = new();
        private readonly Text commandDisplay = new();

        protected void SetOffsetX(int x)
        {
            Graphics.x = x;
            commandDisplay.Graphics.x = -x;
        }

        public bool Transparent { get; protected set; } = false;

        public abstract THandleResult Handle(KeyPress ev);

        public override void HideCommands()
        {
            commandDisplay.Graphics.alpha = 0f;
        }

        public override void ShowCommands()
        {
            commandDisplay.Graphics.alpha = 1f;
        }

        public override void SetCommands(Dictionary<string, string> commands, float scale = 0.75f, Color? background = null, Color? legendColor = null)
        {
            var width = (int)((ScreenWidth / scale) / (Text.DEFAULT_BLOCK_WIDTH + Text.DEFAULT_PADDING)) - 2;

            var legend = (legendColor ?? Color.Brown).ToHexTriple();

            this.commands = commands;

            var @long = $"[line:{Joinable.LR}]".Repeat(width);
            var top = $"[line:{Joinable.DR}]" + @long + $"[line:{Joinable.DL}]";
            var bottom = $"[line:{Joinable.UR}]" + @long + $"[line:{Joinable.UL}]";
            var upDownLine = $"[line:{Joinable.UD}]";
            var lines = new List<ListOfTextChar>() { new() }; 

            foreach (var (disp, expl) in commands)
            {
                var str = Text.ParseText($"{legend}{disp}[white]: {expl}");
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

            AddChild(commandDisplay);
            commandDisplay.SetText(string.Join("\n", getLines()), new() { background=background });
            commandDisplay.Graphics.xscale = scale;
            commandDisplay.Graphics.yscale = scale;
            commandDisplay.Graphics.y = ScreenHeight - (lines.Count + 2) * Text.DEFAULT_LINE_HEIGHT * scale;
        }

        public static class CommonStrings
        {
            public const string ALL_ARROW = "[arrow:1][arrow:2][arrow:4][arrow:8]";
            public const string UD_ARROW = "[arrow:2][arrow:8]";
            public const string LR_ARROW = "[arrow:4][arrow:1]";
            public const string ESCAPE = "[text_escape]";
            public const string CTRL_PLUS = "[text_ctrl]+";
            public const string NAME_CHARS = "a-z0-9_";
            public const string ENTER = "enter";
        }
    }
}
