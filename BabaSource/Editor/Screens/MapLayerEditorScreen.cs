﻿using Core.Content;
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
using static Editor.Screens.AddTextToObjectScreen;

namespace Editor.Screens
{
    internal class MapLayerEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine { get; set; }

        private ColorPickerScreen? colorPicker;
        private ObjectPickerScreen? objectPicker;
        private NumberPickerScreen? resizeScreen;
        private AddTextToObjectScreen? addTextToObjectScreen;
        private readonly MapLayer mapLayer;
        private readonly MapLayerEditor mapLayerEditor;
        private readonly MapLayerDisplay layerDisplay;

        public MapLayerEditorScreen(string name, ScreenStack stack, MapLayer mapLayer, string? theme)
        {
            mapLayerEditor = new(mapLayer);
            this.mapLayer = mapLayer;
            Name = name;
            layerDisplay = new MapLayerDisplay(name, mapLayer, mapLayerEditor.cursor, theme);

            stateMachine = new StateMachine<EditorStates, KeyPress>("map layer editor", EditorStates.None)
                .State(
                    EditorStates.ChangeObjectColor,
                    c => colorPicker?.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.EditMapLayer1,
                        PickerState.ClosePick => EditorStates.EditMapLayer1,
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
                    EditorStates.EditMapLayer1,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.MapEditor,
                        KeyPress { KeyPressed: Keys.C, ModifierKeys: ModifierKeys.Ctrl } => copyObject(),
                        KeyPress { Text: 'c' } => changeObjectColor(),
                        KeyPress { Text: 'p' } => pickObject(),
                        KeyPress { Text: 'x' } => EditorStates.ResizeMapLayerWidth,
                        KeyPress { Text: 'y' } => EditorStates.ResizeMapLayerHeight,
                        KeyPress { Text: 't' } => EditorStates.AddingTextToObject,
                        _ => mapLayerEditor.handleInput(c.KeyPressed, KeyboardState.IsKeyDown(Keys.Space)),
                    }
                )
                .State(
                    EditorStates.ObjectPicker,
                    c => objectPicker?.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.EditMapLayer1,
                        PickerState.ClosePick => EditorStates.EditMapLayer1,
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
                    EditorStates.AddingTextToObject,
                    c => addTextToObjectScreen!.Handle(c) switch
                    {
                        TextObjectStates.Save => EditorStates.EditMapLayer1,
                        TextObjectStates.Cancel => EditorStates.EditMapLayer1,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            var obj = mapLayerEditor.ObjectAtCursor();
                            addTextToObjectScreen = new AddTextToObjectScreen(obj!.text)
                            {
                                OnSave = setObjectText,
                            };
                            stack.Add(addTextToObjectScreen);
                        })
                )
                .State(
                    EditorStates.ResizeMapLayerWidth,
                    c => resizeScreen!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.EditMapLayer1,
                        PickerState.ClosePick => EditorStates.EditMapLayer1,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            resizeScreen = new(10, 25, mapLayer.width)
                            {
                                OnSelect = (obj) =>
                                {
                                    mapLayer.width = uint.Parse(obj);
                                },
                            };
                            stack.Add(resizeScreen);
                        })
                )
                .State(
                    EditorStates.ResizeMapLayerHeight,
                    c => resizeScreen!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.EditMapLayer1,
                        PickerState.ClosePick => EditorStates.EditMapLayer1,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            resizeScreen = new(10, 25, mapLayer.height)
                            {
                                OnSelect = (obj) =>
                                {
                                    mapLayer.height = uint.Parse(obj);
                                },
                            };
                            stack.Add(resizeScreen);
                        })
                );
            AddChild(layerDisplay);
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.EditMapLayer1);
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
                { "x", "width" },
                { "y", "height" },
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

        private EditorStates textToObject()
        {
            if (mapLayerEditor.ObjectAtCursor() == null) return EditorStates.None;
            return EditorStates.AddingTextToObject;
        }

        private void setObjectText(string text)
        {
            mapLayerEditor.ObjectAtCursor()!.text = text;
        }

        private EditorStates copyObject()
        {
            mapLayerEditor.TryCopyObjectAtCursor();
            layerDisplay.SetSelectedObject(mapLayerEditor.currentObject);
            return EditorStates.None;
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
