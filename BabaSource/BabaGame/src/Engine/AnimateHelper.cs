using BabaGame.src.Events;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.src.Engine
{
    internal static class AnimateHelper
    {
        public static Task Animate(ref float value, float time, float destination, Action<float> setValue, Func<float, float>? transform = null) => Animate(ref value, time, new[] {destination}, setValue, transform);

        public static Task Animate(ref float value, float time, float[] destinations, Action<float> setValue, Func<float, float>? transform = null)
        {
            var animate = new AnimateValue(value, destinations, time, transform);
            var task = new TaskCompletionSource<object>();
            EventChannels.ScheduledCallback.SendAsyncMessage(new Core.Events.ScheduledCallback(time)
            {
                PerFrameCallback = (gameTime) =>
                {
                    animate.ValueStillAlive(gameTime.ElapsedGameTime.TotalSeconds, out var valueX);
                    setValue(valueX);
                },
                Callback = () =>
                {
                    task.SetResult(0);
                },
            });
            return task.Task;
        }
    }
}
