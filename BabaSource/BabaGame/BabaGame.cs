using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Core.Events;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BabaGame
{
    public class BabaGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch? _spriteBatch;

        public static BabaGame? Game;

        public BabaGame()
        {
            Game = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            SetScreenSize(1080, 720);
            GameEntryPoint.ROOT.Initialize(CommandLineArguments.Args?.Connect ?? "127.0.0.1");
            base.Initialize();
        }

        public void SetScreenSize(int x, int y)
        {
            _graphics.PreferredBackBufferWidth = x;
            _graphics.PreferredBackBufferHeight = y;
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Scene.Initialize(_spriteBatch, x, y);
        }

        protected override void LoadContent()
        {
            ResourceLoader.LoadAll(this);

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
