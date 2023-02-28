using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core.UI.Text;

namespace Core.Screens
{
    public abstract class BaseScreen : GameObject, IDisposable
    {
        private bool disposedValue;

        public bool Transparent { get; protected set; }
        public abstract void HideCommands();
        public abstract void ShowCommands();
        public abstract void SetCommands(Dictionary<string, string> commands, float scale = 0.75f, Color? background = null, Color? legendColor = null);

        protected abstract void OnDispose();

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnDispose();
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseScreen()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
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
            var pad = space.Repeat(2).ToList();

            foreach (var (disp, expl) in commands)
            {
                var str = ParseText($"{legend}{disp}[white]: {expl}");
                var last = lines.Last();
                var shouldPad = Math.Min(last.Count, pad.Count);
                if (last.TextCharLength() + str.TextCharLength() + shouldPad > width)
                {
                    lines.Add(str);
                }
                else
                {
                    if (shouldPad > 0)
                        last.AddRange(pad);
                    last.AddRange(str);
                }
            }

            lines.Last().AddRange(space.Repeat(width - lines.Last().TextCharLength()));

            RemoveChild(commandDisplay);
            AddChild(commandDisplay);
            commandDisplay.SetText(lines, new() { backgroundColor=background ?? Color.Black });
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

        public static readonly Dictionary<string, string> BasicMenu = new()
        {
            { CommonStrings.UD_ARROW, "move cursor" },
            { CommonStrings.ENTER, "select" },
            { CommonStrings.ESCAPE, "back" },
        };
    }
}
