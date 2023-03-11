using Core.Screens;
using Core.Utils;
using BabaGame.Screens.GamePlay;
using Core.Engine;
using BabaGame.Engine;
using Microsoft.Xna.Framework.Input;

namespace BabaGame.Screens;

internal class MainGamePlayScreen : BaseScreen<BabaGameState>
{
    private StateMachine<MainGameState, KeyPress> stateMachine;

    PlayerSelectScreen? playerSelect;

    GameViewScreen? gameViewScreen;

    public MainGamePlayScreen(ScreenStack stack, BabaWorld worldData)
    {
        PlayerNumber? playerNumber = null;

        stateMachine = new StateMachine<MainGameState, KeyPress>("main game play", MainGameState.Noop)
            .State(
                MainGameState.PlayerSelect,
                ev => playerSelect!.Handle(ev),
                def => def
                    .AddOnEnter(() => {
                        playerSelect = new(s => {
                            playerNumber = s;
                        });
                        stack.Add(playerSelect);
                    })
                    .AddOnLeave(() => stack.Pop())
                )
            .State(
                MainGameState.NoPlayersFound,
                ev => ev.KeyPressed switch
                {
                    Keys.Escape => MainGameState.Exit,
                    Keys.Back => MainGameState.Exit,
                    _ => MainGameState.Noop,
                },
                def => def
                    .AddOnEnter(() =>
                    {
                        AddChild(new NoPlayersFound());
                    })
                )
            .State(
                MainGameState.PlayingGame,
                ev => MainGameState.Noop,
                def => def
                    .AddOnEnter(() =>
                    {
                        gameViewScreen = new(stack, playerNumber, worldData);
                        stack.Add(gameViewScreen);
                        gameViewScreen.init();
                    })
                    .AddOnLeave(() => stack.Pop())
                    .SetShortCircuit(() =>
                    {
                        if (worldData.mapsWithYou().Length == 0) return MainGameState.NoPlayersFound;
                        else return MainGameState.PlayingGame;
                    })
                );

    }

    public void init()
    {
        stateMachine.Initialize(MainGameState.PlayerSelect);
    }

    public override BabaGameState Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
    {
        MainGameState.Exit => BabaGameState.MainMenu,
        _ => BabaGameState.None,
    };

    protected override void OnDispose()
    {
    }
}