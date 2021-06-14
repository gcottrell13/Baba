using Core.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public class CallbackRunner : GameObject
    {
        private List<CallbackRecord> callbacks;
        private readonly Action<Action<ScheduledCallback>> unsubFunction;

        public CallbackRunner(Action<Action<ScheduledCallback>> subscribeFunction, Action<Action<ScheduledCallback>> unsubFunction)
        {
            callbacks = new List<CallbackRecord>();
            subscribeFunction(OnEvent);
            this.unsubFunction = unsubFunction;
        }

        protected override void OnDestroy()
        {
            unsubFunction(OnEvent);
            base.OnDestroy();
        }

        private void OnEvent(ScheduledCallback ev)
        {
            if (ev.DurationSeconds <= 0f) return;

            callbacks.Add(new CallbackRecord
            {
                ScheduledCallback = ev,
                StartTime = DateTime.Now,
                TimeSpan = TimeSpan.FromSeconds(ev.DurationSeconds),
            });
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var cb in callbacks.ToList())
            {
                var elapsed = DateTime.Now - cb.StartTime;

                cb.ScheduledCallback.PerFrameCallback?.Invoke(gameTime);

                if (elapsed > cb.TimeSpan)
                {
                    callbacks.Remove(cb);
                    cb.ScheduledCallback.Callback?.Invoke();
                }
            }
        }

        struct CallbackRecord
        {
            public ScheduledCallback ScheduledCallback;
            public DateTime StartTime;
            public TimeSpan TimeSpan;
        }
    }
}
