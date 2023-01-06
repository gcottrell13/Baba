using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

namespace Core.UI
{
    public class TextInputBox : GameObject
    {
        public Regex? TextFilterRegex;

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

            if (ev.KeyPressed == Keys.Enter)
            {
                SetText(Text + "\n");
                return;
            }

            if (ev.Text == 0) return;
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
            if (TextFilterRegex != null && !TextFilterRegex.IsMatch(text))
            {
                return;
            }

            Text = text;
            (ChildByName("text") as Text)?.SetText(string.Format(Format, text), TextOptions);
        }

    }
}
