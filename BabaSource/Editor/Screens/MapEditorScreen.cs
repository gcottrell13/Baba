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
        private string loadedMapName = string.Empty;
        private Text t;

        public MapEditorScreen()
        {
            t = new Text("Map editor");
            AddChild(t);
            SetCommands(new()
            {
                { "[text_escape]", "go back to world" },
                { "[text_ctrl]+s", "to save" },
            });
        }

        public void LoadMap(string name)
        {
            loadedMapName = name;
            t.SetText($"Map Editor: {name}");
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
