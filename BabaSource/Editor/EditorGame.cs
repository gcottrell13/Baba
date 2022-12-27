using Core;
using Core.UI;
using Core.Bootstrap;
using Core.Events;
using Core.Screens;
using Core.Utils;
using Editor.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Content;

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
                MapEditorScreen mapEditor = new();
                WorldEditorScreen worldEditor = new();
                var mapStack = new ScreenStack(this);

                var s = new StateMachine<States, int>()
                    .State(
                        States.Initial, 
                        c => c switch
                        {
                            TextInput { Character: 'w' } => 1,
                            TextInput { Character: 'm' } => 2,
                            _ => 0,
                        },
                        def => def
                            .Change(1, States.WorldEditor)
                            .Change(2, States.MapEditor)
                            .AddOnEnter(() =>
                            {
                                mapStack.PopTo(null);
                                mapStack.Add(new InitialScreen());
                            })
                    ).State(
                        States.WorldEditor,
                        c => c switch
                        {
                            KeyEvent { ChangedKey: Keys.Escape } => -1,
                            _ => 0,
                        },
                        def => def
                            .AddOnEnter(() =>
                            {
                                mapStack.PopTo(worldEditor);
                                mapStack.Pop();
                                worldEditor = new();
                                mapStack.Add(worldEditor);
                            })
                            .Change(-1, States.Initial)
                    ).State(
                        States.MapEditor,
                        c => c switch
                        {
                            TextInput { Character: 'c' } => 1,
                            TextInput { Character: 't' } => 2,
                            TextInput { Character: 'p' } => 3,
                            TextInput { Character: 'l' } => 4,
                            TextInput { Character: 'r' } => 5,
                            KeyEvent { ChangedKey: Keys.S, Up: false } => mapEditor.TrySavingMap(),
                            TextInput { Character: char f } => 0,
                            KeyEvent { ChangedKey: Keys.Escape } => -1,
                            _ => 0,
                        },
                        def => def
                            .Change(-1, States.Initial)
                            .Change(1, States.ChangeObjectColor)
                            .Change(2, States.AddingTextToObject)
                            .Change(3, States.ObjectPicker)
                            .Change(4, States.MapWordLayer)
                            .Change(5, States.SelectMapRegion)
                            .AddOnEnter(() =>
                            {
                                mapStack.PopTo(mapEditor);
                                mapStack.Pop();
                                mapEditor = new();
                                mapStack.Add(mapEditor);
                            })
                    );

                s.Initialize(States.Initial);
                CoreEventChannels.TextInput.Subscribe(ev => s.SendAction(ev));
                CoreEventChannels.KeyEvent.Subscribe(ev => s.SendAction(ev));

            }
        }

        private enum States
        {
            None,
            Initial,
            WorldEditor,  // editor for laying out maps in the world
            MapEditor, // editor for a section of the world; a single screen

            // In the Map Editor:
            ChangeObjectColor, // bring up a modal to select a color for a specific object
            AddingTextToObject, // bring up a modal for adding text to a specific object
            ObjectPicker, // modal for selecting a new object
            MapWordLayer, // the word layer for this specific map
            SelectMapRegion, // view the map's current region, and select/add/edit a region

            // Region Editor
            AddOrEditRegion, // add a new region, or edit an existing one
            EditRegionName,
            EditRegionWordLayer,
            SelectRegionTheme,

            // World Editor
            WorldEditorPickMap,
        }
    }
}
