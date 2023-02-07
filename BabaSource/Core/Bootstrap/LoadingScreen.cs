using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bootstrap
{
    internal class LoadingScreen : GameObject
    {
        private readonly TimeSpan loadingLength;
        private readonly Action onComplete;
        double percent;
        BaseLoadingScreen screen;

        public LoadingScreen(TimeSpan loadingLength, Action onComplete)
        {
            var loadingScreens = new Func<BaseLoadingScreen>[]
            {
                () => new LS1(),
            };
            var n = DateTime.Now.Second % loadingScreens.Length;
            screen = loadingScreens[n]();
            AddChild(screen);
            this.loadingLength = loadingLength;
            this.onComplete = onComplete;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            percent += gameTime.ElapsedGameTime.TotalMilliseconds / loadingLength.TotalMilliseconds;

            if (percent >= 1)
            {
                onComplete();
            }

            screen.SetPercent((int)(percent * 100));
        }

        private abstract class BaseLoadingScreen : GameObject
        {
            public abstract void SetPercent(int percent);
        }

        private class LS1 : BaseLoadingScreen
        {
            private Text text = new();
            public LS1()
            {
                AddChild(text);
            }

            public override void SetPercent(int percent)
            {
                var n = Math.Clamp(percent / 9, 0, 10);
                text.SetText("[baba]".Repeat(n) + "[gray]" + "[baba]".Repeat(10 - n));
                text.Graphics.x = ScreenWidth / 2 - 5 * 24;
                text.Graphics.y = ScreenHeight / 2 - 12;
            }
        }
    }
}
