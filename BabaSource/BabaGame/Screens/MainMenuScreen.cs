using Core;
using Core.Screens;
using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens;

internal class MainMenuScreen : BaseScreen<BabaGameState>
{
    private ListDisplay<string> _display;
    private CallbackCollector<BabaGameState> callbackCollector = new(BabaGameState.None);
    private SpriteContainer container;

    public MainMenuScreen()
    {
        _display = new(new[] { 
            "play game",
            "exit",
        }, 10, x => x, showTitle: false, showCount: false)
        {
            OnSelect=callbackCollector.cb<string>(select),
        };
        _display.Graphics.y = 100;
        container = new SpriteContainer();
        var title = new Text("Baba [baba] game");
        AddChild(_display, false);
        AddChild(title, false);
        container.AddChild(_display.Graphics);
        container.AddChild(title.Graphics);
        SetCommands(new()
        {
            { CommonStrings.UD_ARROW, "move cursor" },
            { CommonStrings.ENTER, "select" },
        });
        Graphics.AddChild(container);

        var hi = new Text("""
            hi there, you're probably wondering why the menu is doing this. I promise, it's for a good reason. but if you can read this, then that means you have multiple monitors, so that's cool.
            
                                            [what]
                                            [arrow:2]
                                           [arrow:2][arrow:2][arrow:2]
                                          [arrow:2][arrow:2][arrow:2][arrow:2][arrow:2]
                                         [arrow:2][arrow:2][arrow:2][arrow:2][arrow:2][arrow:2][arrow:2]
            """);
        hi.Graphics.y = -ScreenHeight - 100;
        hi.Graphics.x = -500;
        container.AddChild(hi.Graphics);
        AddChild(hi, false);
    }

    protected override void OnUpdate(GameTime gameTime)
    {
        container.x = MouseState.X;
        container.y = MouseState.Y;
    }

    private BabaGameState select(string item) => item switch
    {
        "play game" => BabaGameState.PickingWorld,
        "exit" => BabaGameState.Exit,
        _ => BabaGameState.None,
    };

    public override BabaGameState Handle(KeyPress ev)
    {
        _display.Handle(ev);
        return callbackCollector.latestReturn;
    }

    protected override void OnDispose()
    {
        throw new NotImplementedException();
    }
}
