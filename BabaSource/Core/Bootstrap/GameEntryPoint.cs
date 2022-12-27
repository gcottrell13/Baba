using Core.Events;
using Core.Utils;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Core.Bootstrap
{
    public abstract class GameEntryPoint : GameObject
    {
        public SpriteContainer RootSprite { get; }

        public GameEntryPoint()
        {
            Name = "ROOT";
            RootSprite = new SpriteContainer() { Name = "ROOT" };

        }

        new public void AddChild(GameObject gameObject, bool addGraphics = false)
        {
            base.AddChild(gameObject, addGraphics);
            if (addGraphics)
            {
                RootSprite.AddChild(gameObject.Graphics);
            }
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
