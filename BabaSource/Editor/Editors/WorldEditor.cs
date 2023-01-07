using Core.Content;
using Core.Utils;
using Editor.SaveFormats;
using Editor.Screens;
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
        private MapData? pickedMap;
        public ObjectData cursor { get; private set; } = new() { name = "cursor", color = ThemeInfo.ColorNameMap["rosy"] };

        public WorldEditor(SaveFormat save) 
        {
            this.save = save;
            
        }

        public EditorStates handleInput(Keys key, bool isSpaceDown = false) => key switch
        {
            Keys.Up => cursorUp(isSpaceDown),
            Keys.Down => cursorDown(isSpaceDown),
            Keys.Left => cursorLeft(isSpaceDown),
            Keys.Right => cursorRight(isSpaceDown),
            //Keys.Space => TryPlaceObject() ? EditorStates.None : EditorStates.None,
            //Keys.Delete => TryDeleteObjectAtCursor() ? EditorStates.None : EditorStates.None,
            _ => EditorStates.None,
        };


        private EditorStates cursorUp(bool isSpaceDown)
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y - 1, (int)save.height);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorDown(bool isSpaceDown)
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y + 1, (int)save.height);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorLeft(bool isSpaceDown)
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x - 1, (int)save.width);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorRight(bool isSpaceDown)
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x + 1, (int)save.width);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }

        public void saveWorld()
        {
            LoadSaveFiles.SaveAll(save);
        }

        public void setPickedMap(MapData? map)
        {
            pickedMap = map;
        }
    }
}
