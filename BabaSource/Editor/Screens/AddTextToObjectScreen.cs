using Core.Screens;
using Core.UI;
using Microsoft.Xna.Framework.Input;
using System;
using static Editor.Screens.AddTextToObjectScreen;

namespace Editor.Screens
{
    internal class AddTextToObjectScreen : BaseScreen<TextObjectStates>
    {
        public string Text { get; private set; }
        private TextInputBox inputBox;
        private readonly string startText;

        public Action<string>? OnSave;

        public AddTextToObjectScreen(string startText, string format = "Enter Text:\n{}")
        {
            Text = startText;
            this.startText = startText;
            inputBox = new(format);
            inputBox.SetText(startText);
            AddChild(inputBox);
            renameCommands();
        }

        private void renameCommands()
        {
            SetCommands(new()
            {
                { CommonStrings.ESCAPE, "cancel" },
                { CommonStrings.CTRL_PLUS + "s", "save" },
                { "...", "type text" },
            });
        }

        public override TextObjectStates Handle(KeyPress ev)
        {
            if (ev.KeyPressed == Keys.S && ev.ModifierKeys == ModifierKeys.Ctrl)
            {
                OnSave?.Invoke(Text);
                return TextObjectStates.Save;
            }
            if (ev.KeyPressed == Keys.Escape)
            {
                Text = startText;
                return TextObjectStates.Cancel;
            }

            inputBox.HandleInput(ev);
            Text = inputBox.Text;
            return TextObjectStates.None;
        }

        protected override void OnDispose()
        {
            inputBox.Destroy();
        }

        public enum TextObjectStates
        {
            None,
            Cancel,
            Save,
        }
    }
}
