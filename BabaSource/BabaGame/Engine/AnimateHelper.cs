using Core.Events;
using Core.Utils;
using System;
using System.Threading.Tasks;

namespace BabaGame.Engine
{
    internal static class AnimateHelper
    {
        public static Task Animate(ref float value, float time, float destination, Action<float> setValue, Func<float, float>? transform = null) => Animate(ref value, time, new[] { destination }, setValue, transform);

        public static Task Animate(ref float value, float time, float[] destinations, Action<float> setValue, Func<float, float>? transform = null)
        {
            var animate = new AnimateValue(value, destinations, time, transform);
            var task = new TaskCompletionSource<object>();
            CoreEventChannels.ScheduledCallback.SendAsyncMessage(new ScheduledCallback(time)
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
