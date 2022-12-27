using Content.UI;
using Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class InitialScreen : BaseScreen
    {
        public InitialScreen()
        {
            Name = "Initial Screen";
            AddChild(new Text("initial screen, press w or m"));
        }
    }
}
