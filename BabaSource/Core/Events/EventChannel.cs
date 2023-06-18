using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public abstract class EventChannel
    {
        internal abstract void SendAsyncMessages();
    }

    public class EventChannel<TMessage> : EventChannel
    {
        private List<Action<TMessage>> subscriptions = new List<Action<TMessage>>();
        private List<Func<TMessage, Task>> taskSubscriptions = new List<Func<TMessage, Task>>();

        private List<TMessage> pendingAsyncMessages = new List<TMessage>();

        internal EventChannel()
        {

        }

        internal override void SendAsyncMessages()
        {
            var messages = pendingAsyncMessages.ToList();
            pendingAsyncMessages.Clear();

            foreach (var message in messages)
            {
                foreach (var subscriber in subscriptions.ToList())
                {
                    subscriber(message);
                }
                foreach (var subscriber in taskSubscriptions.ToList())
                {
                    subscriber(message);
                }
            }
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

        public void Subscribe(Func<TMessage, Task> callback)
        {
            if (taskSubscriptions.Contains(callback) == false)
            {
                taskSubscriptions.Add(callback);
            }
        }

        public void Unsubscribe(Func<TMessage, Task> callback)
        {
            if (taskSubscriptions.Contains(callback))
            {
                taskSubscriptions.Remove(callback);
            }
        }
    }
}
