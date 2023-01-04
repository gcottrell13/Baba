using Core;
using Core.Content;
using Core.Utils;
using Editor.SaveFormats;
using Editor.Screens;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Editor.Editors
{
    internal class MapLayerEditor
    {

        private Stack<Action> undoActions = new(capacity: 20);
        public readonly ObjectData cursor = new() { name = "cursor", color = PaletteInfo.ColorNameMap["pink"] };

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

        public EditorStates handleInput(Keys key, bool isSpaceDown) => key switch
        {
            Keys.Up => cursorUp(isSpaceDown),
            Keys.Down => cursorDown(isSpaceDown),
            Keys.Left => cursorLeft(isSpaceDown),
            Keys.Right => cursorRight(isSpaceDown),
            Keys.Space => TryPlaceObject() ? EditorStates.None : EditorStates.None,
            _ => EditorStates.None,
        };

        private EditorStates cursorUp(bool isSpaceDown)
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y - 1, (int)mapLayer.height);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorDown(bool isSpaceDown)
        {
            cursor.y = (uint)MathExtra.MathMod((int)cursor.y + 1, (int)mapLayer.height);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorLeft(bool isSpaceDown)
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x - 1, (int)mapLayer.width);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorRight(bool isSpaceDown)
        {
            cursor.x = (uint)MathExtra.MathMod((int)cursor.x + 1, (int)mapLayer.width);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }

        public ObjectData? ObjectAtCursor()
        {
            return Editor.ObjectAtPosition(cursor.x, cursor.y, mapLayer);
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

        public bool TrySetObjectColor(int color)
        {
            var obj = ObjectAtCursor();
            if (obj == null) return false;
            obj.color = color;
            return true;
        }

        public bool TryPlaceObject()
        {
            if (currentObject == null) return false;

            var obj = ObjectAtCursor();

            if (obj == null)
            {
                obj = new ObjectData() { 
                    name = currentObject,
                    x = cursor.x, 
                    y = cursor.y, 
                    color = ObjectInfo.Info[currentObject].color_active,
                };
                mapLayer.objects.Add(obj);
            }
            else
            {
                obj.original = obj.name;
                obj.name = currentObject;
                obj.color = ObjectInfo.Info[currentObject].color_active;
            }
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
