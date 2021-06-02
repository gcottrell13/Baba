using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Events
{
    public abstract class EventChannel
    {
        internal abstract void SendAsyncMessages();
    }

    public class EventChannel<TMessage> : EventChannel
    {
        private List<Action<TMessage>> subscriptions = new List<Action<TMessage>>();

        private List<TMessage> pendingAsyncMessages = new List<TMessage>();

        internal EventChannel()
        {

        }

        internal override void SendAsyncMessages()
        {
            foreach (var message in pendingAsyncMessages.ToList()) 
            foreach (var subscriber in subscriptions.ToList())
            {
                subscriber(message);
            }

            pendingAsyncMessages = new List<TMessage>();
        }

        public void SendAsyncMessage(TMessage message)
        {
            SendMessage(message, true);
        }

        public void SendSyncMessage(TMessage message)
        {
            SendMessage(message, false);
        }

        public void SendMessage(TMessage message, bool async = true)
        {
            if (async)
            {
                pendingAsyncMessages.Add(message);
            }
            else
            {
                foreach (var subscriber in subscriptions)
                {
                    subscriber(message);
                }
            }
        }

        public void Subscribe(Action<TMessage> callback)
        {
            if (subscriptions.Contains(callback) == false)
            {
                subscriptions.Add(callback);
            }
        }

        public void Unsubscribe(Action<TMessage> callback)
        {
            if (subscriptions.Contains(callback))
            {
                subscriptions.Remove(callback);
            }
        }
    }
}
