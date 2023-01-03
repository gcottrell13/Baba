using Core.Screens;
using Core.UI;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Editor.Screens.RenameScreen;

namespace Editor.Screens
{
    internal class RenameScreen : BaseScreen<RenameStates>
    {
        public string Text { get; private set; }
        private TextInputBox inputBox;
        private readonly string startText;

        public RenameScreen(string startText, string format = "{}")
        {
            Text = startText;
            this.startText = startText;
            inputBox = new(format);
            renameCommands();
        }

        private void renameCommands()
        {
            SetCommands(new()
            {
                { CommonStrings.ESCAPE, "cancel" },
                { CommonStrings.ENTER, "save" },
                { CommonStrings.NAME_CHARS, "type a name" },
            });
        }

        public override RenameStates Handle(KeyPress ev)
        {
            if (ev.KeyPressed == Keys.Enter)
            {
                return RenameStates.Save;
            }
            if (ev.KeyPressed == Keys.Escape)
            {
                Text = startText;
                return RenameStates.Cancel;
            }

            inputBox.HandleInput(ev);
            return RenameStates.None;
        }

        protected override void OnDispose()
        {
            inputBox.Destroy();
        }

        public enum RenameStates
        {
            None,
            Cancel,
            Save,
        }
    }
}
