using Core.Engine;
using Core.Screens;
using Editor.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens;

internal class MapInstancePickerScreen : FiltererModal<SaveMapInstance>
{

    public MapInstancePickerScreen(
        IEnumerable<SaveMapInstance> items, 
        int maxDisplay,
        SaveFormatWorld world,
        Func<SaveMapInstance, bool> isSelected, // we can select multiple
        SaveMapInstance? currentValue = null,
        bool canCancel = true) 
        : base(items, maxDisplay, 
            display: map(world, isSelected), 
            filterBy: map(world, isSelected),
            currentValue: currentValue, 
            canCancel: canCancel)
    {
    }

    private static Func<SaveMapInstance, string> map(SaveFormatWorld world, Func<SaveMapInstance, bool> isSelected) 
        => (SaveMapInstance instance) 
        => (isSelected(instance) ? "[check] " : "" ) + (world.SaveMapDataByInstanceId(instance.instanceId)?.name ?? "none");
}
