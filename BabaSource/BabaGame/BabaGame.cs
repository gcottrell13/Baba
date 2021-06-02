using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Core.Events;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Baba
{
    public class BabaGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch? _spriteBatch;

        public BabaGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            GameEntryPoint.ROOT.Initialize(CommandLineArguments.Args?.Connect ?? "127.0.0.1");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Scene.Initialize(_spriteBatch, 1080, 900);
            
            ResourceLoader.LoadAll(this);

            var j = JsonValues.Colors;

            GameEntryPoint.ROOT.AfterInitialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CoreKeyboard.PollState();
            CoreMouse.PollState();

            GameEntryPoint.ROOT.EntryTick(gameTime);

            // TODO: Add your update logic here

            EventManager.SendAsyncMessages();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Scene.Begin();
            SpriteContainer.ROOT.Draw();
            Scene.End();

            _spriteBatch?.Begin();
            _spriteBatch?.Draw(Scene.scene, Vector2.Zero, Color.White);
            _spriteBatch?.End();

            base.Draw(gameTime);
        }
    }
}
