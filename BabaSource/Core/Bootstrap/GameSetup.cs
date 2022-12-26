using Core;
using Core.Configuration;
using Core.Events;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core.Bootstrap
{
    public class GameSetup : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch? _spriteBatch;

        protected static int MAX_WIDTH = 1080;
        protected static int MAX_HEIGHT = 720;

        protected GameEntryPoint EntryPoint { get; }

        public GameSetup(GameEntryPoint entryPoint)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            EntryPoint = entryPoint;
        }

        protected override void Initialize()
        {
            SetScreenSize(MAX_WIDTH, MAX_HEIGHT);
            EntryPoint.Initialize();
            Window.KeyDown += _window_KeyDown;
            Window.KeyUp += _window_KeyUp;
            base.Initialize();
        }

        private void _window_KeyUp(object? sender, InputKeyEventArgs e)
        {
            CoreEventChannels.KeyPress.SendAsyncMessage(new KeyEvent { ChangedKey = e.Key, Up = true });
        }

        private void _window_KeyDown(object? sender, InputKeyEventArgs e)
        {
            CoreEventChannels.KeyPress.SendAsyncMessage(new KeyEvent { ChangedKey = e.Key, Up = false });
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

            EntryPoint.AfterInitialize();
        }

        protected override void Update(GameTime gameTime)
        {
            CoreKeyboard.PollState();
            CoreMouse.PollState();

            EntryPoint.EntryTick(gameTime);

            EventManager.SendAsyncMessages();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Scene.Begin();
            EntryPoint.RootSprite.Draw();
            Scene.End();

            _spriteBatch?.Begin();
            _spriteBatch?.Draw(Scene.scene, Vector2.Zero, Color.White);
            _spriteBatch?.End();

            base.Draw(gameTime);
        }
    }
}
