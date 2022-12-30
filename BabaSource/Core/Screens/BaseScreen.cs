using Core.UI;
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
        private readonly TextWithBoxOutline commandDisplay = new();

        protected void SetOffsetX(int x)
        {
            Graphics.x = x;
            commandDisplay.Graphics.x = -x;
        }

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

            var lines = new List<ListOfTextChar>() { new() };
            var space = new[] { new ObjectSelect(" ") };

            foreach (var (disp, expl) in commands)
            {
                var str = ParseText($"{legend}{disp}[white]: {expl}");
                var last = lines.Last();
                if (last.TextCharLength() + str.TextCharLength() > width)
                {
                    lines.Add(str);
                }
                else
                {
                    if (last.Count > 0)
                        last.AddRange(space.Repeat(2));
                    last.AddRange(str);
                }
            }

            lines.Last().AddRange(space.Repeat(width - lines.Last().TextCharLength()));

            AddChild(commandDisplay);
            commandDisplay.SetText(lines, new() { backgroundColor=background });
            commandDisplay.Graphics.xscale = scale;
            commandDisplay.Graphics.yscale = scale;
            commandDisplay.Graphics.y = ScreenHeight - (lines.Count + 2) * Text.DEFAULT_LINE_HEIGHT * scale;
        }

        public static class CommonStrings
        {
            public const string ALL_ARROW = "[arrow:1][arrow:2][arrow:4][arrow:8]";
            public const string UD_ARROW = "[arrow:2][arrow:8]";
            public const string LR_ARROW = "[arrow:4][arrow:1]";
            public const string ESCAPE = "ESC";
            public const string CTRL_PLUS = "CTRL+";
            public const string NAME_CHARS = "a-z0-9_";
            public const string ENTER = "enter";
        }
    }
}
