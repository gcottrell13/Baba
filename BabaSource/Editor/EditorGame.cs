using Core;
using Core.UI;
using Core.Bootstrap;
using Core.Screens;
using Core.Utils;
using Editor.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Content;
using Autofac.Core;
using Editor.SaveFormats;
using System.Linq;

namespace Editor
{
    public class EditorGame : GameSetup
    {
        public EditorGame() : base(new EditorGameEntryPoint())
        {
            MAX_WIDTH = 1080;
            MAX_HEIGHT = 720;
        }

        protected override void Initialize()
        {
            ContentLoader.LoadContent(GraphicsDevice);
            base.Initialize();
        }

        private class EditorGameEntryPoint : GameEntryPoint
        {
            public override void Initialize()
            {
                var saveFiles = LoadSaveFiles.LoadAllWorlds();

                MapEditorScreen? mapEditorScreen = null;
                WorldEditorScreen? worldEditorScreen = null;

                var mapStack = new ScreenStack();

                var s = new StateMachine<EditorStates, KeyPress>("game")
                    .State(
                        EditorStates.WorldEditor,
                        c => worldEditorScreen!.Handle(c),
                        def => def
                            .AddOnLeave(() => mapStack.Pop()?.Dispose())
                            .AddOnEnter(() =>
                            {
                                worldEditorScreen = new(mapStack, saveFiles);
                                mapStack.Add(worldEditorScreen);
                                worldEditorScreen.init();
                            })
                    ).State(
                        EditorStates.MapEditor,
                        c => mapEditorScreen!.Handle(c),
                        def => def
                            .AddOnLeave(() => mapStack.Pop()?.Dispose())
                            .AddOnEnter(() =>
                            {
                                mapEditorScreen = new(mapStack, Editor.EDITOR.currentMap!);
                                mapStack.Add(mapEditorScreen);
                                mapEditorScreen.init();
                            })
                    );

                s.Initialize(EditorStates.WorldEditor);
                onKeyPress(ev => s.SendAction(ev));

                AddChild(mapStack);
            }
        }

    }
}
