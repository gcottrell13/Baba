using Core.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using Core.Utils;
using Microsoft.Xna.Framework;
using Editor.SaveFormats;
using Microsoft.Xna.Framework.Input;
using Editor.Editors;

namespace Editor.Screens
{
    internal class WorldEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine;

        private WorldEditor? editor;
        private WorldPickerScreen? worldPicker;
        private MapPickerScreen? mapPicker;

        private TextInputBox titleText = new(format: "[90,90,ff]World: {}") { Name = "editortitle" };

        public WorldEditorScreen(ScreenStack stack, ReadonlySavesList saves)
        {
            stateMachine = new StateMachine<EditorStates, KeyPress>("world editor")
                .State(
                    EditorStates.SelectingWorld,
                    c => worldPicker!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => LoadWorld(worldPicker.Selected!),
                        PickerState.CloseAdd => NewWorld(),
                        _ => EditorStates.SelectingWorld,
                    },
                    def => def
                        .AddOnLeave(() => stack.Pop())
                        .AddOnEnter(() =>
                        {
                            worldPicker = new(saves);
                            stack.Add(worldPicker);
                        })
                ).State(
                    EditorStates.WorldEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Enter } => Editor.EDITOR.HasWorldLoaded() ? EditorStates.WorldEditor : EditorStates.SelectingWorld,
                        KeyPress { KeyPressed: Keys.S, ModifierKeys: ModifierKeys.Ctrl } => SaveWorld(),
                        KeyPress { Text: 'm' } => EditorStates.WorldEditorPickMap,
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.SelectingWorld,
                        KeyPress { Text: 'n' } => EditorStates.RenamingWorld,
                        KeyPress { KeyPressed: Keys k } => editorhandle(k),
                    },
                    def => def
                        .AddOnEnter(() => editorCommands())
                ).State(
                    EditorStates.WorldEditorPickMap,
                    c => mapPicker!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => SetPickedMap(mapPicker!.Selected),
                        PickerState.CloseEdit => EditMap(),
                        PickerState.CloseAdd => NewMap(),
                        _ => EditorStates.WorldEditorPickMap,
                    },
                    def => def
                        .AddOnEnter(() =>
                        {
                            mapPicker = new(Editor.EDITOR.mapDatas);
                            stack.Add(mapPicker);
                        })
                        .AddOnLeave(() => stack.Pop())
                ).State(
                    EditorStates.RenamingWorld,
                    c =>
                    {
                        if (editor != null)
                        {
                            if (c.KeyPressed == Keys.Enter)
                            {
                                editor.save.worldName = titleText.Text;
                                return EditorStates.WorldEditor;
                            }
                            if (c.KeyPressed == Keys.Escape)
                            {
                                titleText.SetText(editor.save.worldName);
                                return EditorStates.WorldEditor;
                            }
                        }
                        titleText.HandleInput(c);
                        return EditorStates.RenamingWorld;
                    },
                    def => def
                        .AddOnEnter(() => renameCommands())
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
                d.Add("q", "zoom out");
                d.Add("e", "zoom in");
                d.Add(CommonStrings.ALL_ARROW, "move cursor");
                d.Add("m", "pick map");
                d.Add("r", "edit regions");
                d.Add("n", "rename world");
                d.Add(CommonStrings.ESCAPE, "select world");
                d.Add(CommonStrings.CTRL_PLUS + "s", "save world");
            }

            SetCommands(d);
        }

        private void renameCommands()
        {
            SetCommands(new()
            {
                { CommonStrings.ESCAPE, "cancel" },
                { CommonStrings.ENTER, "save" },
                { CommonStrings.NAME_CHARS, "type a name" },
            });
        }

        public override EditorStates Handle(KeyPress key) => stateMachine.SendAction(key) switch
        {
            EditorStates.MapEditor => EditorStates.MapEditor,
            _ => EditorStates.WorldEditor,
        };

        public void SetEditor(WorldEditor editor)
        {
            this.editor = editor;
            titleText.SetText(editor.save.worldName);
        }

        public EditorStates LoadWorld(SaveFormat save)
        {
            Editor.EDITOR.LoadWorld(save);
            SetEditor(new WorldEditor(save));
            return EditorStates.WorldEditor;
        }

        public EditorStates NewWorld()
        {
            Editor.EDITOR.NewWorld();
            return EditorStates.WorldEditor;
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
            return EditorStates.WorldEditor;
        }

        private EditorStates editorhandle(Keys k)
        {
            editor?.handleInput(k);
            return EditorStates.WorldEditor;
        }

        private EditorStates NewMap()
        {
            Editor.EDITOR.LoadMap(Editor.EDITOR.NewMap());
            return EditorStates.MapEditor;
        }

        private EditorStates EditMap()
        {
            Editor.EDITOR.LoadMap(mapPicker!.Selected);
            return EditorStates.MapEditor;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // titleText.SetText(editor?.save.worldName ?? "none");
        }
    }
}
