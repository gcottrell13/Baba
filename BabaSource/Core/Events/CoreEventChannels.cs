using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Events
{
    public class CoreEventChannels
    {
        public static EventChannel<KeyEvent> KeyEvent = EventManager.CreateChannel<KeyEvent>();
        public static EventChannel<TextInput> TextInput = EventManager.CreateChannel<TextInput>();
        public static EventChannel<ScheduledCallback> ScheduledCallback = EventManager.CreateChannel<ScheduledCallback>();
    }
}
