using Editor.SaveFormats;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Editors
{
    internal class WorldEditor
    {
        public SaveFormat save { get; private set; }

        public WorldEditor(SaveFormat save) 
        {
            this.save = save;
            
        }

        public void handleInput(Keys key)
        {

        }
    }
}
