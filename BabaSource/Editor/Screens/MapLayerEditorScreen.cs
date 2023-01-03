using Core.Content;
using Core.Screens;
using Core.Utils;
using Editor.SaveFormats;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapLayerEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine { get; set; }

        ColorPickerScreen? colorPicker;
        private readonly MapLayer mapLayer;
        private readonly ObjectData cursor = new();

        public MapLayerEditorScreen(ScreenStack stack, MapLayer mapLayer)
        {
            stateMachine = new StateMachine<EditorStates, KeyPress>("map layer editor")
                .State(
                    EditorStates.ChangeObjectColor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.MapEditor,
                        _ => EditorStates.ChangeObjectColor,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop())
                        .AddOnEnter(() =>
                        {
                            colorPicker = new();
                            stack.Add(colorPicker);
                        })
                ).State(
                    EditorStates.MapEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Up } => cursorUp(),
                        KeyPress { KeyPressed: Keys.Down } => cursorDown(),
                        KeyPress { KeyPressed: Keys.Left } => cursorLeft(),
                        KeyPress { KeyPressed: Keys.Right } => cursorRight(),
                        _ => EditorStates.MapEditor,
                    }
                );
            AddChild(new MapLayerDisplay(mapLayer, cursor));
            this.mapLayer = mapLayer;
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.MapEditor);
            editorCommands();
        }

        private void editorCommands()
        {
            SetCommands(new()
            {
                { CommonStrings.ALL_ARROW, "move cursor" },
                { "c", "color" },
                { "t", "text" },
                { "r", "rotate" },
                { "p", "pick new object" },
                { "s", "change layer size" },
                { CommonStrings.CTRL_PLUS + "s", "to save" },
                { CommonStrings.ESCAPE, "stop editing objects" },
            });
        }

        private EditorStates cursorUp()
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y - 1, (int)mapLayer.height);
            return EditorStates.MapEditor;
        }
        private EditorStates cursorDown()
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y + 1, (int)mapLayer.height);
            return EditorStates.MapEditor;
        }
        private EditorStates cursorLeft()
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x - 1, (int)mapLayer.width);
            return EditorStates.MapEditor;
        }
        private EditorStates cursorRight()
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x + 1, (int)mapLayer.width);
            return EditorStates.MapEditor;
        }

        public override EditorStates Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
        {
            _ => EditorStates.None,
        };

        protected override void OnDispose()
        {
            stateMachine.Dispose();
        }
    }
}
