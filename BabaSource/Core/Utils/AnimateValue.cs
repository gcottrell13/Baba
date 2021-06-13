using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public class AnimateValue
    {
        private readonly float start;
        private readonly float end;
        private readonly float time;
        private double currentTime;

        public AnimateValue(float start, float end, float time)
        {
            this.start = start;
            this.end = end;
            this.time = time;
        }

        public bool ValueStillAlive(GameTime gameTime, out float value)
        {
            currentTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime >= time)
            {
                value = end;
                return false;
            }
            value = (float)Math.Sqrt(currentTime / time) * (end - start) + start;
            return true;
        }
    }
}
