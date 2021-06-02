using Core.Configuration;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Resources
{
    public static class Palettes
    {
        public static Texture2D? GetTextureHandle(string name)
        {
            return allTextures.TryGetValue(name, out var value) ? value.Value : null;
        }

        private static Dictionary<string, ResourceHandle<Texture2D>> allTextures = Load.loadAll("Palettes", ResourceLoader.AddGraphicsResource);

    }
}
