﻿using Core;
using Core.Content;
using Core.UI;
using Core.Utils;
using Editor.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Saves;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class MapLayerEditorDisplay : GameObject
    {
        private readonly SaveMapLayer mapLayer;
        private readonly SaveObjectData? cursor;
        public string theme;
        private Text objectsDisplay = new();
        private Text gridDisplay = new();
        private Text cursorDisplay = new();
        private Text columnNumberDisplay = new();
        private Text rowNumberDisplay = new();
        private SaveObjectData? itemAtCursor;
        private RectangleSprite background = new() { xscale = ScreenWidth, yscale = ScreenHeight };

        public MapLayerEditorDisplay(string name, SaveMapLayer mapLayer, SaveObjectData? cursor, string? theme)
        {
            this.mapLayer = mapLayer;
            this.cursor = cursor;
            this.theme = theme ?? "default";
            Name = name;

            Graphics.AddChild(background);
            background.SetColor(ThemeInfo.GetThemeBackgroundColor(theme ?? "default"));

            AddChild(new Text(name));

            AddChild(gridDisplay);
            AddChild(objectsDisplay);
            AddChild(cursorDisplay);
            AddChild(columnNumberDisplay);
            AddChild(rowNumberDisplay);

        }

        public void SetSelectedObject(SaveObjectData? displayItem)
        {
            itemAtCursor = displayItem;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var objects = new Dictionary<int, Dictionary<int, string>>();

            foreach (var obj in mapLayer.objects)
            {
                var c = PaletteInfo.Palettes[theme][obj.color];
                objects.ConstructDefaultValue(obj.x)[obj.y] = $"{c.ToHexTriple()}[{obj.name}:{obj.state}]";
            }

            var maxYMagnitude = (int)Math.Log10(mapLayer.height + 1) + 1;
            var maxXMagnitude = (int)Math.Log(mapLayer.width, 26) + 1;

            var lines = new List<string>();
            var columnHeaderLines = GridHelpers.GetColumnHeaders(0, mapLayer.width);
            var rowHeaderLines = GridHelpers.GetRowHeaders(0, mapLayer.height);

            for (int y = 0; y < mapLayer.height; y++)
            {
                var line = new List<string>();

                for (int x = 0; x < mapLayer.width; x++)
                {
                    line.Add(objects.ConstructDefaultValue(x).TryGetValue(y, out var obj) ? obj : " ");
                }
                lines.Add(string.Join("", line));
            }

            gridDisplay.SetText(string.Join("\n", new[] { "[editorsquare]".Repeat((int)mapLayer.width) }.Repeat((int)mapLayer.height)));
            objectsDisplay.SetText(string.Join("\n", lines));
            columnNumberDisplay.SetText(string.Join("\n", columnHeaderLines));
            rowNumberDisplay.SetText(string.Join("\n", rowHeaderLines));

            objectsDisplay.Graphics.x = (objectsDisplay.CurrentOptions!.padding + objectsDisplay.CurrentOptions.blockWidth) * (maxYMagnitude + 1);
            objectsDisplay.Graphics.y = (objectsDisplay.CurrentOptions!.padding + objectsDisplay.CurrentOptions.lineHeight) * (maxXMagnitude + 1) + 25;

            columnNumberDisplay.Graphics.x = objectsDisplay.Graphics.x;
            columnNumberDisplay.Graphics.y = 25;
            rowNumberDisplay.Graphics.y = objectsDisplay.Graphics.y;
            rowNumberDisplay.Graphics.x = 0;

            gridDisplay.Graphics.x = objectsDisplay.Graphics.x;
            gridDisplay.Graphics.y = objectsDisplay.Graphics.y;

            if (cursor != null && cursor.x < mapLayer.width && cursor.y < mapLayer.height)
            {
                cursorDisplay.SetText("\n".Repeat(cursor.y) + " ".Repeat(cursor.x) + ThemeInfo.MakeObjectString("default", cursor.name, cursor.color));
                cursorDisplay.Graphics.x = objectsDisplay.Graphics.x;
                cursorDisplay.Graphics.y = objectsDisplay.Graphics.y;
            }

            if (itemAtCursor != null && cursorDisplay.Graphics.children.FirstOrDefault() is SpriteContainer cursorSprite)
            {
                Text? disp = (Text?)ChildByName("currentItemDisplay");
                if (disp == null)
                {
                    disp = new Text() { Name = "currentItemDisplay" };
                    disp.Graphics.alpha = 0.5f;
                    AddChild(disp);
                }
                var color = PaletteInfo.Palettes["default"][itemAtCursor.color].ToHexTriple();
                disp.SetText($"{color}[{itemAtCursor.name}:{itemAtCursor.state}]");
                disp.Graphics.x = cursorSprite.x + cursorDisplay.Graphics.x;
                disp.Graphics.y = cursorSprite.y + cursorDisplay.Graphics.y;
            }
            else
            {
                RemoveChild(ChildByName("currentItemDisplay"));
            }

            if (ThemeInfo.GetThemeBackgroundColor(theme) is Color bgColor)
            {
                background.SetColor(bgColor);
            }
            
            var yscale = 20f / (mapLayer.height + 3);
            var xscale = 36f / (mapLayer.width + 3);
            var scale = Math.Min(xscale, yscale);
            Graphics.xscale = scale;
            Graphics.yscale = scale;
        }
    }
}
