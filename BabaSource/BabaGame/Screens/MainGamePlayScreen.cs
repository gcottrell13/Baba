using Core.Screens;
using Core.Utils;
using BabaGame.Screens.GamePlay;
using Core.Engine;
using BabaGame.Engine;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace BabaGame.Screens;

internal class MainGamePlayScreen : BaseScreen<BabaGameState>
{
    private StateMachine<MainGameState, KeyPress> stateMachine;

    GameViewScreen? gameViewScreen;

    public MainGamePlayScreen(ScreenStack stack, BabaWorld worldData)
    {
        PlayerNumber playerNumber = PlayerNumber.One;

        stateMachine = new StateMachine<MainGameState, KeyPress>("main game play", MainGameState.Noop)
            .State(
                MainGameState.NoPlayersFound,
                ev => ev.KeyPressed switch
                {
                    Keys.Escape => MainGameState.Exit,
                    _ => MainGameState.Noop,
                },
                def => def
                    .AddOnEnter(() =>
                    {
                        AddChild(new NoPlayersFound());
                    })
                    .AddOnLeave(() =>
                    {
                        foreach (var child in Children.ToList()) if (child is NoPlayersFound) RemoveChild(child);
                    })
                )
            .State(
                MainGameState.PlayingGame,
                ev => gameViewScreen!.Handle(ev),
                def => def
                    .AddOnEnter(() =>
                    {
                        gameViewScreen = new GameViewScreen(stack, playerNumber, worldData);
                        stack.Add(gameViewScreen);
                        gameViewScreen.init();
                    })
                    .AddOnLeave(() => stack.Pop())
                    .SetShortCircuit(() =>
                    {
                        if (worldData.mapsWithYou(playerNumber.Name).Length == 0) return MainGameState.NoPlayersFound;
                        else return MainGameState.PlayingGame;
                    })
                );

    }

    public void init()
    {
        stateMachine.Initialize(MainGameState.PlayingGame);
    }

    public override BabaGameState Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
    {
        MainGameState.Exit => BabaGameState.MainMenu,
        MainGameState.PlayingGame => BabaGameState.None,
        _ => BabaGameState.None,
    };

    protected override void OnDispose()
    {
    }
}