using Core;
using Core.Utils;
using Editor.SaveFormats;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Editor.Editors
{
    internal class MapLayerEditor : GameObject
    {

        private Stack<Action> undoActions = new(capacity: 20);
        private readonly int pxWidth;
        private readonly int pxHeight;

        private uint cursorX = 0;
        private uint cursorY = 0;

        private string? currentObject;

        private MapLayer? layer;

        public MapLayerEditor(int maxPxWidth, int maxPxHeight)
        {
            pxWidth = maxPxWidth;
            pxHeight = maxPxHeight;
        }

        public void Undo()
        {

        }

        public void LoadMap(MapLayer? map)
        {
            layer = map;
            undoActions.Clear();
        }

        public void Save()
        {
            Editor.EDITOR.SaveAll();
        }

        public void handleInput(Keys key)
        {

        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var scale = 1f;
            Graphics.xscale = scale;
            Graphics.yscale = scale;


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
            var obj = Editor.ObjectAtPosition(cursorX, cursorY, layer);
            if (obj == null) return false;
            obj.color = colorName;
            return true;
        }

        public bool TryPlaceObject()
        {
            var obj = Editor.ObjectAtPosition(cursorX, cursorY, layer);
            if (currentObject == null || obj == null) return false;

            obj.original = obj.name;
            obj.name = currentObject;
            return true;
        }

        public bool TrySetCurrentLayerDimensions(string size)
        {
            if (layer == null) return false;
            if (!size.TryRowColToInt(out var dims)) return false;

            layer.width = (uint)dims.X;
            layer.height = (uint)dims.Y;
            return true;
        }
    }
}
