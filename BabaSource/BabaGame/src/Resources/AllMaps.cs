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
        public static TiledMap? GetMapHandle(string name)
        {
            return allMaps.TryGetValue(name, out var value) ? value.Value : null;
        }

        public static void LoadMaps()
        {
            allMaps = Load.loadAll("Maps", ResourceLoader.AddTiledMapResource);
        }

        private static Dictionary<string, ResourceHandle<TiledMap>> allMaps;


        public static TiledMap? GetWorldHandle(string name)
        {
            return allWorlds.TryGetValue(name, out var value) ? value.Value : null;
        }
        public static void LoadWorlds()
        {
            allWorlds = Load.loadAll("Worlds", ResourceLoader.AddTiledMapResource);
        }

        private static Dictionary<string, ResourceHandle<TiledMap>> allWorlds;
    }
}
