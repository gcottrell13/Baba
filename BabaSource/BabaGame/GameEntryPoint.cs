using BabaGame.src.Events;
using BabaGame.src;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using BabaGame.src.WordEngine;
using Core;
using Core.Configuration;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame
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
            AllMaps.LoadMaps();
            ObjectSprites.LoadTextures();

            AddChild(new Map(), true);

            EventChannels.MapChange.SendAsyncMessage(new MapChange { NewMapName = "start" });
        }

    }

}
