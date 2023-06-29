using BabaGame.Engine;
using BabaGame.Events;
using BabaGame.Screens;
using Core.Bootstrap;
using Core.Content;
using Core.Engine;
using Core.Screens;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace BabaGame;

public class BabaGame : GameSetup
{
    private Effect? blurEffect;

    public BabaGame() : base(new BabaGameEntryPoint())
    {
        MAX_WIDTH = 1024;
        MAX_HEIGHT = 1024;

    }

    protected override void LoadContent()
    {
        blurEffect = Content.Load<Effect>("blur");
        base.LoadContent();
    }

    private class BabaGameEntryPoint : GameEntryPoint
    {
        public override void Initialize()
        {
            base.Initialize();

            SaveFile? saveFile = null;
            WorldData? selectedWorld = null;

            void select(WorldData wd)
            {
                selectedWorld = wd;
            }

            var saveFiles = LoadGameSaveFiles.LoadAllCompiledMaps();

            WorldSelectScreen? worldSelectScreen = null;
            SaveFileSelectScreen? saveFileSelectScreen = null;
            MainMenuScreen? mainMenuScreen = null;
            MainGamePlayScreen? mainGamePlayScreen = null;
            BabaWorld? babaWorld = null;

            PlaySound.PlayMusic("menu");

            var stack = new ScreenStack();

            var stateMachine = new StateMachine<BabaGameState, KeyPress>("babaGame", BabaGameState.None)
                .State(
                    BabaGameState.PickingSaveFile,
                    @event => saveFileSelectScreen!.Handle(@event),
                    def => def
                        .AddOnEnter(() =>
                        {
                            saveFile ??= saveFiles.Values.First();
                            saveFileSelectScreen = new(saveFile, select, () =>
                            {
                                var newSave = WorldData.Deserialize(saveFile.InitialContent.Serialize());
                                var ver = ((char)(saveFile.SaveFiles.Count + 'a')).ToString();
                                newSave.Name = ver;
                                LoadGameSaveFiles.SaveCompiledMap(newSave, saveFile.Name, ver);
                                select(newSave);
                            });
                            stack.Add(saveFileSelectScreen);
                        })
                        .AddOnLeave(() => stack.Pop())
                )
                .State(
                    BabaGameState.PickingWorld,
                    @event => worldSelectScreen!.Handle(@event),
                    def => def
                        .AddOnEnter(() =>
                        {
                            worldSelectScreen = new(saveFiles.Values.ToList(), saveFile, s => {
                                saveFile = s;
                            });
                            stack.Add(worldSelectScreen);
                        })
                        .AddOnLeave(() => stack.Pop())
                        .SetShortCircuit(() => saveFiles.Count == 1 ? BabaGameState.PickingSaveFile : BabaGameState.None)
                )
                .State(
                    BabaGameState.PlayingGame,
                    @event => mainGamePlayScreen!.Handle(@event),
                    def => def
                        .AddOnEnter(() =>
                        {
                            babaWorld = new BabaWorld(selectedWorld!);
                            mainGamePlayScreen = new(stack, babaWorld);
                            stack.Add(mainGamePlayScreen);
                            mainGamePlayScreen.init();
                        })
                        .AddOnLeave(() => stack.Pop())
                )
                .State(
                    BabaGameState.MainMenu,
                    ev => mainMenuScreen!.Handle(ev),
                    def => def
                        .AddOnEnter(() =>
                        {
                            mainMenuScreen = new();
                            stack.Add(mainMenuScreen);
                        })
                        .AddOnLeave(() => stack.Pop())
                )
                .State(BabaGameState.Exit, e => BabaGameState.None, def => def.AddOnEnter(() => Exit()));

            stateMachine.Initialize(BabaGameState.MainMenu);

            onKeyPress(ev => stateMachine.SendAction(ev));
            AddChild(stack);

            void saveGame(int i)
            {
                var wd = babaWorld?.ToWorldData();
                LoadGameSaveFiles.SaveCompiledMap(wd, saveFile.Name, wd.Name);
            }
            EventChannels.SaveGame.Subscribe(saveGame);
        }
    }
}

internal enum BabaGameState
{
    None,
    Exit,

    MainMenu,
    SettingsMenu,

    PickingWorld,
    PickingSaveFile,
    PlayingGame,


}