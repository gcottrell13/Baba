using Core.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.SaveFormats;

namespace Editor.Screens
{
    internal class MapEditorScreen : BaseScreen<EditorStates>
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

        public override EditorStates Handle(KeyPress ev)
        {
            throw new NotImplementedException();
        }

        public void LoadMap(MapData? d)
        {
            loadedMapName = d.name;
            t.SetText($"Map Editor: {d.name}");
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
