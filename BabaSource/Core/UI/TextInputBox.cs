using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI
{
    public class TextInputBox : GameObject
    {
        public string DisallowedCharacters = string.Empty;
        public string Text { get; private set; } = string.Empty;
        public readonly Text.TextOptions TextOptions = new();

        public string Format = "{0}";

        public TextInputBox()
        {
            AddChild(new Text(Text) { Name = "text" });
        }

        public void HandleInput(KeyPress ev)
        {
            if (ev.KeyPressed == Keys.Back)
            {
                if (Text.Length > 1)
                    SetText(Text[..^1]);
                else
                    SetText("");
                return;
            }
            if (ev.Text == 0) return;
            if (DisallowedCharacters.Contains(ev.Text)) return;
            SetText(Text + ev.Text);
        }

        public void SetFormat(string format)
        {
            if (format != Format)
            {
                Format = format;
                SetText(Text);
            }
        }

        public void SetText(string text)
        {
            Text = text;
            (ChildByName("text") as Text)?.SetText(string.Format(Format, text), TextOptions);
        }

    }
}
