using BabaGame.src;
using BabaGame.src.Events;
using BabaGame.src.Resources;
using Core.Bootstrap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BabaGame
{
    public class BabaGame : GameSetup
    {
        public BabaGame() : base(new BabaGameEntryPoint())
        {
            MAX_WIDTH = 1080;
            MAX_HEIGHT = 720;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            // TODO: Add your update logic here
        }

        private class BabaGameEntryPoint : GameEntryPoint
        {
            public override void Initialize()
            {
                AllMaps.LoadMaps();
                AllMaps.LoadWorlds();
                ObjectSprites.LoadTextures();

                AddChild(new World("world1"), true);
                AddCallbackRunner();

                EventChannels.MapChange.SendAsyncMessage(new MapChange { X = 22, Y = 26 });
                EventChannels.CharacterControl.SendAsyncMessage(new CharacterControl { Enable = true });
            }
        }
    }
}
