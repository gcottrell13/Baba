using Baba;
using BabaGame.src.Engine;
using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Core;
using Core.Configuration;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGame.src
{
    public class World : GameObject
    {
        private DateTime lastInput;

        private bool allowInput = false;

        private Dictionary<(int, int), MapDisplay> MapDisplays;

        private List<Keys> AllKeysPressed;

        private string worldName;

        private WordEngine engine;

        private TiledMap? WorldHandle;

        private MapData? currentMap;

        private List<MapData> Maps;

        private Dictionary<MapData, Dictionary<Direction, (int x, int y)>> Connections;

        public World(string world)
        {
            Maps = new List<MapData>();
            Connections = new Dictionary<MapData, Dictionary<Direction, (int x, int y)>>();
            MapDisplays = new Dictionary<(int, int), MapDisplay>();
            worldName = world;
            lastInput = DateTime.Today;
            engine = new WordEngine();
            WorldHandle = AllMaps.GetWorldHandle(world);

            EventChannels.MapChange.Subscribe(OnSetMapEvent);
            EventChannels.KeyPress.Subscribe(onKeyPress);
            EventChannels.CharacterControl.Subscribe(onCharacterControl);

            var (px, ph) = WorldVariables.GetSizeInPixels(WorldVariables.MapWidth + 2, WorldVariables.MapHeight + 2);
            var scale = BabaGame.Game?.SetScreenSize(px, ph);
            WorldVariables.Scale = (scale ?? 1f) * WorldVariables.BaseScale;

            AllKeysPressed = new List<Keys>();
        }

        protected override void OnDestroy()
        {
            EventChannels.MapChange.Unsubscribe(OnSetMapEvent);
            EventChannels.KeyPress.Unsubscribe(onKeyPress);
            EventChannels.CharacterControl.Unsubscribe(onCharacterControl);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Graphics.xscale = WorldVariables.Scale;
            Graphics.yscale = WorldVariables.Scale;

            base.OnUpdate(gameTime);

            if (allowInput && AllKeysPressed?.Count > 0)
            {
                if (DateTime.Now - lastInput >= TimeSpan.FromSeconds(WorldVariables.InputDelaySeconds + WorldVariables.MoveAnimationSeconds))
                {
                    lastInput = DateTime.Now;

                    if (AllKeysPressed.Contains(KeyMap.Up))
                    {
                        TakeAction("up");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Down))
                    {
                        TakeAction("down");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Left))
                    {
                        TakeAction("left");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Right))
                    {
                        TakeAction("right");
                    }
                    else if (AllKeysPressed.Contains(KeyMap.Wait))
                    {
                        TakeAction("wait");
                    }
                }

            }

        }

        void onCharacterControl(CharacterControl characterControl)
        {
            allowInput = characterControl.Enable;
        }

        public async Task OnSetMapEvent(MapChange ev)
        {
            var (px, py) = WorldVariables.GetSizeInPixels(1, 1);
            Graphics.x = px;
            Graphics.y = py;

            if (ev.Direction != null)
            {
                EventChannels.CharacterControl.SendMessage(new CharacterControl { Enable = false });

                static float smoothing(float t)
                {
                    return t * t * (-2 * t + 3);
                }

                Graphics.x = ev.Direction switch
                {
                    Direction.Up => 0,
                    Direction.Down => 0,
                    Direction.Left => -WorldVariables.MapWidthPixels * WorldVariables.Scale,
                    Direction.Right => WorldVariables.MapWidthPixels * WorldVariables.Scale,
                    _ => 0,
                } + px;

                Graphics.y = ev.Direction switch
                {
                    Direction.Up => -WorldVariables.MapHeightPixels * WorldVariables.Scale,
                    Direction.Down => WorldVariables.MapHeightPixels * WorldVariables.Scale,
                    Direction.Left => 0,
                    Direction.Right => 0,
                    _ => 0,
                } + py;

                var time = 1f;
                (AnimateValue? x, AnimateValue? y) animate = ev.Direction switch
                {
                    Direction.Up => (null, new AnimateValue(Graphics.y, py, time, smoothing)),
                    Direction.Down => (null, new AnimateValue(Graphics.y, py, time, smoothing)),
                    Direction.Left => (new AnimateValue(Graphics.x, px, time, smoothing), null),
                    Direction.Right => (new AnimateValue(Graphics.x, px, time, smoothing), null),
                    _ => (null, null),
                };

                EventChannels.ScheduledCallback.SendAsyncMessage(new Core.Events.ScheduledCallback(time)
                {
                    PerFrameCallback = (gameTime) =>
                    {
                        if (animate.x is AnimateValue x && x.ValueStillAlive(gameTime.ElapsedGameTime.TotalSeconds, out var valueX))
                        {
                            Graphics.x = valueX;
                        }
                        if (animate.y is AnimateValue y && y.ValueStillAlive(gameTime.ElapsedGameTime.TotalSeconds, out var valueY))
                        {
                            Graphics.y = valueY;
                        }
                    },
                    Callback = () =>
                    {
                        EventChannels.CharacterControl.SendAsyncMessage(new CharacterControl { Enable = true });
                    },
                });
            }
            else
            {
                Graphics.x = px;
                Graphics.y = py;
            }

            WorldHandle = AllMaps.GetWorldHandle(worldName);
            currentMap = GetOrLoadMap(ev.X, ev.Y);

            var mapsLoaded = new List<MapData>();

            if (currentMap != null)
            {
                foreach (var dir in loadMapDeltas)
                {
                    var md = LoadNeighbor(currentMap, dir);
                    if (md != null)
                    {
                        LoadNeighbor(md, DirectionExtensions.Opposite(dir));
                        if (!MapDisplays.TryGetValue((md.MapX, md.MapY), out var display))
                        {
                            display = new MapDisplay();
                            MapDisplays[(md.MapX, md.MapY)] = display;

                            foreach (var obj in md.AllObjects)
                            {
                                display.AddChild(obj, addGraphics: true);
                            }
                        }

                        if (display == null)
                            continue;

                        AddChild(display, addGraphics: true);
                        var (dx, dy) = DirectionExtensions.DeltaFromDirection(dir);
                        display.Graphics.x = dx * WorldVariables.MapWidthPixels;
                        display.Graphics.y = dy * WorldVariables.MapHeightPixels;
                        mapsLoaded.Add(md);
                    }
                }
            }

            foreach (var map in mapsLoaded)
            {
                map.DoJoinable();
            }

            foreach (var unloadMap in Maps.Except(mapsLoaded).ToList())
            {
                if (unloadMap is MapData md)
                {
                    UnloadMap(md);
                }
            }
        }

        void onKeyPress(KeyEvent keyEvent)
        {
            if (!keyEvent.Up && AllKeysPressed.Contains(keyEvent.ChangedKey) == false)
            {
                AllKeysPressed.Add(keyEvent.ChangedKey);
            }
            else if (keyEvent.Up && AllKeysPressed.Contains(keyEvent.ChangedKey))
            {
                AllKeysPressed.Remove(keyEvent.ChangedKey);
            }
        }

        private static Direction[] loadMapDeltas = new[]
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right,
            Direction.None,
        };

        public void TakeAction(string action)
        {
            currentMap?.TakeAction(action);

            //foreach (var _map in Maps.ToList())
            //{
            //    if (_map is MapData map && map != currentMap)
            //    {
            //        map.TakeAction("wait");
            //    }
            //}
        }

        public void MoveObjectToMap(MapData oldMap, MapData newMap, BaseObject obj)
        {
            oldMap.AllObjects.Remove(obj);
            newMap.AddObject(obj);
            obj.Parent?.RemoveChild(obj, graphics: true);
            if (MapDisplays.TryGetValue((newMap.MapX, newMap.MapY), out var newMapDisplay))
            {
                newMapDisplay.AddChild(obj, addGraphics: true);
            }
        }

        public IEnumerable<BaseObject> ObjectsAt(MapData from, int x, int y)
        {
            var _dir = from.IsObjectOutOfBounds(x, y);
            if (_dir is Direction dir)
            {
                var _neighbor = GetNeighbor(from, dir);
                if (_neighbor is MapData neighbor)
                {
                    var localX = neighbor.MapX * WorldVariables.MapWidth + x % WorldVariables.MapWidth;
                    var localY = neighbor.MapY * WorldVariables.MapHeight + y % WorldVariables.MapHeight;

                    return neighbor.ObjectsAt(localX, localY);
                }
            }
            return Array.Empty<BaseObject>();
        }

        public MapData? GetNeighbor(MapData me, Direction dir)
        {
            if (!Connections.ContainsKey(me))
                return null;
            if (!Connections[me].TryGetValue(dir, out var neighborCoord))
                return null;
            var (x, y) = neighborCoord;
            return GetOrLoadMap(x, y);
        }

        public Direction? GetDirectionOfNeighbor(MapData from, MapData to)
        {
            if (Connections.TryGetValue(from, out var conns))
            {
                var n = conns.FirstOrDefault(k => GetOrLoadMap(k.Value.x, k.Value.y) == to).Key;
                if (n != default) 
                    return n;
            }
            return null;
        }

        public MapData? LoadNeighbor(MapData me, Direction dir) 
        {
            if (GetNeighbor(me, dir) is MapData n) return n;

            var x = dir switch
            {
                Direction.Left => me.MapX - 1,
                Direction.Right => me.MapX + 1,
                _ => me.MapX,
            };

            var y = dir switch
            {
                Direction.Up => me.MapY - 1,
                Direction.Down => me.MapY + 1,
                _ => me.MapY,
            };

            var (dx, dy) = DirectionExtensions.DeltaFromDirection(dir);
            var delta = new Vector2(dx, dy);

            MapData? load(int mapX, int mapY)
            {
                if (ExistsMapDataAtMapCoords(mapX, mapY)) 
                {
                    return GetOrLoadMap(mapX, mapY);
                }

                if (WorldHandle?.ObjectLayers.First(layer => layer.Name == "Connections") is TiledMapObjectLayer layer)
                {
                    foreach (var obj in layer.Objects)
                    {
                        if (obj is TiledMapPolylineObject poly)
                        {
                            var (p1, p2) = polyLineToMapCoord(poly);

                            var destinationMapCoord = new Vector2(mapX, mapY);
                            p1.Floor();
                            p2.Floor();

                            if (p1 != destinationMapCoord && p2 != destinationMapCoord)
                                continue;

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

                            return load(linkedX, linkedY);
                        }
                    }
                }

                return null;
            }

            var loaded = load(x, y);
            if (loaded != null)
            {
                AddConnection(me, dir, loaded);
            }

            return loaded;
        }

        private (Vector2, Vector2) polyLineToMapCoord(TiledMapPolylineObject poly)
        {
            var scale = new Vector2(WorldVariables.MapWidth * WorldVariables.TileWidth, WorldVariables.MapHeight * WorldVariables.TileHeight);
            var p1 = poly.Position * 4 / scale;
            var p2 = p1 + new Vector2(poly.Points[1].X, poly.Points[1].Y) * 4 / scale;
            return (p1, p2);
        }

        private void AddConnection(MapData from, Direction dir, MapData to)
        {
            if (Connections.ContainsKey(from) == false)
            {
                Connections[from] = new Dictionary<Direction, (int x, int y)>()
                {
                    { dir, (to.MapX, to.MapY) },
                };
            }
            else if (Connections[from].ContainsKey(dir) == false)
            {
                Connections[from][dir] = (to.MapX, to.MapY);
            }
        }

        public bool ExistsMapDataAtTileCoords(int x, int y, out MapData? mapData)
        {
            var worldX = x / WorldVariables.MapWidth;
            var worldY = y / WorldVariables.MapHeight;
            return ExistsMapDataAtMapCoords(worldX, worldY, out mapData);
        }

        public bool ExistsMapDataAtMapCoords(int worldX, int worldY)
        {
            return ExistsMapDataAtMapCoords(worldX, worldY, out var _);
        }

        public bool ExistsMapDataAtMapCoords(int worldX, int worldY, out MapData? mapData)
        {
            mapData = null;
            try
            {
                var gid = WorldHandle?.TileLayers.First(layer => layer.Name == "Terrain").Tiles[worldX + worldY * WorldHandle.Width].GlobalIdentifier;
                if (gid == null || gid == 0)
                    return false;
                mapData = GetOrLoadMap(worldX, worldY);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public MapData? GetMapByTileCoord(int x, int y)
        {
            var worldX = x / WorldVariables.MapWidth;
            var worldY = y / WorldVariables.MapHeight;
            return GetOrLoadMap(worldX, worldY);
        }

        public MapData? GetOrLoadMap(int mapX, int mapY)
        {
            if (Maps.FirstOrDefault(map => map.MapX == mapX && map.MapY == mapY) is MapData m)
                return m;

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
                            Maps.Add(data);
                            data.Initialize();
                            return data;
                        }
                    }
                }
            }
            return null;
        }

        public void UnloadMap(MapData map)
        {
            if (Maps.Contains(map))
            {
                Maps.Remove(map);
                if (MapDisplays.Remove((map.MapX, map.MapY), out var removedDisplay))
                {
                    RemoveChild(removedDisplay, graphics: true);
                }
            }
        }
    }
}
