using BabaGame.src.Events;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src
{
    public class KeyListener : GameObject
    {
        private Keys[] pressedKeys = Array.Empty<Keys>();

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var key in pressedKeys.Union(KeyboardState.GetPressedKeys()))
            {
                var isDown = KeyboardState.IsKeyDown(key);
                var wasDown = pressedKeys.Contains(key);
                if (isDown && wasDown == false)
                {
                    EventChannels.KeyPress.SendMessage(new KeyEvent { ChangedKey = key, Up = false});
                }
                else if (isDown == false && wasDown)
                {
                    EventChannels.KeyPress.SendMessage(new KeyEvent { ChangedKey = key, Up = true});
                }
            }
            pressedKeys = KeyboardState.GetPressedKeys();
        }
    }
}
