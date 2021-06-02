using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public static class CoreKeyboard
    {
        public static KeyboardState keyboardState;

        public static void PollState()
        {
            keyboardState = Keyboard.GetState();
        }
    }
}
