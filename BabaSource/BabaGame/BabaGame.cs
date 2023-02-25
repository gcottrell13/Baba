using BabaGame.Events;
using BabaGame.Screens;
using Core.Bootstrap;
using Core.Engine;
using Core.Screens;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace BabaGame;

public class BabaGame : GameSetup
{
    public BabaGame() : base(new BabaGameEntryPoint())
    {
        MAX_WIDTH = 1080;
        MAX_HEIGHT = 720;
    }

    private class BabaGameEntryPoint : GameEntryPoint
    {
        public override void Initialize()
        {
            base.Initialize();

            SaveFile? saveFile = null;

            var saveFiles = LoadGameSaveFiles.LoadAllCompiledMaps();

            WorldSelectScreen? worldSelectScreen = null;
            SaveFileSelectScreen? saveFileSelectScreen = null;

            var stack = new ScreenStack();

            var stateMachine = new StateMachine<BabaGameState, KeyPress>("babaGame", BabaGameState.None)
                .State(BabaGameState.PickingSaveFile,
                    @event => saveFileSelectScreen!.Handle(@event),
                    def => def
                        .AddOnEnter(() =>
                        {
                            saveFile ??= saveFiles.Values.First();
                            saveFileSelectScreen = new(saveFile, wd => {
                                saveFile!.SetSave(wd);
                                LoadGameSaveFiles.SaveCompiledMap(wd, saveFile.Name, saveFile.SaveFiles.Count.ToString());
                            });
                            stack.Add(saveFileSelectScreen);
                        })
                        .AddOnLeave(() => stack.Pop())
                )
                .State(BabaGameState.PickingWorld,
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
                .State(BabaGameState.PlayingGame,
                    @event => BabaGameState.None,
                    def => def
                        .AddOnEnter(() =>
                        {

                        })
                )
                .State(BabaGameState.Exit, e => BabaGameState.None, def => def.AddOnEnter(() => Exit()));

            stateMachine.Initialize(BabaGameState.PickingWorld);

            onKeyPress(ev => stateMachine.SendAction(ev));
            AddChild(stack);
        }
    }
}

internal enum BabaGameState
{
    None,
    Exit,

    PickingWorld,
    PickingSaveFile,
    PlayingGame,


}