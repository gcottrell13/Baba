using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;


internal enum MainGameState
{
    Noop,

    Exit,

    PlayingGame,

    PauseMenu,

    InventoryMenu,


    // selecting which map to play in (it must have something that IS YOU)
    SelectingMap,

    // normal gameplay on the base layer
    PlayingMap,

    // normal gameplay on a map layer that should be displayed "above" the base layer
    PlayingUplayer,

    // when control is locked and we are moving between maps.
    // triggered by an event caused when something that IS YOU moves out of a map.
    MapTransition,

    // if the player number is invalid, or there is not anything that IS YOU for the given player.
    NoPlayersFound,
}