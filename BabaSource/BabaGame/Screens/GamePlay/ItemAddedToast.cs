using Core;
using Core.Content;
using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

public class ItemAddedToast : GameObject
{

    public const double DECAY_TIME = 0.5f;
    public const double STAY_TIME = 1f;

    private const double TOTAL_TIME = DECAY_TIME + STAY_TIME;

    private double age = 0f;

    public ItemAddedToast(ObjectTypeId obj, int added, int newTotal)
    {
        var text = new TextWithBoxOutline();
        var objColor = ColorExtensions.ToHexTriple(ThemeInfo.GetObjectColor("default", obj.ToString()));
        text.SetText($"{objColor}[{obj}][white] +{added} [gray]({newTotal})", new() { backgroundColor = Color.Black });
        AddChild(text);
    }

    protected override void OnUpdate(GameTime gameTime)
    {
        age += gameTime.ElapsedGameTime.TotalSeconds;
        if (age > STAY_TIME)
        {
            Graphics.alpha = (float)(1 - (age - STAY_TIME) / DECAY_TIME);
        }
        else if (age > TOTAL_TIME)
        {
            Graphics.alpha = 0f;
            Parent?.RemoveChild(this);
        }
    }
}
