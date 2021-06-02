using Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baba.src.Events
{
    static class EventChannels
    {
        public static EventChannel<KeyEvent> KeyPress = EventManager.CreateChannel<KeyEvent>();
    }
}
