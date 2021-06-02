using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public static class CoreMouse
    {
        public static MouseState mouseState;

        public static void PollState()
        {
            mouseState = Mouse.GetState();
        }
    }
}
