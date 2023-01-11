using Core.Content;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MusicPickerScreen : FiltererModal<string>
    {
        public MusicPickerScreen(string? current = null) : base(PlaySound.musicNames, 10, x => x, currentValue: current)
        {
            Name = "MusicPicker";
        }
    }
}
