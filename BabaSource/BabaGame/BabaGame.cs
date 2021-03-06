using BabaGame.src.Events;
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

        public static int MAX_WIDTH = 1080;
        public static int MAX_HEIGHT = 720;

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
            Window.KeyDown += _window_KeyDown;
            Window.KeyUp += _window_KeyUp;
            base.Initialize();
        }

        private void _window_KeyUp(object? sender, InputKeyEventArgs e)
        {
            EventChannels.KeyPress.SendAsyncMessage(new KeyEvent { ChangedKey = e.Key, Up = true });
        }

        private void _window_KeyDown(object? sender, InputKeyEventArgs e)
        {
            EventChannels.KeyPress.SendAsyncMessage(new KeyEvent { ChangedKey = e.Key, Up = false });
        }

        public float SetScreenSize(int x, int y)
        {
            var maxRatio = (float)MAX_WIDTH / MAX_HEIGHT;
            var ratio = (float)x / y;
            var scale = 1f;

            if (ratio >= maxRatio && x > MAX_WIDTH)
            {
                scale = (float)MAX_WIDTH / x;
                x = MAX_WIDTH;
                y = (int)(y * scale);
            }
            else if (ratio <= maxRatio && y > MAX_HEIGHT)
            {
                scale = (float)MAX_HEIGHT / y;
                y = MAX_HEIGHT;
                x = (int)(x * scale);
            }

            _graphics.PreferredBackBufferWidth = x;
            _graphics.PreferredBackBufferHeight = y;
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Scene.Initialize(_spriteBatch, x, y);

            return scale;
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
