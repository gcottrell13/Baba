using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Events
{
    public class EventManager
    {
        private static readonly List<EventChannel> eventChannels = new List<EventChannel>();

        public static EventChannel<TMessage> CreateChannel<TMessage>()
        {
            var channel = new EventChannel<TMessage>();
            eventChannels.Add(channel);
            return channel;
        }

        public static void SendAsyncMessages()
        {
            foreach (var channel in eventChannels.ToList())
            {
                channel.SendAsyncMessages();
            }
        }
    }
}
