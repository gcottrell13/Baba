using Core.Screens;
using Editor.Saves;

namespace Editor.Screens
{
    internal class WorldPickerScreen : FiltererModal<SaveFormatWorld>
    {
        public WorldPickerScreen(ReadonlySavesList items) : base(items, 15, x => x.worldName, x => $"world: {x.worldName}")
        {
            Name = "WorldPicker";
            maxDisplay = 15;
            SetDisplayTypeName("world");
        }
    }
}
