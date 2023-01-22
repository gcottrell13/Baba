using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class NumberPickerScreen : FiltererModal<string>
    {
        private static IEnumerable<string> range(int start, int end)
        {
            for (var i = start; i <= end; i++) yield return i.ToString();
        }

        public NumberPickerScreen(int lo, int hi, int? current = null) : base(range(lo, hi), 15, display: x => x.ToString(), currentValue: current?.ToString())
        {

        }
    }
}
