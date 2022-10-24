using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public class AnimateValue
    {
        private readonly List<float> points;
        private readonly float time;
        private readonly Func<float, float> transform;
        private double currentTime;
        private DateTime startTime;

        private double inverseChunkTime;

        public AnimateValue(float start, float end, float time, Func<float, float>? transform = null): this(start, new[] {end}, time, transform)
        {
        }

        private float defaultTransform(float t) => t;

        public AnimateValue(float start, IEnumerable<float> end, float time, Func<float, float>? transform = null)
        {
            this.points = new[] { start }.Concat(end).ToList();
            this.time = time;
            this.transform = transform ?? defaultTransform;
            startTime = DateTime.Now;
            inverseChunkTime = end.Count() / time;
        }

        public bool ValueStillAlive(double elapsed, out float value)
        {
            currentTime += elapsed;
            if (currentTime >= time)
            {
                value = points.Last();
                Debug.WriteLine($"Animation for {time} seconds took {(DateTime.Now - startTime).TotalSeconds}");
                return false;
            }
            var index = (int)Math.Ceiling(currentTime * inverseChunkTime);
            var start = points[index - 1];
            var end = points[index];
            value = transform((float)currentTime / time) * (end - start) + start;
            return true;
        }
    }
}
