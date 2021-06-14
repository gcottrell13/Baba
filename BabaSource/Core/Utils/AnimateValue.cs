using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.Utils
{
    public class AnimateValue
    {
        private readonly float start;
        private readonly float end;
        private readonly float time;
        private readonly Func<float, float>? transform;
        private double currentTime;
        private DateTime startTime;

        public AnimateValue(float start, float end, float time, Func<float, float>? transform = null)
        {
            this.start = start;
            this.end = end;
            this.time = time;
            this.transform = transform;
            startTime = DateTime.Now;
        }

        public bool ValueStillAlive(double elapsed, out float value)
        {
            currentTime += elapsed;
            if (currentTime >= time)
            {
                value = end;
                Debug.WriteLine($"Animation for {time} seconds took {(DateTime.Now - startTime).TotalSeconds}");
                return false;
            }
            if (transform != null)
            {
                value = transform((float)currentTime / time) * (end - start) + start;
            }
            else
            {
                value = ((float)currentTime / time) * (end - start) + start;
            }
            return true;
        }
    }
}
