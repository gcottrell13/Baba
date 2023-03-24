using Core.Content;
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
    private readonly ListDisplay<PlayerNumber> filtererModal;
    private CallbackCollector<MainGameState> callbackCollector = new(MainGameState.Noop);

    public PlayerSelectScreen(Action<PlayerNumber> onSelect)
    {
        filtererModal = new(new[]
        {
            PlayerNumber.One,
            PlayerNumber.Two,
        }, 2, display, showCount: false)
        {
            OnSelect = callbackCollector.cb(select(onSelect)),
        };
        filtererModal.SetDisplayTypeName("Choose your player");
        AddChild(filtererModal);
        SetCommands(BasicMenu);
    }

    private string display(PlayerNumber item) => item.Name switch
    {
        ObjectTypeId.you => "Player [text_you]",
        ObjectTypeId.you2 => "player [text_you2]",
        _ => "?",
    };

    private Func<PlayerNumber, MainGameState> select(Action<PlayerNumber> onSelect) => s =>
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


internal class PlayerNumber
{

    public static PlayerNumber One = new(ObjectTypeId.you);
    public static PlayerNumber Two = new(ObjectTypeId.you2);

    public ObjectTypeId Name { get; }
        
    public PlayerNumber(ObjectTypeId name)
    {
        Name = name;
    }
}