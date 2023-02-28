using Core.Engine;
using Core.Screens;
using Core.UI;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens.GamePlay;

internal class PlayerSelectScreen : BaseScreen<MainGameState>
{
    private readonly ListDisplay<string> filtererModal;
    private CallbackCollector<MainGameState> callbackCollector = new(MainGameState.Noop);

    public PlayerSelectScreen(Action<string> onSelect)
    {
        filtererModal = new(new[]
        {
            "one",
            "two",
        }, 2, display, showCount: false)
        {
            OnSelect = callbackCollector.cb(select(onSelect)),
        };
        filtererModal.SetDisplayTypeName("Choose your player");
        AddChild(filtererModal);
        SetCommands(BasicMenu);
    }

    private string display(string item) => item switch
    {
        "one" => "Player [text_you]",
        "two" => "player [text_you2]",
        _ => "?",
    };

    private Func<string, MainGameState> select(Action<string> onSelect) => s =>
    {
        onSelect(s);
        return MainGameState.PlayingGame;
    };

    public override MainGameState Handle(KeyPress ev) {
        if (ev.KeyPressed == Microsoft.Xna.Framework.Input.Keys.Escape) return MainGameState.Exit;
        filtererModal.Handle(ev);
        return callbackCollector.latestReturn;
    }

    protected override void OnDispose()
    {
    }
}
