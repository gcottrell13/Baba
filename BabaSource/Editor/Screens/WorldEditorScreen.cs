using Core.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils;
using Microsoft.Xna.Framework;
using Editor.SaveFormats;
using Editor.Editors;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Editor.Screens
{
    internal class WorldEditorScreen : BaseScreen<EditorStates>
    {
        private StateMachine<EditorStates, KeyPress> stateMachine;

        private WorldEditor? editor;
        private WorldPickerScreen? worldPicker;
        private MapPickerScreen? mapPicker;

        private TextInputBox titleText = new(format: "[90,90,ff]World: {}") { Name = "editortitle" };

        public WorldEditorScreen(ScreenStack stack, List<SaveFormat> saves)
        {
            stateMachine = new StateMachine<EditorStates, KeyPress>("world editor")
                .State(
                    EditorStates.SelectingWorld,
                    c => worldPicker!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => LoadWorld(worldPicker.Selected!),
                        PickerState.CloseAdd => NewWorld($"new world {saves.Count + 1}"),
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
                        KeyPress { KeyPressed: Keys.Enter } => editor == null ? EditorStates.SelectingWorld : EditorStates.WorldEditor,
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
                        PickerState.ClosePick => EditorStates.WorldEditor,
                        PickerState.CloseEdit => EditorStates.MapEditor,
                        PickerState.CloseAdd => EditorStates.MapEditor,
                        _ => EditorStates.WorldEditorPickMap,
                    },
                    def => def
                        .AddOnEnter(() =>
                        {
                            mapPicker = new(editor!.save.MapDatas);
                            stack.Add(mapPicker);
                        })
                        .AddOnLeave(() => stack.Pop())
                ).State(
                    EditorStates.RenamingWorld,
                    c =>
                    {
                        if (c.KeyPressed == Keys.Enter)
                        {
                            if (editor != null)
                            {
                                editor.save.worldName = titleText.Text;
                                return EditorStates.WorldEditor;
                            }
                        }
                        if (c.KeyPressed == Keys.Escape)
                        {
                            return EditorStates.WorldEditor;
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
            _ => EditorStates.WorldEditor,
        };

        public void SetEditor(WorldEditor editor)
        {
            this.editor = editor;
            titleText.SetText(editor.save.worldName);
        }

        public EditorStates LoadWorld(SaveFormat save)
        {
            SetEditor(new WorldEditor(save));
            return EditorStates.WorldEditor;
        }

        public EditorStates NewWorld(string name)
        {
            SetEditor(new WorldEditor(new() { worldName = name }));
            return EditorStates.WorldEditor;
        }

        public EditorStates SetPickedMap(MapData d)
        {
            titleText.SetText($"World editor, picked [green]{d.name}[white] to place");
            return EditorStates.WorldEditor;
        }

        public EditorStates SaveWorld()
        {
            return EditorStates.WorldEditor;
        }

        private EditorStates editorhandle(Keys k)
        {
            editor?.handleInput(k);
            return EditorStates.WorldEditor;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}
