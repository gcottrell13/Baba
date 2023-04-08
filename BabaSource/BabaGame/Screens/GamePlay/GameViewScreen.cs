using BabaGame.Engine;
using Core.Content;
using Core.Engine;
using Core.Screens;
using Core.Utils;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

/// <summary>
/// when the playing field is in view
/// </summary>
internal class GameViewScreen : BaseScreen<MainGameState>
{
    private StateMachine<MainGameState, KeyPress> stateMachine;
    private readonly PlayerNumber playerNumber;
    private readonly BabaWorld worldData;
    private readonly MapViewWindow mapViewWindow;

    public GameViewScreen(ScreenStack stack, PlayerNumber playerNumber, BabaWorld worldData)
    {
        this.playerNumber = playerNumber;
        this.worldData = worldData;
        mapViewWindow = new(worldData);
        short currentMapId = 0;
        AddChild(mapViewWindow);

        stateMachine = new StateMachine<MainGameState, KeyPress>("game view screen", MainGameState.Noop)
            .State(
                MainGameState.SelectingMap,
                ev => MainGameState.Noop,
                def => def
                    .SetShortCircuit(() => worldData.mapsWithYou(playerNumber.Name).Length switch
                    {
                        0 => MainGameState.NoPlayersFound,
                        1 => MainGameState.PlayingMap,
                        _ => MainGameState.Noop,
                    })
                )
            .State(
                MainGameState.PlayingUplayer,
                ev => MainGameState.Noop
                )
            .State(
                MainGameState.PlayingMap,
                handlePlayingMap,
                def => def
                    .AddOnEnter(() => { 
                        if (currentMapId == 0)
                        {
                            currentMapId = worldData.mapsWithYou(playerNumber.Name).First();
                        }
                        mapViewWindow.LoadMap(currentMapId);
                        AddChild(mapViewWindow);
                    })
                    .AddOnLeave(() => { })
                )
            .State(
                MainGameState.MapTransition,
                ev => MainGameState.Noop);
    }

    public void init()
    {
        stateMachine.Initialize(MainGameState.SelectingMap);
    }

    public override MainGameState Handle(KeyPress ev)
    {
        throw new NotImplementedException();
    }

    private MainGameState handlePlayingMap(KeyPress ev)
    {
        worldData.Step(0, Array.Empty<short>(), Direction.None, playerNumber.Name);
        return MainGameState.Noop;
    }

    protected override void OnDispose()
    {
    }
}
