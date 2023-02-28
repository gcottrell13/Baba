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
    private readonly SaveFile saveFile;
    private readonly Action<WorldData> onSelect;

    public SaveFileSelectScreen(SaveFile saveFile, Action<WorldData> onSelect)
	{
        filtererModal = new(saveFile.SaveFiles.Values.Append(new()).ToList(), 10, display: displayWorldData, canCancel: false)
        {
            OnSelect = OnSelect,
        };
        filtererModal.SetDisplayTypeName("Save File");
        AddChild(filtererModal);
        this.saveFile = saveFile;
        this.onSelect = onSelect;
	}

    private void OnSelect(WorldData data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            var newSave = WorldData.Deserialize(saveFile.InitialContent.Serialize());
            newSave.Name = $"Save {(char)(saveFile.SaveFiles.Count + 'a')}";
            onSelect(newSave);
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
        return wd.Name;
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
