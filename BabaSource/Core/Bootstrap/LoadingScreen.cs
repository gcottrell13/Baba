using Core.Content;
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

            var name = new[]
            {
                "baba",
                "baba",
                "baba",
                "baba",
                "baba",
                "jiji",
                "robot",
            }.RandomElement();

            screen = new Func<BaseLoadingScreen>[]
            {
                () => new StringOfBaba(name),
                () => new LongBaba(name),
            }.RandomElement()();
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

            screen.SetPercent(percent);
        }

        private abstract class BaseLoadingScreen : GameObject
        {
            public abstract void SetPercent(double percent);
        }

        private class StringOfBaba : BaseLoadingScreen
        {
            private readonly string name;
            private Text text = new();
            public StringOfBaba(string name)
            {
                AddChild(text);
                this.name = name;
            }

            public override void SetPercent(double percent)
            {
                var p = (int)(percent * 100);
                var n = Math.Clamp(p / 9, 0, 10);
                text.SetText($"[{name}]".Repeat(n) + "[gray]" + $"[{name}]".Repeat(10 - n));
                text.Graphics.x = ScreenWidth / 2 - 5 * 24;
                text.Graphics.y = ScreenHeight / 2 - 12;
            }
        }

        private class LongBaba : BaseLoadingScreen
        {
            private Text text = new();
            public LongBaba(string name)
            {
                AddChild(text);
                var color = ThemeInfo.GetObjectColor("default", name).ToHexTriple();
                text.SetText($"{color}[{name}]");
            }

            public override void SetPercent(double percent)
            {
                var xscale = (ScreenWidth / 2) / 12 * (float)percent;
                var yscale = (ScreenHeight / 2) / 12 * (float)percent;
                text.Graphics.xscale = xscale;
                text.Graphics.yscale = yscale;
                text.Graphics.x = ScreenWidth / 2 - 12 * xscale;
                text.Graphics.y = ScreenHeight / 2 - 12 * yscale;
            }
        }
    }
}
