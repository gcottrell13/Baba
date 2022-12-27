using Core.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content
{
    public class ContentLoader
    {
        public static ContentLoader? LoadedContent { get; private set; }

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            if (LoadedContent != null) return;

            LoadedContent = new ContentLoader(graphicsDevice);
        }

        public readonly Dictionary<string, SpriteValues> SpriteValues;

        private ContentLoader(GraphicsDevice graphicsDevice)
        {
            var sheets = Sheets.GetSheets(graphicsDevice);
            SpriteValues = SheetMap.GetSpriteInfo(sheets);
        }
    }
}
