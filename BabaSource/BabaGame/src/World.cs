using Baba;
using BabaGame.src.Engine;
using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src
{
    public class World : GameObject
    {
        private WorldStructure data;

        private DateTime lastInput;

        private bool allowInput = false;

        private Dictionary<(int, int), GameObject> MapDisplays;

        private List<Keys> AllKeysPressed;

        public World(string world)
        {
            MapDisplays = new Dictionary<(int, int), GameObject>();

            lastInput = DateTime.Today;
            data = new WorldStructure(world);
            EventChannels.MapChange.Subscribe(setMap);
            EventChannels.KeyPress.Subscribe(onKeyPress);
            EventChannels.CharacterControl.Subscribe(onCharacterControl);

            var (px, ph) = WorldVariables.GetSizeInPixels(WorldStructure.MapWidth, WorldStructure.MapHeight);
            BabaGame.Game?.SetScreenSize(px, ph);

            AllKeysPressed = new List<Keys>();
        }

        ~World()
        {
            EventChannels.MapChange.Unsubscribe(setMap);
            EventChannels.KeyPress.Unsubscribe(onKeyPress);
            EventChannels.CharacterControl.Unsubscribe(onCharacterControl);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Graphics.xscale = WorldVariables.Scale;
            Graphics.yscale = WorldVariables.Scale;

            if (allowInput && AllKeysPressed?.Count > 0)
            {
                if (DateTime.Now - lastInput >= TimeSpan.FromSeconds(WorldVariables.InputDelaySeconds + WorldVariables.MoveAnimationSeconds))
                {
                    lastInput = DateTime.Now;

                    if (AllKeysPressed.Contains(KeyMap.Up))
                    {
                        data.TakeAction("up");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Down))
                    {
                        data.TakeAction("down");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Left))
                    {
                        data.TakeAction("left");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Right))
                    {
                        data.TakeAction("right");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Wait))
                    {
                        data.TakeAction("wait");
                    }
                }

            }

            base.OnUpdate(gameTime);
        }

        void onCharacterControl(CharacterControl characterControl)
        {
            allowInput = characterControl.Enable;
        }

        void setMap(MapChange ev)
        {
            if (ev.Direction != null)
            {
                EventChannels.CharacterControl.SendAsyncMessage(new CharacterControl { Enable = true });
            }

            Graphics.x = WorldVariables.Margin;
            Graphics.y = WorldVariables.Margin;


            Graphics.children.Clear();
            children.Clear();

            foreach (var obj in data.SetMap(ev))
            {
                AddChild(obj, addGraphics: true);
            }
        }

        void onKeyPress(KeyEvent keyEvent)
        {
            if (data == null) return;
            if (!keyEvent.Up && AllKeysPressed.Contains(keyEvent.ChangedKey) == false)
            {
                AllKeysPressed.Add(keyEvent.ChangedKey);
            }
            else if (keyEvent.Up && AllKeysPressed.Contains(keyEvent.ChangedKey))
            {
                AllKeysPressed.Remove(keyEvent.ChangedKey);
            }
        }

    }
}
