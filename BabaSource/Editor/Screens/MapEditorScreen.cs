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
        private RenameScreen? renameScreen;

        private TextInputBox titleText = new(format: "[90,ff,90]Map Editor: {}") { Name = "editortitle" };

        public MapEditorScreen(ScreenStack stack, MapData mapData)
        {
            editor = new(mapData);
            layerEditorScreen = new(stack, mapData.layer1);
            AddChild(new MapLayerDisplay(mapData.layer1, null));

            stateMachine = new StateMachine<EditorStates, KeyPress>("map editor")
                .State(
                    EditorStates.MapEditor,
                    c => c switch
                    {
                        KeyPress { KeyPressed: Keys.Escape } => EditorStates.WorldEditor,
                        KeyPress { Text: '1' } => editLayer1(stack),
                        KeyPress { Text: '2' } => editLayer2(stack),
                        KeyPress { Text: 'r' } => selectMapRegion(),
                        KeyPress { KeyPressed: Keys.S, ModifierKeys: ModifierKeys.Ctrl } => SaveMap(),
                        _ => EditorStates.MapEditor,
                    }
                ).State(
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
                ).State(
                    EditorStates.RenamingMap,
                    c => renameScreen!.Handle(c) switch
                    {
                        RenameScreen.RenameStates.Cancel => EditorStates.MapEditor,
                        RenameScreen.RenameStates.Save => EditorStates.MapEditor,
                        _ => EditorStates.RenamingMap,
                    },
                    def => def
                        .AddOnLeave(() =>
                        {
                            editor.mapData.name = renameScreen!.Text;
                            titleText.SetText(renameScreen!.Text);
                            stack.Pop().Dispose();
                        })
                        .AddOnEnter(() =>
                        {
                            renameScreen = new(editor.mapData.name, "name map: {}");
                            stack.Add(renameScreen);
                        })
                );
        }

        public void init()
        {
            stateMachine.Initialize(EditorStates.MapEditor);
            editorCommands();
            AddChild(titleText);
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
            _ => EditorStates.MapEditor,
        };

        private EditorStates editLayer1(ScreenStack stack)
        {
            RemoveChild(layerEditorScreen);
            layerEditorScreen.Dispose();
            layerEditorScreen = new(stack, Editor.EDITOR.currentMap!.layer1);
            return EditorStates.EditMapLayer;
        }

        private EditorStates editLayer2(ScreenStack stack)
        {
            RemoveChild(layerEditorScreen);
            layerEditorScreen.Dispose();
            layerEditorScreen = new(stack, Editor.EDITOR.currentMap!.layer2);
            return EditorStates.EditMapLayer;
        }

        private EditorStates selectMapRegion()
        {
            return EditorStates.SelectMapRegion;
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
