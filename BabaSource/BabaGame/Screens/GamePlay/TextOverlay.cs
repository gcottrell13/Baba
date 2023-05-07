using Core;
using Core.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

internal class TextOverlay : GameObject
{
    public void AddText(string text, int x, int y, float scale)
    {
        var display = new TextWithBoxOutline();
        display.SetText(text, new()
        {
            borderColor = Color.White,
            backgroundColor = Color.Black,
        });
        var (width, height) = display.TextFinalDimensions();
        display.Graphics.x = x - width * scale / 2; 
        display.Graphics.y = y - height * scale;
        display.Graphics.xscale = scale;
        display.Graphics.yscale = scale;
        AddChild(display);
    }

    public void RemoveAllText() 
    {
        RemoveAllChildren();
    }
}
