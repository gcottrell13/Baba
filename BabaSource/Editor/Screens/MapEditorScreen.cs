using Core.UI;
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
        private string? loadedMapName = null;
        private Text t;

        public MapEditorScreen()
        {
            t = new Text("Map editor");
            AddChild(t);
            SetCommands(new()
            {
                { CommonStrings.ESCAPE, "go back to world" },
                { "c", "obj color" },
                { "t", "obj text" },
                { "p", "obj picker" },
                { "l", "map layer 2" },
                { "r", "map region" },
                { CommonStrings.CTRL_PLUS + "s", "to save" },
            });
        }

        public void LoadMap(string? name)
        {
            loadedMapName = name;
            t.SetText($"Map Editor: {name}");
        }

        public void NewMap()
        {

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
