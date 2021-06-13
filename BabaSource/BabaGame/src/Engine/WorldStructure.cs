using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src.Engine
{
    public class WorldStructure
    {

        private MapData[,] Maps;
        public const int MapWidth = 24;
        public const int MapHeight = 18;
        private readonly string worldName;

        private WordEngine engine;

        public int CurrentX { get; private set; }
        public int CurrentY { get; private set; }

        public WorldStructure(string worldName)
        {
            engine = new WordEngine();
            Maps = new MapData[0, 0];

            engine.AddRule(new WordEngine.FullRule
            {
                TargetName = "baba",
                Verb = "is",
                Feature = "you",
            });

            engine.AddRule(new WordEngine.FullRule
            {
                TargetName = "wall",
                Verb = "is",
                Feature = "stop",
            });

            engine.AddRule(new WordEngine.FullRule
            {
                TargetName = "box",
                Verb = "is",
                Feature = "push",
            });

            this.worldName = worldName;
        }

        public IEnumerable<BaseObject> SetMap(MapChange mapChange)
        {
            var world = AllMaps.GetWorldHandle(worldName);
            if (Maps.Length == 0 && world != null)
            {
                Maps = new MapData[world.Width + 1, world.Height + 1];
            }

            CurrentX = mapChange.X;
            CurrentY = mapChange.Y;

            if (world != null) {
                var gid = world.TileLayers.First(layer => layer.Name == "Terrain").Tiles[CurrentX + CurrentY * world.Width].GlobalIdentifier;
                foreach (var tileset in world.Tilesets)
                {
                    foreach (var tile in tileset.Tiles)
                    {
                        if (tile.Properties.TryGetValue("name", out var name))
                        {
                            var data = new MapData(name, engine, this);
                            Maps[CurrentX, CurrentY] = data;
                            return data.Initialize();
                        }
                    }
                }
            }

            return Array.Empty<BaseObject>();
        }

        public void TakeAction(string action)
        {
            foreach (var map in Maps)
            {
                map?.TakeAction(action);
            }
        }

        public IEnumerable<BaseObject> ObjectsAt(MapData from, int x, int y)
        {
            yield break;
        }

        public void UnloadMap(MapData map)
        {

        }
    }
}
