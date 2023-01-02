using Core.Screens;
using Editor.SaveFormats;

namespace Editor.Screens
{
    internal class WorldPickerScreen : FiltererModal<SaveFormat>
    {
        public WorldPickerScreen(ReadonlySavesList items) : base(items, 15, x => x.worldName, x => $"world: {x.worldName}")
        {
            Name = "WorldPicker";
            Add = true;
            maxDisplay = 15;
            SetDisplayTypeName("world");
        }
    }
}
