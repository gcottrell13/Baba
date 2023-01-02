using Core.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.SaveFormats;
using Editor.Editors;
using Core.Utils;
using Microsoft.Xna.Framework.Input;

namespace Editor.Screens
{
    internal class MapEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine;
        private MapLayerEditor mapEditor;

        private TextInputBox titleText = new(format: "[90,ff,90]Map Editor: {}") { Name = "editortitle" };

        public MapEditorScreen(ScreenStack stack)
        {
            mapEditor = new(ScreenWidth, ScreenHeight);
            stateMachine = new StateMachine<EditorStates, KeyPress>("map editor")
                .State(
                    EditorStates.MapEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.WorldEditor,
                    }
                );
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.MapEditor);
            editorCommands();
            AddChild(titleText);
        }

        private void editorCommands()
        {
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
            if (d == null) throw new ArgumentNullException(nameof(d));
            mapEditor.LoadMap(d.layer1);
            titleText.SetText(d.name);
        }

        public void NewMap()
        {

        }

        public int TrySavingMap()
        {
            if (KeyboardState.GetPressedKeys().Contains(Keys.LeftControl))
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
