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
using Autofac.Core;

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
                MapPickerScreen mapPickerScreen = new(new() { 
                    "one", 
                    "two",
                    "three",
                    "four",
                    "five",
                    "six",
                    "seven",
                    "eight",
                    "nine",
                    "ten",
                });
                var mapStack = new ScreenStack(this);

                var s = new StateMachine<States, int>()
                    .State(
                        States.WorldEditor,
                        c => c switch
                        {
                            // KeyEvent { ChangedKey: Keys.Escape } => -1,
                            TextInput { Character: 'm' } => 1,
                            _ => 0,
                        },
                        def => def
                            .Change(1, States.MapPicker)
                            .AddOnEnter(() => mapStack.EnsureTop(worldEditor))
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
                            .Change(-1, States.WorldEditor)
                            .Change(1, States.ChangeObjectColor)
                            .Change(2, States.AddingTextToObject)
                            .Change(3, States.ObjectPicker)
                            .Change(4, States.MapWordLayer)
                            .Change(5, States.SelectMapRegion)
                            .AddOnEnter(() => mapStack.EnsureTop(mapEditor))
                    ).State(
                        States.MapPicker,
                        c => c switch
                        {
                            TextInput { Character: char f } => mapPickerScreen.RecieveText(f),
                            KeyEvent { ChangedKey: Keys k, Up: false } => mapPickerScreen.RecieveKey(k),
                            _ => 0,
                        },
                        def => def
                            .Change(-1, States.WorldEditor)
                            .Change(1, States.WorldEditor)
                            .Change(2, States.MapEditor)
                            .Change(3, States.MapEditor)
                            .AddOnEnter(() => mapStack.EnsureTop(mapPickerScreen))
                            .AddOnEnter(() => mapPickerScreen.SetFilter(""))
                            .AddOnEnter(() => mapPickerScreen.RecieveKey(Keys.None))
                            .AddOnLeave((state, trans) =>
                            {
                                switch (trans)
                                {
                                    case 1: { worldEditor.SetPickedMap(mapPickerScreen.Selected); break; }
                                    case 2: { mapEditor.LoadMap(mapPickerScreen.Selected); break; }
                                    case 3: { mapEditor.NewMap(); break; }
                                };
                            })
                    );

                s.Initialize(States.WorldEditor);
                CoreEventChannels.TextInput.Subscribe(ev => s.SendAction(ev));
                CoreEventChannels.KeyEvent.Subscribe(ev => s.SendAction(ev));

            }
        }

        private enum States
        {
            None,
            WorldEditor,  // editor for laying out maps in the world
            MapEditor, // editor for a section of the world; a single screen

            // In the Map Editor:
            ChangeObjectColor, // bring up a modal to select a color for a specific object
            AddingTextToObject, // bring up a modal for adding text to a specific object
            ObjectPicker, // modal for selecting a new object
            MapWordLayer, // the word layer for this specific map
            SelectMapRegion, // view the map's current region, and select/add/edit a region
            MapPicker, // select which map we are editing

            // Region Editor
            AddOrEditRegion, // add a new region, or edit an existing one
            EditRegionName,
            EditRegionWordLayer,
            SelectRegionTheme,

            // World Editor
            WorldEditorPickMap, // Choose which map to place in the world
        }
    }
}
