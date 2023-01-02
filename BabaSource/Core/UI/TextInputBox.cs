using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Core.UI
{
    public class TextInputBox : GameObject
    {
        public string DisallowedCharacters = string.Empty;
        public string Text { get; private set; } = string.Empty;
        private Text.TextOptions TextOptions = new();

        private string Format;

        public TextInputBox(string format = "{}")
        {
            Format = format.Replace("{}", "{0}");
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
                Format = format.Replace("{}", "{0}");
                SetText(Text);
            }
        }

        public void SetOptions(Text.TextOptions options)
        {
            TextOptions = options;
            (ChildByName("text") as Text)?.SetText(string.Format(Format, Text), TextOptions);
        }

        public void SetText(string text)
        {
            foreach (var d in DisallowedCharacters)
            {
                text = text.Replace(d.ToString(), "");
            }
            Text = text;
            (ChildByName("text") as Text)?.SetText(string.Format(Format, text), TextOptions);
        }

    }
}
