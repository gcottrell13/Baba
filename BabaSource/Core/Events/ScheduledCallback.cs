using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Events
{
    public class ScheduledCallback
    {
        public ScheduledCallback(double durationSeconds)
        {
            DurationSeconds = durationSeconds;
        }

        public Action? Callback;
        public Action<double>? PerFrameCallback;
        public double DurationSeconds;
    }
}
