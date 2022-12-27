using Core.Screens;
using Core.UI;
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
            AddChild(new Text("Welcome to the Editor"));

            SetCommands(new()
            {
                { "w", "World Editor" },
                { "m", "Map Editor" },
            });
        }
    }
}
