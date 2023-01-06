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
        private MapEditor editor;
        private MapLayerEditorScreen layerEditorScreen;

        private TextInputBox titleText = new(format: "[90,ff,90]Map Editor: {}") { Name = "editortitle" };
        private readonly MapData mapData;

        public MapEditorScreen(ScreenStack stack, MapData mapData)
        {
            editor = new(mapData);
            var theme = Editor.EDITOR.regions.FirstOrDefault(x => x.id == mapData.regionId)?.theme;
            layerEditorScreen = new("layer 1", stack, mapData.layer1, theme);
            var layerDisplay = new MapLayerDisplay("layer 1", mapData.layer1, null, theme);
            layerDisplay.Graphics.y = 25;
            AddChild(layerDisplay);
            titleText.SetText(mapData.name);
            AddChild(titleText);


            RenameScreen? renameScreen = null;
            RegionPickerScreen? regionPicker = null;

            stateMachine = new StateMachine<EditorStates, KeyPress>("map editor")
                .State(
                    EditorStates.MapEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.WorldEditor,
                        KeyPress { Text: '1' } => editLayer1(stack),
                        KeyPress { Text: '2' } => editLayer2(stack),
                        KeyPress { Text: 'r' } => EditorStates.SelectMapRegion,
                        KeyPress { Text: 'n' } => EditorStates.RenamingMap,
                        KeyPress { KeyPressed: Keys.S, ModifierKeys: ModifierKeys.Ctrl } => SaveMap(),
                        _ => EditorStates.MapEditor,
                    }
                )
                .State(
                    EditorStates.EditMapLayer,
                    c => layerEditorScreen!.Handle(c),
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            // the edit functions make a new screen object
                            stack.Add(layerEditorScreen);
                            layerEditorScreen.init();
                        })
                )
                .State(
                    EditorStates.SelectMapRegion,
                    c => regionPicker!.Handle(c) switch {
                        PickerState.CloseCancel => EditorStates.MapEditor,
                        PickerState.ClosePick => EditorStates.MapEditor,
                        PickerState.CloseEdit => EditorStates.RegionEditor,
                        PickerState.CloseAdd => EditorStates.RegionEditor,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            var regions = Editor.EDITOR.regions;
                            regionPicker = new(regions, regions.FirstOrDefault(x => x.id == mapData.id))
                            {
                                OnEdit = editRegion,
                                OnAdd = addRegion,
                                OnSelect = selectMapRegion,
                            };
                            stack.Add(regionPicker);
                        })
                )
                .State(
                    EditorStates.RenamingMap,
                    c => renameScreen!.Handle(c) switch
                    {
                        RenameScreen.RenameStates.Cancel => EditorStates.MapEditor,
                        RenameScreen.RenameStates.Save => EditorStates.MapEditor,
                        _ => EditorStates.RenamingMap,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            renameScreen = new(editor.mapData.name, "name map: {}")
                            {
                                OnSave = renameMap,
                            };
                            stack.Add(renameScreen);
                        })
                );
            this.mapData = mapData;
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
                { "1", "edit layer 1" },
                { "2", "edit layer 2" },
                { "r", "map region" },
                { "n", "rename" },
                { CommonStrings.ESCAPE, "go back to world" },
                { CommonStrings.CTRL_PLUS + "s", "to save" },
            });
        }

        public override EditorStates Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
        {
            EditorStates.WorldEditor => EditorStates.WorldEditor,
            EditorStates.RegionEditor => EditorStates.RegionEditor,
            _ => EditorStates.MapEditor,
        };

        private EditorStates editLayer1(ScreenStack stack)
        {
            if (layerEditorScreen.Name == "layer 2")
            {
                RemoveChild(layerEditorScreen);
                layerEditorScreen.Dispose();
                var theme = Editor.EDITOR.regions.FirstOrDefault(x => x.id == mapData.regionId)?.theme;
                layerEditorScreen = new("layer 1", stack, Editor.EDITOR.currentMap!.layer1, theme);
            }
            return EditorStates.EditMapLayer;
        }

        private EditorStates editLayer2(ScreenStack stack)
        {
            if (layerEditorScreen.Name == "layer 1")
            {
                RemoveChild(layerEditorScreen);
                layerEditorScreen.Dispose();
                var theme = Editor.EDITOR.regions.FirstOrDefault(x => x.id == mapData.regionId)?.theme;
                layerEditorScreen = new("layer 2", stack, Editor.EDITOR.currentMap!.layer2, theme);
            }
            return EditorStates.EditMapLayer;
        }

        private void selectMapRegion(Region region)
        {
        }

        private void addRegion()
        {
            Editor.EDITOR.LoadRegion(Editor.EDITOR.NewRegion());
        }

        private void editRegion(Region? r)
        {
            Editor.EDITOR.LoadRegion(r);
        }

        private void renameMap(string name)
        {
            editor.mapData.name = name;
            titleText.SetText(name);
        }

        public EditorStates SaveMap()
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
            return EditorStates.MapEditor;
        }

        protected override void OnDispose()
        {
            stateMachine.Dispose();
        }
    }
}
