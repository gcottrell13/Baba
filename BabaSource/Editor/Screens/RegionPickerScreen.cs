using Core.Screens;
using Editor.Saves;
using System.Collections.Generic;

namespace Editor.Screens
{
    internal class RegionPickerScreen : FiltererModal<SaveRegion>
    {
        public RegionPickerScreen(IEnumerable<SaveRegion> regions, SaveRegion? current) : base(regions, 10, x => x.name, x => x.name, current)
        {
            Name = "Region Picker Screen";
            SetDisplayTypeName("region");
        }
    }
}
