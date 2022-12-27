using Core.Events;
using Core.Utils;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Core.Bootstrap
{
    public abstract class GameEntryPoint : GameObject
    {
        public GameEntryPoint()
        {
            Name = "ROOT";
            Graphics.Name = "ROOT";
        }

        public virtual void EntryTick(GameTime gameTime)
        {
            Tick(gameTime);
        }

        protected void AddCallbackRunner()
        {
            AddChild(new CallbackRunner(CoreEventChannels.ScheduledCallback.Subscribe, CoreEventChannels.ScheduledCallback.Unsubscribe));
        }

        public abstract void Initialize();
    }

}
