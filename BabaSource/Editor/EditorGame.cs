using Content;
using Content.UI;
using Core.Bootstrap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Editor
{
    public class EditorGame : GameSetup
    {
        public EditorGame() : base(new EditorGameEntryPoint())
        {
            MAX_WIDTH = 1080;
            MAX_HEIGHT = 720;
        }

        protected override void Initialize()
        {
            ContentLoader.LoadContent(GraphicsDevice);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            // TODO: Add your update logic here
        }

        private class EditorGameEntryPoint : GameEntryPoint
        {
            public override void Initialize()
            {
                AddChild(new Text("hello-[blue]there/[100,255,100]?\nBaba [arrow] [white][baba:17][green] says:[200, 200, 200] \"hi\"", Color.Red), true);
            }
        }
    }
}
