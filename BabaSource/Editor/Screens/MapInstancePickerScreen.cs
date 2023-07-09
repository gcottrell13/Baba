using Core.Engine;
using Core.Screens;
using Editor.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens;

internal class MapInstancePickerScreen : FiltererModal<SaveScreenInstance>
{

    public MapInstancePickerScreen(
        IEnumerable<SaveScreenInstance> items, 
        int maxDisplay,
        SaveFormatWorld world,
        Func<SaveScreenInstance, bool> isSelected, // we can select multiple
        SaveScreenInstance? currentValue = null,
        bool canCancel = true) 
        : base(items, maxDisplay, 
            display: map(world, isSelected), 
            filterBy: map(world, isSelected),
            currentValue: currentValue, 
            canCancel: canCancel)
    {
    }

    private static Func<SaveScreenInstance, string> map(SaveFormatWorld world, Func<SaveScreenInstance, bool> isSelected) 
        => (SaveScreenInstance instance) 
        => (isSelected(instance) ? "[check] " : "" ) + (world.SaveScreenDataByInstanceId(instance.instanceId)?.name ?? "none");
}
