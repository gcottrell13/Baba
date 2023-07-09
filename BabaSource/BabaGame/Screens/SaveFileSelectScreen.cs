using Autofac.Features.OwnedInstances;
using Core.Engine;
using Core.Screens;
using Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens;

internal class SaveFileSelectScreen : BaseScreen<BabaGameState>
{
    private readonly FiltererModal<WorldData> filtererModal;
    private readonly Campaign saveFile;
    private readonly Action<WorldData> onSelect;
    private readonly Action onNew;

    public SaveFileSelectScreen(Campaign saveFile, Action<WorldData> onSelect, Action onNew)
    {
        filtererModal = new(saveFile.SaveFiles.Values.Append(new()).ToList(), 10, display: displayWorldData, canCancel: false)
        {
            OnSelect = OnSelect,
        };
        filtererModal.SetDisplayTypeName("Save File");
        AddChild(filtererModal);
        this.saveFile = saveFile;
        this.onSelect = onSelect;
        this.onNew = onNew;
        SetCommands(BasicMenu);
    }

    private void OnSelect(WorldData data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            onNew();
        }
        else
        {
            onSelect(data);
        }
    }

    private string displayWorldData(WorldData wd)
    {
        if (string.IsNullOrWhiteSpace(wd.Name))
        {
            return "New Save";
        }
        var percentComplete = wd.Screens.Count(map => map.visited) * 100 / wd.Screens.Count;
        return $"Save {wd.Name} - {percentComplete}%";
    }

    public override BabaGameState Handle(KeyPress ev) => filtererModal.Handle(ev) switch
    {
        PickerState.ClosePick => BabaGameState.PlayingGame,
        PickerState.CloseCancel => BabaGameState.MainMenu,
        _ => BabaGameState.None,
    };

    protected override void OnDispose()
    {
    }
}
