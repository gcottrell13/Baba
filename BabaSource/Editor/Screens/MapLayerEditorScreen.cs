using Core.Content;
using Core.Screens;
using Core.Utils;
using Editor.Editors;
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

        private ColorPickerScreen? colorPicker;
        private ObjectPickerScreen? objectPicker;
        private readonly MapLayer mapLayer;
        private readonly MapLayerEditor mapLayerEditor;

        public MapLayerEditorScreen(string name, ScreenStack stack, MapLayer mapLayer)
        {
            mapLayerEditor = new(mapLayer);
            this.mapLayer = mapLayer;
            Name = name;
            var display = new MapLayerDisplay(name, mapLayer, mapLayerEditor.cursor);

            stateMachine = new StateMachine<EditorStates, KeyPress>("map layer editor")
                .State(
                    EditorStates.ChangeObjectColor,
                    c => colorPicker?.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.EditMapLayer,
                        PickerState.ClosePick => EditorStates.EditMapLayer,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() =>
                        {
                            stack.Pop();
                            if (colorPicker?.Selected == null) return;
                            mapLayerEditor?.TrySetObjectColor(colorPicker.Selected.value);
                        })
                        .AddOnEnter(() =>
                        {
                            colorPicker = new(mapLayerEditor.ObjectAtCursor()?.color ?? 0);
                            stack.Add(colorPicker);
                        })
                )
                .State(
                    EditorStates.EditMapLayer,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.MapEditor,
                        KeyPress { Text: 'c' } => changeObjectColor(),
                        KeyPress { Text: 'p' } => pickObject(),
                        _ => mapLayerEditor.handleInput(c.KeyPressed, KeyboardState.IsKeyDown(Keys.Space)),
                    }
                )
                .State(
                    EditorStates.ObjectPicker,
                    c => objectPicker?.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.EditMapLayer,
                        PickerState.ClosePick => EditorStates.EditMapLayer,
                        _ => EditorStates.None
                    },
                    def => def
                        .AddOnEnter(() =>
                        {
                            objectPicker = new(mapLayerEditor.ObjectAtCursor()?.name);
                            stack.Add(objectPicker);
                        })
                        .AddOnLeave(() =>
                        {
                            stack.Pop();
                            if (objectPicker?.Selected == null) return;
                            mapLayerEditor.SetSelectedObject(objectPicker.Selected.sprite);
                            display.SetSelectedObject(ObjectPickerScreen.ObjectDefaultSprite(objectPicker.Selected.sprite));
                        })
                );
            AddChild(display);
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.EditMapLayer);
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
                { CommonStrings.ESCAPE, "stop editing objects" },
            });
        }

        private EditorStates changeObjectColor()
        {
            if (mapLayerEditor.ObjectAtCursor() == null)
            {
                return EditorStates.None;
            }
            return EditorStates.ChangeObjectColor;
        }

        private EditorStates pickObject()
        {
            return EditorStates.ObjectPicker;
        }

        public override EditorStates Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
        {
            EditorStates.MapEditor => EditorStates.MapEditor,
            _ => EditorStates.None,
        };

        protected override void OnDispose()
        {
            stateMachine.Dispose();
        }
    }
}
