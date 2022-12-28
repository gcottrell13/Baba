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

        public WorldEditorScreen(ScreenStack stack, List<SaveFormat> saves)
        {
            stateMachine = new StateMachine<EditorStates, KeyPress>("world editor")
                .State(
                    EditorStates.SelectingWorld,
                    c => worldPicker!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => EditorStates.WorldEditor,
                        PickerState.CloseAdd => EditorStates.WorldEditor,
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
                        KeyPress { Text: (char)0 } => EditorStates.WorldEditor,
                        KeyPress { KeyPressed: Keys k } => editorhandle(k),
                    }
                ).State(
                    EditorStates.WorldEditorPickMap,
                    c => mapPicker!.Handle(c) switch
                    {
                        PickerState.CloseCancel => EditorStates.WorldEditor,
                        PickerState.ClosePick => EditorStates.WorldEditor,
                        PickerState.CloseEdit => EditorStates.MapEditor,
                        PickerState.CloseAdd => EditorStates.MapEditor,
                        _ => EditorStates.WorldEditor,
                    },
                    def => def
                        .AddOnEnter(() =>
                        {
                            mapPicker = new(editor!.save.MapDatas);
                            stack.Add(mapPicker);
                        })
                        .AddOnLeave(() => stack.Pop())
                );
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.SelectingWorld);
            commands(EditorStates.WorldEditor);
            AddChild(new Text("World editor") { Name = "editortitle" });
        }

        private void commands(EditorStates state)
        {
            var d = new Dictionary<string, string>();
            switch (state)
            {
                case EditorStates.WorldEditor:
                    {
                        if (editor == null)
                        {
                            d.Add(CommonStrings.ENTER, "select world");
                            break;
                        }

                        d.Add("q", "zoom out");
                        d.Add("e", "zoom in");
                        d.Add(CommonStrings.ALL_ARROW, "move cursor");
                        d.Add("m", "pick map");
                        d.Add("r", "edit regions");
                        d.Add("n", "rename world");
                        d.Add(CommonStrings.ESCAPE, "select world");
                        d.Add(CommonStrings.CTRL_PLUS + "s", "save world");
                        break;
                    }
            }
            SetCommands(d);
        }

        public override EditorStates Handle(KeyPress key) => stateMachine.SendAction(key) switch
        {
            _ => EditorStates.WorldEditor,
        };

        public void SetEditor(WorldEditor editor)
        {
            this.editor = editor;
            (ChildByName("editortitle") as Text)?.SetText($"[blue]World: {editor.save.worldName}");
        }

        public void LoadWorld(SaveFormat save)
        {
            SetEditor(new WorldEditor(save));
        }

        public void NewWorld(string name)
        {
            SetEditor(new WorldEditor(new() { worldName = name }));
        }

        public void SetPickedMap(MapData d)
        {
            (ChildByName("editortitle") as Text)?.SetText($"World editor, picked [green]{d.name}[white] to place");
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
