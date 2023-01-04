using Core;
using Core.Utils;
using Editor.SaveFormats;
using Editor.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Core.GameObject;

namespace Editor.Editors
{
    internal class MapLayerEditor
    {

        private Stack<Action> undoActions = new(capacity: 20);
        public readonly ObjectData cursor = new() { name = "cursor", color = "pink" };

        private string? currentObject;

        private MapLayer mapLayer;

        public MapLayerEditor(MapLayer map)
        {
            mapLayer = map;
        }

        public void Undo()
        {

        }

        public void Save()
        {
            Editor.EDITOR.SaveAll();
        }

        public EditorStates handleInput(Keys key) => key switch
        {
            Keys.Up => cursorUp(),
            Keys.Down => cursorDown(),
            Keys.Left => cursorLeft(),
            Keys.Right => cursorRight(),
            Keys.Space => TryPlaceObject() ? EditorStates.None : EditorStates.None,
            _ => EditorStates.None,
        };

        private EditorStates cursorUp()
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y - 1, (int)mapLayer.height);
            return EditorStates.None;
        }
        private EditorStates cursorDown()
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y + 1, (int)mapLayer.height);
            return EditorStates.None;
        }
        private EditorStates cursorLeft()
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x - 1, (int)mapLayer.width);
            return EditorStates.None;
        }
        private EditorStates cursorRight()
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x + 1, (int)mapLayer.width);
            return EditorStates.None;
        }

        public ObjectData? ObjectAtCursor()
        {
            return Editor.ObjectAtPosition(cursor.y, cursor.y, mapLayer);
        }

        public void SetSelectedObject(string name, bool withUndo = true)
        {
            if (withUndo && currentObject != null)
            {
                var oldSelectedObject = currentObject;
                undoActions.Push(() => SetSelectedObject(oldSelectedObject, withUndo: false));
            }

            currentObject = name;
        }

        public bool TrySetObjectColor(string colorName)
        {
            var obj = ObjectAtCursor();
            if (obj == null) return false;
            obj.color = colorName;
            return true;
        }

        public bool TryPlaceObject()
        {
            var obj = ObjectAtCursor();
            if (currentObject == null || obj == null) return false;

            obj.original = obj.name;
            obj.name = currentObject;
            return true;
        }

        public bool TrySetCurrentLayerDimensions(string size)
        {
            if (!size.TryRowColToInt(out var dims)) return false;

            mapLayer.width = (uint)dims.X;
            mapLayer.height = (uint)dims.Y;
            return true;
        }
    }
}
