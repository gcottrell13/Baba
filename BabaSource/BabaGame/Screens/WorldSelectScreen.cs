using Core.Engine;
using Core.Screens;
using Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.Screens;

internal class WorldSelectScreen : BaseScreen<BabaGameState>
{
    private readonly FiltererModal<SaveFile> filtererModal;

    public WorldSelectScreen(List<SaveFile> saveFiles, SaveFile? current, Action<SaveFile> onSelect)
    {
        filtererModal = new(saveFiles, 10, displaySaveFile, currentValue: current)
        {
            OnSelect = onSelect,
        };
        filtererModal.SetDisplayTypeName("World");
        AddChild(filtererModal);
    }

    private string displaySaveFile(SaveFile saveFile) => saveFile.InitialContent.Name;

    public override BabaGameState Handle(KeyPress ev) => filtererModal.Handle(ev) switch
    {
        PickerState.ClosePick => BabaGameState.PickingSaveFile,
        PickerState.CloseCancel => BabaGameState.Exit,
        _ => BabaGameState.None,
    };

    protected override void OnDispose()
    {
    }
}
