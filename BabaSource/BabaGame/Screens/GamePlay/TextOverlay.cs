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
    public const int MAX_CHARS_PER_ROW = 20;

    public void AddText(string text, int x, int y, float scale)
    {
        var display = new TextWithBoxOutline();
        display.SetText(text, new()
        {
            borderColor = Color.White,
            backgroundColor = Color.Black,
        });
        var (width, height) = display.TextFinalDimensions();
        display.Graphics.x = Math.Clamp(x - width * scale / 2 + 0.5f, 0, Math.Max(0, ScreenWidth - width)); 
        display.Graphics.y = Math.Clamp(y - height * scale, 0, Math.Max(0, ScreenHeight - height));
        display.Graphics.xscale = scale;
        display.Graphics.yscale = scale;
        AddChild(display);
    }

    public void RemoveAllText() 
    {
        RemoveAllChildren();
    }
}
