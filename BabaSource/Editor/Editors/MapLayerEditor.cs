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

        public ObjectData? currentObject { get; private set; }

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
            Keys.Delete => TryDeleteObjectAtCursor() ? EditorStates.None : EditorStates.None,
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

        public ObjectData SetSelectedObject(string name, bool withUndo = true)
        {
            if (withUndo && currentObject != null)
            {
                var oldSelectedObject = currentObject;
                undoActions.Push(() => SetSelectedObject(oldSelectedObject));
            }
            var d = new ObjectData() { name = name, color = ObjectInfo.Info[name].color_active };
            SetSelectedObject(d);
            return d;
        }

        private void SetSelectedObject(ObjectData? selectedObject)
        {
            currentObject = selectedObject;
        }

        public bool TryCopyObjectAtCursor(bool withUndo = true)
        {
            var atCursor = ObjectAtCursor();
            if (atCursor == null) return false;
            if (withUndo && currentObject != null)
            {
                var oldSelectedObject = currentObject;
                undoActions.Push(() => SetSelectedObject(oldSelectedObject));
            }
            SetSelectedObject(new ObjectData() { 
                name = atCursor.name, 
                color = atCursor.color,
                state = atCursor.state,
                original = atCursor.original,
            });
            return true;
        }

        public bool TryDeleteObjectAtCursor(bool withUndo = true)
        {
            var atCursor = ObjectAtCursor();
            if (atCursor == null) return false;
            if (withUndo)
            {
                var oldObject = atCursor;
                undoActions.Push(() => TryPlaceObjectAtPosition(oldObject, oldObject.x, oldObject.y));
            }
            var removed = mapLayer.objects.RemoveAll(x => x.x == atCursor.x && x.y == atCursor.y);
            return removed > 0;
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
                return TryPlaceObjectAtPosition(currentObject, cursor.x, cursor.y);
            }

            obj.original = obj.name;
            obj.name = currentObject.name;
            obj.color = currentObject.color;
            obj.state = currentObject.state;
            return true;
        }

        private bool TryPlaceObjectAtPosition(ObjectData d, uint x, uint y)
        {
            var obj = ObjectAtCursor();
            if (obj != null) return false;

            obj = new ObjectData()
            {
                name = d.name,
                x = cursor.x,
                y = cursor.y,
                color = d.color,
                state = d.state,
            };
            mapLayer.objects.Add(obj);
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
