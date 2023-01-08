using Core.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using Core.Utils;
using Microsoft.Xna.Framework;
using Editor.SaveFormats;
using Microsoft.Xna.Framework.Input;
using Editor.Editors;
using System.Linq;

namespace Editor.Screens
{
    internal class WorldEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine;

        private WorldEditor? editor;
        private WorldPickerScreen? worldPicker;
        private MapPickerScreen? mapPicker;
        private RenameScreen? renameScreen;
        private MapEditorScreen? mapEditorScreen;
        private RegionEditorScreen? regionEditorScreen;
        private RegionPickerScreen? regionPicker;

        private WorldEditorDisplay? worldEditorDisplay;

        private TextInputBox titleText = new(format: "[90,90,ff]World: [white]{}") { Name = "editortitle" };

        public WorldEditorScreen(ScreenStack stack, ReadonlySavesList saves)
        {
            stateMachine = new StateMachine<EditorStates, KeyPress>("world editor", EditorStates.None)
                .State(
                    EditorStates.SelectingWorld,
                    c => worldPicker!.Handle(c) switch
                    {
                        PickerState.ClosePick => EditorStates.WorldEditor,
                        PickerState.CloseAdd => EditorStates.WorldEditor,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop())
                        .AddOnEnter(() =>
                        {
                            worldPicker = new(saves)
                            {
                                OnSelect = LoadWorld,
                                OnAdd = NewWorld,
                            };
                            stack.Add(worldPicker);
                        })
                )
                .State(
                    EditorStates.WorldEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Enter } => Editor.EDITOR.HasWorldLoaded() ? EditorStates.WorldEditor : EditorStates.SelectingWorld,
                        KeyPress { KeyPressed: Keys.S, ModifierKeys: ModifierKeys.Ctrl } => SaveWorld(),
                        KeyPress { Text: 'm' } => EditorStates.WorldEditorPickMap,
                        KeyPress { Text: 'n' } => EditorStates.RenamingWorld,
                        KeyPress { Text: 'r' } => EditorStates.SelectMapRegion,
                        KeyPress { Text: 'z' } => ZoomOut(),
                        KeyPress { Text: 'x' } => ZoomIn(),
                        KeyPress { Text: 'e' } => editAtCursor(),
                        KeyPress { KeyPressed: Keys k } => editorhandle(k),
                    },
                    def => def
                        .AddOnEnter(() => editorCommands())
                )
                .State(
                    EditorStates.WorldEditorPickMap,
                    c => mapPicker!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => EditorStates.WorldEditor,
                        PickerState.CloseEdit => EditorStates.MapEditor,
                        PickerState.CloseAdd => EditorStates.MapEditor,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnEnter(() =>
                        {
                            mapPicker = new(Editor.EDITOR.mapDatas)
                            {
                                OnAdd=() => NewMap(),
                                OnSelect=(obj) => SetPickedMap(obj),
                                OnEdit=(obj) => EditMap(obj),
                            };
                            stack.Add(mapPicker);
                        })
                        .AddOnLeave(() => stack.Pop())
                )
                .State(
                    EditorStates.RenamingWorld,
                    c => renameScreen!.Handle(c) switch
                    {
                        RenameScreen.RenameStates.Cancel => EditorStates.WorldEditor,
                        RenameScreen.RenameStates.Save => EditorStates.WorldEditor,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            renameScreen = new(editor!.save.worldName, "name world: {}")
                            {
                                OnSave=onRenameWorld,
                            };
                            stack.Add(renameScreen);
                        })
                )
                .State(
                    EditorStates.SelectMapRegion,
                    c => regionPicker!.Handle(c) switch {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => EditorStates.RegionEditor,
                        PickerState.CloseAdd => EditorStates.RegionEditor,
                        _ => EditorStates.None,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            var regions = Editor.EDITOR.regions;
                            regionPicker = new(regions, null)
                            {
                                OnSelect=editRegion,
                                OnAdd=addRegion,
                            };
                            stack.Add(regionPicker);
                        })
                )
                .State(
                    EditorStates.RegionEditor,
                    c => regionEditorScreen!.Handle(c),
                    def => def
                        .AddOnLeave(() => stack.Pop().Dispose())
                        .AddOnEnter(() =>
                        {
                            regionEditorScreen = new(stack, Editor.EDITOR.currentRegion!);
                            stack.Add(regionEditorScreen);
                            regionEditorScreen.init();
                        })
                )
                .State(
                    EditorStates.MapEditor,
                    c => mapEditorScreen!.Handle(c),
                    def => def
                        .AddOnLeave(() => stack.Pop()?.Dispose())
                        .AddOnEnter(() =>
                        {
                            mapEditorScreen = new(stack, Editor.EDITOR.currentMap!);
                            stack.Add(mapEditorScreen);
                            mapEditorScreen.init();
                        })
                );
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.SelectingWorld);
            editorCommands();
            AddChild(titleText);
        }

        private void editorCommands()
        {
            var d = new Dictionary<string, string>();

            if (editor == null)
            {
                d.Add(CommonStrings.ENTER, "select world");
            }
            else
            {
                d.Add("z", "zoom out");
                d.Add("x", "zoom in");
                d.Add(CommonStrings.ALL_ARROW, "move cursor");
                d.Add("m", "pick map");
                d.Add("r", "edit regions");
                d.Add("n", "rename world");
                d.Add(CommonStrings.CTRL_PLUS + "s", "save world");
            }

            SetCommands(d);
        }

        public override EditorStates Handle(KeyPress key) => stateMachine.SendAction(key) switch
        {
            _ => EditorStates.None,
        };

        public void SetEditor(WorldEditor editor)
        {
            this.editor = editor;
            titleText.SetText(editor.save.worldName);
        }

        private void onRenameWorld(string name)
        {
            editor!.save.worldName = name;
            titleText.SetText(name);
        }

        public void LoadWorld(SaveFormat save)
        {
            Editor.EDITOR.LoadWorld(save);
            SetEditor(new WorldEditor(save));

            worldEditorDisplay = new(save, editor!.cursor, ScreenWidth * 2 / 3);
            worldEditorDisplay.Graphics.y = 25;
            AddChild(worldEditorDisplay);
        }

        public void NewWorld()
        {
            Editor.EDITOR.NewWorld();
        }

        private void addRegion()
        {
            Editor.EDITOR.LoadRegion(Editor.EDITOR.NewRegion());
        }

        private void editRegion(Region region)
        {
            Editor.EDITOR.LoadRegion(region);
        }

        private EditorStates editAtCursor()
        {
            var id = editor?.MapAtCursor()?.mapDataId;
            var map = Editor.EDITOR.mapDatas.FirstOrDefault(x => x.id == id);
            if (map == null) return EditorStates.None;
            EditMap(map);
            return EditorStates.MapEditor;
        }

        public EditorStates SetPickedMap(MapData? pickedMap)
        {
            if (pickedMap == null) throw new ArgumentNullException(nameof(pickedMap));
            editor?.setPickedMap(pickedMap);
            titleText.SetText($"picked [green]{pickedMap.name}[white] to place");
            return EditorStates.WorldEditor;
        }

        public EditorStates SaveWorld()
        {
            editor?.saveWorld();
            return EditorStates.None;
        }

        private EditorStates editorhandle(Keys k)
        {
            editor?.handleInput(k);
            return EditorStates.None;
        }

        private void NewMap()
        {
            Editor.EDITOR.LoadMap(Editor.EDITOR.NewMap());
        }

        private void EditMap(MapData selected)
        {
            Editor.EDITOR.LoadMap(selected);
        }

        private EditorStates ZoomOut()
        {
            worldEditorDisplay?.ZoomOut();
            return EditorStates.None;
        }

        private EditorStates ZoomIn()
        {
            worldEditorDisplay?.ZoomIn();
            return EditorStates.None;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // titleText.SetText(editor?.save.worldName ?? "none");
        }


        protected override void OnDispose()
        {
            stateMachine.Dispose();
        }
    }
}
