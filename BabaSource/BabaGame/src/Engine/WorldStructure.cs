using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src.Engine
{
    public class WorldStructure
    {

        public MapData[,] Maps;
        private readonly string worldName;

        private WordEngine engine;

        private TiledMap? WorldHandle;

        public int CurrentX { get; private set; }
        public int CurrentY { get; private set; }

        public WorldStructure(string worldName)
        {
            engine = new WordEngine();
            Maps = new MapData[0, 0];

            this.worldName = worldName;
        }

        public void SetMap(MapChange mapChange)
        {
            WorldHandle = AllMaps.GetWorldHandle(worldName);
            if (Maps.Length == 0 && WorldHandle != null)
            {
                Maps = new MapData[WorldHandle.Width + 1, WorldHandle.Height + 1];
            }

            CurrentX = mapChange.X;
            CurrentY = mapChange.Y;

            GetOrLoadMap(CurrentX, CurrentY);
        }

        public MapData? GetOrLoadMap(int mapX, int mapY)
        {
            if (Maps[mapX, mapY] != null) 
                return Maps[mapX, mapY];

            if (WorldHandle != null)
            {
                var gid = WorldHandle.TileLayers.First(layer => layer.Name == "Terrain").Tiles[mapX + mapY * WorldHandle.Width].GlobalIdentifier;
                var _tileset = WorldHandle.GetTilesetByTileGlobalIdentifier(gid);
                if (_tileset is TiledMapTileset tileset)
                {
                    var firstGid = WorldHandle.GetTilesetFirstGlobalIdentifier(tileset);
                    foreach (var tile in tileset.Tiles)
                    {
                        if (tile.LocalTileIdentifier + firstGid == gid && tile.Properties.TryGetValue("name", out var name))
                        {
                            var data = new MapData(name, engine, this, mapX, mapY);
                            Maps[mapX, mapY] = data;
                            data.Initialize();
                            return data;
                        }
                    }
                }
            }
            return null;
        }

        public void TakeAction(string action)
        {
            foreach (var _map in Maps)
            {
                if (_map is MapData map)
                {
                    map.TakeAction(action);
                }
            }
        }

        public IEnumerable<BaseObject> ObjectsAt(MapData from, int x, int y)
        {
            var _neighbor = GetNeighborByTileCoord(from, x, y);
            if (_neighbor is MapData neighbor)
            {
                var localX = neighbor.MapX * WorldVariables.MapWidth + x % WorldVariables.MapWidth;
                var localY = neighbor.MapY * WorldVariables.MapHeight + y % WorldVariables.MapHeight;

                return neighbor.ObjectsAt(localX, localY);
            }
            return Array.Empty<BaseObject>();
        }

        private MapData? GetNeighborByTileCoord(MapData from, int mapX, int mapY)
        {
            return GetNeighbor(from, mapX / WorldVariables.MapWidth, mapY / WorldVariables.MapHeight);
        }

        public MapData? GetNeighbor(MapData me, int x, int y)
        {
            var destinationMapCoord = new Vector2(x, y);
            var mapCoord = new Vector2(me.MapX, me.MapY);
            var delta = destinationMapCoord - mapCoord;

            var neighbor = GetOrLoadMap((int)destinationMapCoord.X, (int)destinationMapCoord.Y);

            if (neighbor == null)
            {
                if (ExistsMapDataAtMapCoords(x, y))
                {
                    return neighbor;
                }

                if (WorldHandle?.ObjectLayers.First(layer => layer.Name == "Connections") is TiledMapObjectLayer layer)
                {
                    foreach (var obj in layer.Objects)
                    {
                        if (obj is TiledMapPolylineObject poly)
                        {
                            var (p1, p2) = polyLineToMapCoord(poly);

                            var destinationLink = p1 == destinationMapCoord ? p2 : p1;

                            var linkedX = (int)destinationLink.X;
                            var linkedY = (int)destinationLink.Y;

                            if (ExistsMapDataAtMapCoords(linkedX, linkedY))
                            {
                                return GetOrLoadMap(linkedX, linkedY);
                            }

                            destinationLink += delta;

                            linkedX = (int)destinationLink.X;
                            linkedY = (int)destinationLink.Y;

                            return GetOrLoadMap(linkedX, linkedY);
                        }
                    }
                }
            }

            return neighbor;
        }

        private (Vector2, Vector2) polyLineToMapCoord(TiledMapPolylineObject poly)
        {
            var scale = new Vector2(WorldVariables.MapWidth * WorldVariables.TileWidth, WorldVariables.MapHeight * WorldVariables.TileHeight);
            var p1 = poly.Position * 4 / scale;
            var p2 = p1 + new Vector2(poly.Points[1].X, poly.Points[1].Y) * 4 / scale;
            return (p1, p2);
        }

        public bool ExistsMapDataAtTileCoords(int x, int y)
        {
            var worldX = x / WorldVariables.MapWidth;
            var worldY = y / WorldVariables.MapHeight;
            return ExistsMapDataAtMapCoords(worldX, worldY);
        }

        public bool ExistsMapDataAtMapCoords(int worldX, int worldY)
        {
            try
            {
                var gid = WorldHandle?.TileLayers.First(layer => layer.Name == "Terrain").Tiles[worldX + worldY * WorldHandle.Width].GlobalIdentifier;
                if (gid == null || gid == 0)
                    return false;
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public void UnloadMap(MapData map)
        {

        }
    }
}
