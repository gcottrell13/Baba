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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapLayerEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine { get; set; }

        private ColorPickerScreen? colorPicker;
        private ObjectPickerScreen? objectPicker;
        private RenameScreen? resizeScreen;
        private readonly MapLayer mapLayer;
        private readonly MapLayerEditor mapLayerEditor;
        private readonly MapLayerDisplay layerDisplay;

        public MapLayerEditorScreen(string name, ScreenStack stack, MapLayer mapLayer, string? theme)
        {
            mapLayerEditor = new(mapLayer);
            this.mapLayer = mapLayer;
            Name = name;
            layerDisplay = new MapLayerDisplay(name, mapLayer, mapLayerEditor.cursor, theme);

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
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            colorPicker = new(mapLayerEditor.ObjectAtCursor()?.color ?? 0)
                            {
                                OnSelect=(color) => mapLayerEditor?.TrySetObjectColor(color.value),
                            };
                            stack.Add(colorPicker);
                        })
                )
                .State(
                    EditorStates.EditMapLayer,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.MapEditor,
                        KeyPress { KeyPressed: Keys.C, ModifierKeys: ModifierKeys.Ctrl } => copyObject(),
                        KeyPress { Text: 'c' } => changeObjectColor(),
                        KeyPress { Text: 'p' } => pickObject(),
                        //KeyPress { Text: 's' } => resize(),
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
                            objectPicker = new(mapLayerEditor.ObjectAtCursor()?.name)
                            {
                                OnSelect=(obj) =>
                                {
                                    var d = mapLayerEditor.SetSelectedObject(obj.sprite);
                                    layerDisplay.SetSelectedObject(d);
                                },
                            };
                            stack.Add(objectPicker);
                        })
                        .AddOnLeave(() => stack.Pop().Dispose())
                )
                .State(
                    EditorStates.ResizeMapLayer,
                    c => resizeScreen!.Handle(c) switch
                    {
                        RenameScreen.RenameStates.Cancel => EditorStates.EditMapLayer,
                        RenameScreen.RenameStates.Save => EditorStates.EditMapLayer,
                        _ => EditorStates.RenamingMap,
                    },
                    def => def
                        .AddOnLeave(() =>
                        {
                            if (resizeScreen?.Text != null && EnumerableExtensions.TryRowColToInt(resizeScreen.Text, out var dims))
                            {
                                mapLayer.width = (uint)dims.X;
                                mapLayer.height = (uint)dims.Y;
                            }
                            stack.Pop().Dispose();
                        })
                        .AddOnEnter(() =>
                        {
                            resizeScreen = new(new Vector2(mapLayer.width, mapLayer.height).ToRowColString());
                            stack.Add(resizeScreen);
                        })
                );
            AddChild(layerDisplay);
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

        private EditorStates copyObject()
        {
            mapLayerEditor.TryCopyObjectAtCursor();
            layerDisplay.SetSelectedObject(mapLayerEditor.currentObject);
            return EditorStates.None;
        }

        private EditorStates resize()
        {
            return EditorStates.ResizeMapLayer;
        }

        public override EditorStates Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
        {
            EditorStates.MapEditor => EditorStates.MapEditor,
            EditorStates.RegionEditor => EditorStates.RegionEditor,
            _ => EditorStates.None,
        };

        protected override void OnDispose()
        {
            stateMachine.Dispose();
        }
    }
}
