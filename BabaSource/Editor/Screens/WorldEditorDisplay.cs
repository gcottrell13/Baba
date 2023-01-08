using Core;
using Core.Content;
using Core.UI;
using Core.Utils;
using Editor.SaveFormats;
using Editor.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Screens
{
    internal class WorldEditorDisplay : GameObject
    {
        private readonly SaveFormat world;
        private readonly ObjectData cursor;
        private readonly float gridWidth;
        private int zoom = 4;

        private Container mapsContainer = new();
        private Container cursorContainer = new();
        private Text gridDisplay = new();
        private Text columnNumberDisplay = new();
        private Text rowNumberDisplay = new();

        public WorldEditorDisplay(SaveFormat world, ObjectData cursor, float gridWidth)
        {
            this.world = world;
            this.cursor = cursor;
            this.gridWidth = gridWidth;
            AddChild(gridDisplay);
            AddChild(mapsContainer);
            AddChild(cursorContainer);
            AddChild(columnNumberDisplay);
            AddChild(rowNumberDisplay);

            cursorContainer.AddChild(new Text(ThemeInfo.MakeObjectString("default", cursor.name, cursor.color)));
        }

        public void ZoomIn()
        {
            zoom = Math.Max(3, zoom - 1);
        }

        public void ZoomOut()
        {
            zoom = Math.Min(7, zoom + 1);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var mapPxWidth = gridWidth / zoom;
            var mapPxHeight = (18f / 24) * mapPxWidth;

            var topX = (int)Math.Clamp(cursor.x - (int)(zoom / 2), 0, world.width - zoom);
            var topY = (int)Math.Clamp(cursor.y - (int)(zoom / 2), 0, world.height - zoom);

            foreach (var instance in world.WorldLayout)
            {
                var name = $"map {instance.x} {instance.y}";
                var c = mapsContainer.ChildByName(name);

                if (instance.x < topX || instance.x >= topX + zoom || instance.y < topY || instance.y >= topY + zoom)
                {
                    mapsContainer.RemoveChild(c);
                    continue;
                }

                var mapData = world.MapDatas.FirstOrDefault(x => x.id == instance.mapDataId);

                if (mapData == null)
                {
                    mapsContainer.RemoveChild(c);
                    continue;
                }

                var mld = c as MapLayerDisplay;

                if (mld == null || mld.mapLayer != mapData.layer1)
                {
                    mapsContainer.RemoveChild(c);
                    mld = new MapLayerDisplay(mapData.name, mapData.layer1, Editor.EDITOR.GetRegionTheme(mapData.regionId), false) { Name = name };
                    mapsContainer.AddChild(mld);
                }

                mld.theme = Editor.EDITOR.GetRegionTheme(mapData.regionId) ?? "default";
                mld.Graphics.x = (instance.x - topX) * mapPxWidth;
                mld.Graphics.y = (instance.y - topY) * mapPxHeight;
                mld.Graphics.xscale = mapPxWidth / (float)(mapData.layer1.width * 24);
                mld.Graphics.yscale = mapPxHeight / (float)(mapData.layer1.height * 24);
            }

            var maxYMagnitude = (int)Math.Log10(world.height + 1) + 1;
            var maxXMagnitude = (int)Math.Log(world.width, 26) + 1;

            var columnHeaderLines = GridHelpers.GetColumnHeaders((uint)topX, (uint)(topX + zoom), (uint)maxXMagnitude);
            var rowHeaderLines = GridHelpers.GetRowHeaders((uint)topY, (uint)(topY + zoom), (uint)maxYMagnitude);

            columnNumberDisplay.SetText(string.Join("\n", columnHeaderLines), new() { blockWidth = (int)mapPxWidth });
            rowNumberDisplay.SetText(string.Join("\n", rowHeaderLines), new() { lineHeight = (int)mapPxHeight });

            var grid = string.Join("\n", new[] { "[editorsquare]".Repeat(zoom) }.Repeat(zoom));
            gridDisplay.SetText(grid);
            gridDisplay.Graphics.xscale = mapPxWidth / 24;
            gridDisplay.Graphics.yscale = mapPxHeight / 24;

            gridDisplay.Graphics.x = (gridDisplay.CurrentOptions!.padding + gridDisplay.CurrentOptions.blockWidth) * (maxYMagnitude + 1);
            gridDisplay.Graphics.y = (gridDisplay.CurrentOptions!.padding + gridDisplay.CurrentOptions.lineHeight) * (maxXMagnitude + 1);

            columnNumberDisplay.Graphics.x = gridDisplay.Graphics.x;
            rowNumberDisplay.Graphics.y = gridDisplay.Graphics.y;

            if (cursorContainer.Children.FirstOrDefault() is Text cursorSprite)
            {
                cursorSprite.SetText(ThemeInfo.MakeObjectString("default", cursor.name, cursor.color));
                cursorSprite.Graphics.x = (cursor.x - topX) * 24 * 2 + 12;
                cursorSprite.Graphics.y = (cursor.y - topY) * 24 * 2 + 12;
                cursorContainer.Graphics.xscale = gridDisplay.Graphics.xscale / 2;
                cursorContainer.Graphics.yscale = gridDisplay.Graphics.yscale / 2;
                cursorContainer.Graphics.x = gridDisplay.Graphics.x;
                cursorContainer.Graphics.y = gridDisplay.Graphics.y;
            }

            mapsContainer.Graphics.x = gridDisplay.Graphics.x;
            mapsContainer.Graphics.y = gridDisplay.Graphics.y;
        }

        private class Container : GameObject { }
    }
}
