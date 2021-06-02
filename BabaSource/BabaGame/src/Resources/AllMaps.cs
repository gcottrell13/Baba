using Core.Configuration;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BabaGame.src.Resources
{
    public static class AllMaps
    {
        public static ResourceHandle<TiledMap> Map = ResourceLoader.AddTiledMapResource($"Maps/start");
        public static TiledMap? GetMapHandle(string name)
        {
            return allMaps.TryGetValue(name, out var value) ? value.Value : null;
        }

        private static Dictionary<string, ResourceHandle<TiledMap>> allMaps = Load.loadAll("Maps", ResourceLoader.AddTiledMapResource);
    }
}
