using Content.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class WorldEditorScreen : BaseScreen
    {
        public WorldEditorScreen()
        {
            AddChild(new Text("World editor, [text_escape] to go back"));
        }
    }
}
