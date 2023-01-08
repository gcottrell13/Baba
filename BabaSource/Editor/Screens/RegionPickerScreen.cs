using Core.Screens;
using Editor.SaveFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
