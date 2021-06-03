using Baba;
using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src
{
    public class Map : GameObject
    {
        private WordEngine.WordEngine? engine;

        public Map()
        {
            EventChannels.MapChange.Subscribe(setMap);
            
            AddChild(new KeyListener());
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

            for (var i = 0; i < 10; i++)
            {
                var f = new BaseObject("me", i, i, "right");
                AddChild(f, addGraphics: true);
                engine.AddObject(f);
            }


            for (var i = 0; i < 10; i++)
            {
                var f = new BaseObject("wall", i, i);
                AddChild(f, addGraphics: true);
                engine.AddObject(f);
            }


            var me = new BaseObject("baba", 11, 11);
            AddChild(me, addGraphics: true);
            engine.AddObject(me);

            var rock = new BaseObject("rock", 15, 10);
            AddChild(rock, addGraphics: true);
            engine.AddObject(rock);
        }


        void onKeyPress(KeyEvent keyEvent)
        {
            if (engine == null) return;
            if (keyEvent.Up == false) return;
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
