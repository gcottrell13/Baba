using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Core.Configuration
{
    public static class ResourceLoader
    {
        private static readonly List<IResourceHandle> resourceHandles = new List<IResourceHandle>();

        public static ResourceHandle<Texture2D> AddGraphicsResource(string name)
        {
            var handle = new ResourceHandle<Texture2D>(name);
            resourceHandles.Add(handle);
            return handle;
        }

        public static ResourceHandle<TiledMap> AddTiledMapResource(string name)
        {
            var handle = new ResourceHandle<TiledMap>(name);
            resourceHandles.Add(handle);
            return handle;
        }

        public static void LoadAll(Game game)
        {
            foreach (var resource in resourceHandles)
            {
                if (resource is ResourceHandle<Texture2D> tx)
                {
                    tx.SetValue(game.Content.Load<Texture2D>(resource.Name));
                }
                else if (resource is ResourceHandle<TiledMap> tiled)
                {
                    tiled.SetValue(game.Content.Load<TiledMap>(resource.Name));
                }
            }
        }
    }
}
