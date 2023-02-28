using Core.Screens;
using Core.Utils;
using BabaGame.Screens.GamePlay;

namespace BabaGame.Screens;

internal class MainGamePlayScreen : BaseScreen<BabaGameState>
{
    private readonly ScreenStack stack;
    private StateMachine<MainGameState, KeyPress> stateMachine;

    PlayerSelectScreen? playerSelect; 

    public MainGamePlayScreen(ScreenStack stack)
    {
        this.stack = stack;
        stateMachine = new StateMachine<MainGameState, KeyPress>("main game play", MainGameState.Noop)
            .State(
                MainGameState.PlayerSelect,
                ev => playerSelect!.Handle(ev),
                def => def
                    .AddOnEnter(() => {
                        playerSelect = new(s => { });
                        stack.Add(playerSelect);
                    })
                    .AddOnLeave(() => stack.Pop())
                );

    }

    public void init()
    {
        stateMachine.Initialize(MainGameState.PlayerSelect);
    }

    public override BabaGameState Handle(KeyPress ev) => stateMachine.SendAction(ev) switch
    {
        MainGameState.Exit => BabaGameState.MainMenu,
        _ => BabaGameState.None,
    };

    protected override void OnDispose()
    {
    }
}