using Baba.src.Events;
using BabaGame.src;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baba
{
    class GameEntryPoint : GameObject
    {
        public static GameEntryPoint ROOT = new GameEntryPoint();

        new public void AddChild(GameObject gameObject, bool addGraphics = false)
        {
            base.AddChild(gameObject, addGraphics);
            if (addGraphics)
            {
                SpriteContainer.ROOT.AddChild(gameObject.Graphics);
            }
        }

        public void EntryTick(GameTime gameTime)
        {
            ROOT.Tick(gameTime);
        }

        public void Initialize(string connectTo)
        {
            var map = AllMaps.GetMapHandle("start");
            ROOT.Graphics.xscale = World.Scale;
            ROOT.Graphics.yscale = World.Scale;


            ROOT.AddChild(new KeyListener());

            var me = new BaseObject("me", 11, 11);
            ROOT.AddChild(me, addGraphics: true);

            void onKeyPress(KeyEvent keyEvent)
            {
                if (keyEvent.Up == false) return;
                if (keyEvent.Key == KeyMap.Up)
                {
                    me.SetY(me.Y - 1);
                }
                else if (keyEvent.Key == KeyMap.Down)
                {
                    me.SetY(me.Y + 1);
                }
                else if (keyEvent.Key == KeyMap.Left)
                {
                    me.SetX(me.X - 1);
                }
                else if (keyEvent.Key == KeyMap.Right)
                {
                    me.SetX(me.X + 1);
                }
            }

            EventChannels.KeyPress.Subscribe(onKeyPress);

            for (var i = 0; i < 10; i++) 
                ROOT.AddChild(new BaseObject("baba", i, i), addGraphics: true);

            //var map = new WaitingRoom();
            //ROOT.AddChild(map);

            //ROOT.AddChild(new KeyListener());
            //ROOT.AddChild(new Character(600, 1300, src.Maps.MapBase.EntityScale, map.Graphics, color: Color.Red)
            //{
            //    CharacterRadius = 15f,
            //});
            //ROOT.AddChild(new Assembler(705, 1274, src.Maps.MapBase.EntityScale, map.Graphics));
            //ROOT.AddChild(new Tent(map.Graphics));
        }

    }

}
