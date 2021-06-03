using Baba;
using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src
{
    public class Map : GameObject
    {
        private WordEngine.WordEngine? engine;

        private DateTime lastInput;

        public Map()
        {
            lastInput = DateTime.Now;
            EventChannels.MapChange.Subscribe(setMap);
            EventChannels.KeyPress.Subscribe(onKeyPress);
        }

        ~Map()
        {
            EventChannels.MapChange.Unsubscribe(setMap);
            EventChannels.KeyPress.Unsubscribe(onKeyPress);
        }

        protected override void OnBeforeUpdate(GameTime gameTime)
        {
            Graphics.xscale = World.Scale;
            Graphics.yscale = World.Scale;

            base.OnBeforeUpdate(gameTime);
        }

        void setMap(MapChange ev)
        {
            var map = AllMaps.GetMapHandle(ev.NewMapName);

            if (map == null) return;

            World.TileWidth = map.TileWidth;
            World.TileHeight = map.TileHeight;

            engine = new WordEngine.WordEngine(map.Width, map.Height);
            var (px, ph) = World.GetSizeInPixels(map.Width, map.Height);

            Graphics.x = World.Margin;
            Graphics.y = World.Margin;

            BabaGame.Game?.SetScreenSize(px, ph);

            Graphics.children.Clear();
            children.Clear();

            AddChild(new KeyListener());

            foreach (var layer in map.TileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.GlobalIdentifier != 0)
                    {
                        var name = JsonValues.Tileset[tile.GlobalIdentifier.ToString()];
                        if (name.StartsWith("text_") || name.Contains("_") == false)
                        {
                            // non-directional
                            var f = new BaseObject(name, tile.X, tile.Y);
                            AddChild(f, addGraphics: true);
                            engine.AddObject(f);
                        }
                        else
                        {
                            var parts = name.Split("_");
                            name = parts[0];
                            var direction = parts[1];

                            var f = new BaseObject(name, tile.X, tile.Y, direction);
                            AddChild(f, addGraphics: true);
                            engine.AddObject(f);
                        }
                    }
                }
            }

            foreach (var obj in engine.Objects)
            {
                if (obj.Joinable)
                {
                    var flag = 0;
                    if (obj.Y > 0 && engine.Grid[obj.X, obj.Y - 1]?.Any(o => o.Name == obj.Name) == true) flag += 2;
                    if (obj.X < map.Width - 1 && engine.Grid[obj.X + 1, obj.Y]?.Any(o => o.Name == obj.Name) == true) flag += 1;
                    if (obj.Y < map.Height - 1 && engine.Grid[obj.X, obj.Y + 1]?.Any(o => o.Name == obj.Name) == true) flag += 8;
                    if (obj.X > 0 && engine.Grid[obj.X - 1, obj.Y]?.Any(o => o.Name == obj.Name) == true) flag += 4;
                    obj.SetJoinWithNeighbors(flag.ToString());
                }
            }
        }


        void onKeyPress(KeyEvent keyEvent)
        {
            if (engine == null) return;
            if (keyEvent.Up == false) return;

            if ((DateTime.Now - lastInput) < TimeSpan.FromSeconds(World.InputDelaySeconds)) return;

            lastInput = DateTime.Now;

            if (keyEvent.Key == KeyMap.Up)
            {
                engine.TakeAction("up");
            }
            else if (keyEvent.Key == KeyMap.Down)
            {
                engine.TakeAction("down");
            }
            else if (keyEvent.Key == KeyMap.Left)
            {
                engine.TakeAction("left");
            }
            else if (keyEvent.Key == KeyMap.Right)
            {
                engine.TakeAction("right");
            }
            else if (keyEvent.Key == KeyMap.Wait)
            {
                engine.TakeAction("wait");
            }
        }

    }
}
