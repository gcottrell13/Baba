using Core.UI;
using Core.Screens;
using System;
using System.Diagnostics;
using System.Linq;
using Core.Content;
using Editor.Editors;
using Core.Utils;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Editor.Screens
{
    internal class MapEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine;
        private MapEditor editor;
        private MapLayerEditorScreen layerEditorScreen;

        private TextInputBox titleText = new(format: "[90,ff,90]Map Editor: [white]{}") { Name = "editortitle" };
        private readonly MapData mapData;

        public MapEditorScreen(ScreenStack stack, MapData mapData)
        {
            editor = new(mapData);
            var theme = Editor.EDITOR.regions.FirstOrDefault(x => x.id == mapData.regionId)?.theme;
            layerEditorScreen = new("layer 1", stack, mapData.layer1, EditorStates.MapEditor, theme);

            var layer1Display = new MapLayerDisplay("layer 1", mapData.layer1, theme);
            layer1Display.Graphics.y = 25;
            var layer2Display = new MapLayerDisplay("layer 2", mapData.layer2, theme);
            layer2Display.Graphics.y = 25;

            AddChild(layer1Display);
            AddChild(layer2Display);
            titleText.SetText(mapData.name);
            AddChild(titleText);


            RenameScreen? renameScreen = null;
            RegionPickerScreen? regionPicker = null;

            stateMachine = new StateMachine<EditorStates, KeyPress>("map editor", EditorStates.None)
                .State(
                    EditorStates.MapEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.WorldEditor,
                        KeyPress { Text: '1' } => EditorStates.EditMapLayer1,
                        KeyPress { Text: '2' } => EditorStates.EditMapLayer2,
                        KeyPress { Text: 'r' } => EditorStates.SelectMapRegion,
                        KeyPress { Text: 'n' } => EditorStates.RenamingMap,
                        KeyPress { KeyPressed: Keys.S, ModifierKeys: ModifierKeys.Ctrl } => SaveMap(),
                        _ => EditorStates.None,
                    }
                )
                .State(
                    EditorStates.EditMapLayer1,
                    c => layerEditorScreen!.Handle(c),
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            // the edit functions make a new screen object
                            var theme = Editor.EDITOR.regions.FirstOrDefault(x => x.id == mapData.regionId)?.theme;
                            layerEditorScreen = new("layer 1", stack, Editor.EDITOR.currentMap!.layer1, EditorStates.MapEditor, theme);
                            stack.Add(layerEditorScreen);
                            layerEditorScreen.init();
                        })
                )
                .State(
                    EditorStates.EditMapLayer2,
                    c => layerEditorScreen!.Handle(c),
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            // the edit functions make a new screen object
                            var theme = Editor.EDITOR.regions.FirstOrDefault(x => x.id == mapData.regionId)?.theme;
                            layerEditorScreen = new("layer 2", stack, Editor.EDITOR.currentMap!.layer2, EditorStates.MapEditor, theme);
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
                            regionPicker = new(regions, regions.FirstOrDefault(x => x.id == mapData.regionId))
                            {
                                OnEdit = editRegion,
                                OnAdd = addRegion,
                                OnSelect = (Region region) =>
                                {
                                    Editor.EDITOR.currentMap!.regionId = region.id;
                                    layer1Display.theme = region.theme;
                                    layer2Display.theme = region.theme;
                                },
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
                        _ => EditorStates.None,
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

        protected override void OnUpdate(GameTime gameTime)
        {
            var layer1Display = ChildByName("layer 1")!;
            var layer2Display = ChildByName("layer 2")!;
            layer2Display.Graphics.x = ScreenWidth / 2 / Graphics.xscale;
            layer2Display.Graphics.xscale = (ScreenWidth / 2f) / (mapData.layer2.width * 25);
            layer2Display.Graphics.yscale = layer2Display.Graphics.xscale;

            layer1Display.Graphics.xscale = (ScreenWidth / 2f) / (mapData.layer1.width * 25);
            layer1Display.Graphics.yscale = layer1Display.Graphics.xscale;

            base.OnUpdate(gameTime);
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
