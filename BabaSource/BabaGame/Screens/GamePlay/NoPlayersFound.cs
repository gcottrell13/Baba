using Core;
using Core.Screens;
using Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

internal class NoPlayersFound : GameObject
{
    public NoPlayersFound()
    {
        AddChild(new Text("where would we be without [text_you]?\nnowhere, apparently.\n\nNo valid playable objects were found.\nPress [text_escape] to go back."));
    }
}
