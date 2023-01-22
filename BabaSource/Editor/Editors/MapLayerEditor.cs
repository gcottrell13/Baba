using Core;
using Editor.Saves;
using Core.Utils;
using Editor.Screens;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Core.Content;

namespace Editor.Editors
{
    internal class MapLayerEditor
    {

        private Stack<Action> undoActions = new(capacity: 20);
        public readonly SaveObjectData cursor = new() { name = "cursor", color = ThemeInfo.ColorNameMap["rosy"] };

        public SaveObjectData? currentObject { get; private set; }

        private SaveMapLayer mapLayer;

        public MapLayerEditor(SaveMapLayer map)
        {
            mapLayer = map;
        }

        public void Undo()
        {
            if (undoActions.Count > 0)
                undoActions.Pop()();
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
            cursor.y = MathExtra.MathMod((int)cursor.y - 1, (int)mapLayer.height);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorDown(bool isSpaceDown)
        {
            cursor.y = MathExtra.MathMod((int)cursor.y + 1, (int)mapLayer.height);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorLeft(bool isSpaceDown)
        {
            cursor.x = MathExtra.MathMod((int)cursor.x - 1, (int)mapLayer.width);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }
        private EditorStates cursorRight(bool isSpaceDown)
        {
            cursor.x = MathExtra.MathMod((int)cursor.x + 1, (int)mapLayer.width);
            if (isSpaceDown) { TryPlaceObject(); }
            return EditorStates.None;
        }

        public SaveObjectData? ObjectAtCursor() => ObjectAtPosition(cursor.x, cursor.y);

        public SaveObjectData? ObjectAtPosition(int x, int y) => Editor.ObjectAtPosition(x, y, mapLayer);

        public SaveObjectData SetSelectedObject(string name, bool withUndo = true)
        {
            if (withUndo && currentObject != null)
            {
                var oldSelectedObject = currentObject;
                undoActions.Push(() => SetSelectedObject(oldSelectedObject));
            }
            var d = new SaveObjectData() { name = name, color = ObjectInfo.Info[name].color_active };
            SetSelectedObject(d);
            return d;
        }

        private void SetSelectedObject(SaveObjectData? selectedObject)
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
            SetSelectedObject(new SaveObjectData() { 
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
                undoActions.Push(() => trySetObjectAtPosition(oldObject, oldObject.x, oldObject.y, false));
            }
            return tryDeleteObjectAtPosition(cursor.x, cursor.y);
        }

        private bool tryDeleteObjectAtPosition(int x, int y)
        {
            var removed = mapLayer.objects.RemoveAll(item => item.x == x && item.y == y);
            return removed > 0;
        }

        public bool TrySetObjectColor(int color, bool withUndo = true)
        {
            var obj = ObjectAtCursor();
            if (obj == null) return false;

            if (withUndo)
            {
                var oldColor = obj.color;
                undoActions.Push(() => trySetColorAtPosition(oldColor, obj.x, obj.y));
            }
            return trySetColorAtPosition(color, obj.x, obj.y);
        }

        private bool trySetColorAtPosition(int color, int x, int y)
        {
            var obj = ObjectAtPosition(x, y);
            if (obj == null) return false;
            obj.color = color;
            return true;
        }

        public bool TryPlaceObject(bool withUndo = true)
        {
            if (currentObject == null) return false;

            var obj = ObjectAtCursor();

            if (withUndo)
            {
                if (obj != null)
                {
                    var oldObject = obj.copy();
                    undoActions.Push(() => trySetObjectAtPosition(oldObject, oldObject.x, oldObject.y, false));
                }
                else
                {
                    var x = cursor.x;
                    var y = cursor.y;
                    undoActions.Push(() => tryDeleteObjectAtPosition(x, y));
                }
            }

            return trySetObjectAtPosition(currentObject, cursor.x, cursor.y);
        }

        private bool trySetObjectAtPosition(SaveObjectData d, int x, int y, bool keepOriginal = true)
        {
            var obj = ObjectAtPosition(x, y);

            if (obj == null)
            {
                obj = new SaveObjectData()
                {
                    name = d.name,
                    x = x,
                    y = y,
                    color = d.color,
                    state = d.state,
                };
                mapLayer.objects.Add(obj);
            }
            else
            {
                if (keepOriginal)
                {
                    obj.original = new() { name = obj.name, color = obj.color, state = obj.state };
                }
                else
                {
                    obj.original = d.original;
                }

                obj.name = d.name;
                obj.color = d.color;
                obj.state = d.state;
            }

            return true;
        }

        public bool TrySetCurrentLayerDimensions(string size)
        {
            if (!size.TryRowColToInt(out var dims)) return false;

            mapLayer.width = (int)dims.X;
            mapLayer.height = (int)dims.Y;
            return true;
        }

        public bool TryRotateObjectAtCursor(bool ccw = true, bool withUndo = true)
        {
            var obj = ObjectAtCursor();
            if (obj == null) return false;

            if (withUndo)
            {
                undoActions.Push(() => tryRotateObjectAtPosition(obj.x, obj.y, !ccw));
            }
            return tryRotateObjectAtPosition(obj.x, obj.y, ccw);
        }

        public bool tryRotateObjectAtPosition(int x, int y, bool ccw = true)
        {
            var obj = ObjectAtPosition(x, y);
            if (obj == null) return false;

            if (ContentLoader.LoadedContent?.SpriteValues[obj.name] is FacingOnMove)
            {
                if (ccw)
                {
                    obj.state = (Direction)obj.state switch
                    {
                        Direction.Up => (uint)Direction.Right,
                        Direction.Right => (uint)Direction.Down,
                        Direction.Down => (uint)Direction.Left,
                        Direction.Left => (uint)Direction.Up,
                        _ => (uint)Direction.Down,
                    };
                }
                else
                {
                    obj.state = (Direction)obj.state switch
                    {
                        Direction.Up => (uint)Direction.Left,
                        Direction.Right => (uint)Direction.Up,
                        Direction.Down => (uint)Direction.Right,
                        Direction.Left => (uint)Direction.Down,
                        _ => (uint)Direction.Up,
                    };
                }
            }

            return true;
        }
    }
}
