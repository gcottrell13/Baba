using Core.Screens;
using Core.Content;
using System.Collections.Generic;

namespace Editor.Screens
{
    internal class RegionPickerScreen : FiltererModal<Region>
    {
        public RegionPickerScreen(IEnumerable<Region> regions, Region? current) : base(regions, 10, x => x.name, x => x.name, current)
        {
            Name = "Region Picker Screen";
            SetDisplayTypeName("region");
        }
    }
}
