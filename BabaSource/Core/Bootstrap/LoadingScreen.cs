using Core.UI;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bootstrap
{
    internal class LoadingScreen : GameObject
    {
        private Text text = new();

        public LoadingScreen()
        {
            AddChild(text);
        }

        public void SetPercent(int percent)
        {
            var n = percent / 10;
            text.SetText("[baba]".Repeat(n) + "[gray]" + "[baba]".Repeat(10 - n));
            text.Graphics.x = ScreenWidth / 2 - 5 * 24;
            text.Graphics.y = ScreenHeight / 2 - 12;
        }
    }
}
