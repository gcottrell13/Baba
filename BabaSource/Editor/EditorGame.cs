using Core.Bootstrap;
using Core.Screens;
using Core.Utils;
using Editor.Screens;
using Core.Content;
using Editor.Saves;

namespace Editor
{
    public class EditorGame : GameSetup
    {
        public EditorGame() : base(new EditorGameEntryPoint())
        {
            MAX_WIDTH = 1080;
            MAX_HEIGHT = 720;
        }

        private class EditorGameEntryPoint : GameEntryPoint
        {
            public override void Initialize()
            {
                var saveFiles = LoadSaveFiles.LoadAllWorlds();

                var mapStack = new ScreenStack();
                PlaySound.PlayMusic("editorsong");

                WorldEditorScreen worldEditorScreen = new(mapStack, saveFiles);

                var s = new StateMachine<EditorStates, KeyPress>("game", EditorStates.None)
                    .State(
                        EditorStates.WorldEditor,
                        c => worldEditorScreen.Handle(c),
                        def => def
                            .AddOnLeave(() => mapStack.Pop()?.Dispose())
                            .AddOnEnter(() =>
                            {
                                worldEditorScreen = new(mapStack, saveFiles);
                                mapStack.Add(worldEditorScreen);
                                worldEditorScreen.init();
                            })
                    );

                s.Initialize(EditorStates.WorldEditor);
                onKeyPress(ev => s.SendAction(ev));

                AddChild(mapStack);
            }
        }

    }
}
