using BabaGame.Engine;
using BabaGame.Events;
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
    private short currentMapId = 0;

    public GameViewScreen(ScreenStack stack, PlayerNumber playerNumber, BabaWorld worldData)
    {
        this.playerNumber = playerNumber;
        this.worldData = worldData;
        mapViewWindow = new MapViewWindow(worldData, ScreenWidth, ScreenHeight);
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
                ev => handlePlayingMap(ev, currentMapId),
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
        EventChannels.MapChange.Subscribe(mapChange);
        EventChannels.SoundPlay.Subscribe(playSound);
    }

    private async Task playSound(MusicPlay sound)
    {
        await PlaySound.PlaySoundFile(sound.TrackName!);
    }

    private void mapChange(MapChange e)
    {
        currentMapId = e.MapId;
        mapViewWindow.LoadMap(e.MapId);
    }

    public void init()
    {
        stateMachine.Initialize(MainGameState.SelectingMap);
    }

    public override MainGameState Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
    {
        MainGameState.PlayingGame => MainGameState.Noop,
        _ => MainGameState.Noop,
    };

    private MainGameState handlePlayingMap(KeyPress ev, short currentMapId)
    {
        Direction? dir = ev.KeyPressed switch
        {
            Keys.Up => Direction.Up,
            Keys.Down => Direction.Down,
            Keys.Left => Direction.Left,
            Keys.Right => Direction.Right,
            Keys.Space => Direction.None,
            Keys.W => Direction.Up,
            Keys.S => Direction.Down,
            Keys.A => Direction.Left,
            Keys.D => Direction.Right,
            _ => null,
        };

        if (dir is Direction d)
        {
            var mapIds = mapViewWindow.GetVisibleMaps();
            worldData.Step(currentMapId, mapIds, d, playerNumber.Name);
            mapViewWindow.OnMove(mapIds);
        }
        return MainGameState.Noop;
    }

    protected override void OnDispose()
    {
        EventChannels.MapChange.Unsubscribe(mapChange);
        EventChannels.SoundPlay.Unsubscribe(playSound);
    }
}
