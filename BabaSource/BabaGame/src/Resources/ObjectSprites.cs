using Core.Configuration;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BabaGame.src.Resources
{
    public static class ObjectSprites
    {
        public static ResourceHandle<Texture2D> GetTextureHandle(string name)
        {
            return allTextures.TryGetValue(name, out var value) ? value : throw new Exception($"Failed to load sprite for {name}");
        }

        private static Dictionary<string, ResourceHandle<Texture2D>> allTextures = Load.loadAll("Sheets", ResourceLoader.AddGraphicsResource);

    }
}
