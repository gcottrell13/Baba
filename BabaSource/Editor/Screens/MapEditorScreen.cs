using Content.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapEditorScreen : BaseScreen
    {
        public MapEditorScreen()
        {
            AddChild(new Text("Map editor, [text_escape] to go back, [text_ctrl]+s to save"));
        }

        public int TrySavingMap()
        {
            if (KeyboardState.GetPressedKeys().Contains(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                // save
                Debug.WriteLine("saving!");
            }
            else
            {
                Debug.WriteLine("not saving =(");
            }
            return 0;
        }
    }
}
