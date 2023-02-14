using Core.Content;
using Core.Utils;
using Editor.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Editor.Saves;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Editors
{
    internal class WorldEditor
    {
        public SaveFormatWorld save { get; private set; }
        private SaveMapData? pickedMap;
        public SaveObjectData cursor { get; private set; } = new() { name = "cursor", color = ThemeInfo.ColorNameMap["rosy"] };

        public Vector2 warpPoint1;

        public WorldEditor(SaveFormatWorld save) 
        {
            this.save = save;
            
        }

        public EditorStates handleInput(Keys key, bool isSpaceDown = false) => key switch
        {
            Keys.Up => cursorUp(isSpaceDown),
            Keys.Down => cursorDown(isSpaceDown),
            Keys.Left => cursorLeft(isSpaceDown),
            Keys.Right => cursorRight(isSpaceDown),
            Keys.Space => TryPlaceMapAtCursor() ? EditorStates.None : EditorStates.None,
            Keys.Delete => TryDeleteObjectAtCursor() ? EditorStates.None : EditorStates.None,
            _ => EditorStates.None,
        };


        private EditorStates cursorUp(bool isSpaceDown)
        {
            cursor.y = MathExtra.MathMod((int)cursor.y - 1, (int)save.height);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorDown(bool isSpaceDown)
        {
            cursor.y = MathExtra.MathMod((int)cursor.y + 1, (int)save.height);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorLeft(bool isSpaceDown)
        {
            cursor.x = MathExtra.MathMod((int)cursor.x - 1, (int)save.width);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorRight(bool isSpaceDown)
        {
            cursor.x = MathExtra.MathMod((int)cursor.x + 1, (int)save.width);
            //if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }

        public bool TryAddWarpAtCursor()
        {
            save.Warps.Add(new() { x1 = (int)warpPoint1.X, y1 = (int)warpPoint1.Y, x2 = cursor.x, y2 = cursor.y });
            return true;
        }

        public bool TryDeleteWarpAtCursor()
        {
            var x = cursor.x;
            var y = cursor.y;
            var warp = save.Warps.FirstOrDefault(w => (w.x1 == x && w.y1 == y) || (w.x2 == x && w.y2 == y));
            if (warp == null) return false;
            return save.Warps.Remove(warp);
        }

        public bool TryDeleteObjectAtCursor()
        {
            var m = MapAtCursor();
            if (m == null) return false;

            return save.WorldLayout.Remove(m);
        }

        public SaveMapInstance? MapAtCursor() => save.WorldLayout.FirstOrDefault(x => x.x == cursor.x && x.y == cursor.y);

        public bool TryPlaceMapAtCursor()
        {
            if (pickedMap == null) return false;
            var m = MapAtCursor();
            if (m == null)
            {
                m = new SaveMapInstance { x = cursor.x, y = cursor.y };
                save.WorldLayout.Add(m);
            }
            m.mapDataId = pickedMap.id;
            return true;
        }

        public void saveWorld()
        {
            LoadSaveFiles.SaveAll(save);
            
            var compiled = CompileMap.CompileWorld(save);
            LoadSaveFiles.SaveCompiledMap(compiled, save.fileName ?? "compiled");
        }

        public void setPickedMap(SaveMapData? map)
        {
            pickedMap = map;
        }
    }
}
