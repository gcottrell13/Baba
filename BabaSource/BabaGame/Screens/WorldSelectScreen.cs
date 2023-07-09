using Core.Engine;
using Core.Screens;
using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens;

internal class WorldSelectScreen : BaseScreen<BabaGameState>
{
    private readonly ListDisplay<Campaign> filtererModal;

    private CallbackCollector<PickerState> callbackCollector = new(PickerState.Open);

    public WorldSelectScreen(List<Campaign> saveFiles, Campaign? current, Action<Campaign> onSelect)
    {
        filtererModal = new(saveFiles, 10, display: displaySaveFile, currentValue: current)
        {
            OnSelect = callbackCollector.cb(select(onSelect)),
        };
        filtererModal.SetDisplayTypeName("World");
        AddChild(filtererModal);
        SetCommands(BasicMenu);
    }

    private Func<Campaign, PickerState> select(Action<Campaign> saveFile) => save =>
    {
        saveFile(save);
        return PickerState.ClosePick;
    };

    private string displaySaveFile(Campaign saveFile) => saveFile.InitialContent.Name;

    public override BabaGameState Handle(KeyPress ev)
    {
        if (ev.KeyPressed == Keys.Escape) return BabaGameState.Exit;
        filtererModal.Handle(ev);
        if (callbackCollector.latestReturn == PickerState.ClosePick) return BabaGameState.PickingSaveFile;
        return BabaGameState.None;
    }

    protected override void OnDispose()
    {
    }
}
