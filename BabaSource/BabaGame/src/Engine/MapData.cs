using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BabaGame.src.Engine.WordEngine;

namespace BabaGame.src.Engine
{
    public class MapData
    {
        private readonly int gridx;
        private readonly int gridy;
        public List<BaseObject> AllObjects;

        private WordEngine engine;
        private readonly WorldStructure world;
        private TiledMap? tiledMap;

        public int MapX { get; }
        public int MapY { get; }

        public MapData(string mapName, WordEngine engine, WorldStructure world, int mapX, int mapY)
        {
            tiledMap = AllMaps.GetMapHandle(mapName);

            WorldVariables.TileWidth = tiledMap.TileWidth;
            WorldVariables.TileHeight = tiledMap.TileHeight;

            gridx = tiledMap.Width;
            gridy = tiledMap.Height;

            AllObjects = new List<BaseObject>();

            this.engine = engine;
            this.world = world;
            MapX = mapX;
            MapY = mapY;
        }

        public IEnumerable<BaseObject> Initialize()
        {
            var objects = new List<BaseObject>();

            var offsetX = MapX * WorldVariables.MapWidth;
            var offsetY = MapY * WorldVariables.MapHeight;

            foreach (var layer in tiledMap.TileLayers)
            {
                if (layer.Name == "Map")
                {
                    foreach (var tile in layer.Tiles)
                    {
                        if (tile.GlobalIdentifier != 0)
                        {
                            var name = JsonValues.Tileset[tile.GlobalIdentifier];
                            if (name.Contains("-") == false)
                            {
                                // non-directional
                                var stats = JsonValues.ObjectInfo[name];
                                var f = new BaseObject(name, tile.X + offsetX, tile.Y + offsetY, this);
                                objects.Add(f);
                                AddObject(f);
                                f.Graphics.zindex = stats.layer;
                            }
                            else
                            {
                                var parts = name.Split("-");
                                name = parts[0];
                                var direction = parts[1];

                                var stats = JsonValues.ObjectInfo[name];

                                var f = new BaseObject(name, tile.X + offsetX, tile.Y + offsetY, this, direction);
                                objects.Add(f);
                                AddObject(f);
                                f.Graphics.zindex = stats.layer;
                            }
                        }
                    }
                }
                else if (layer.Name == "Words")
                {
                    if (layer.Properties.TryGetValue("sentences", out var builtInSentences))
                    {
                        foreach (var line in builtInSentences.Split('\n'))
                        {
                            engine.AddRule(WordEngine.ParsePhrase(line.Trim()));
                        }
                    }
                }

            }

            DoJoinable();

            return objects;
        }

        public Dictionary<Direction, List<BaseObject>> GetObjectsNear(int x, int y)
        {
            var near = new Dictionary<Direction, List<BaseObject>>
            {
                [Direction.Up] = ObjectsAt(x, y - 1).Concat(world.ObjectsAt(this, x, y - 1)).ToList(),
                [Direction.Down] = ObjectsAt(x, y + 1).Concat(world.ObjectsAt(this, x, y + 1)).ToList(),
                [Direction.Left] = ObjectsAt(x - 1, y).Concat(world.ObjectsAt(this, x - 1, y)).ToList(),
                [Direction.Right] = ObjectsAt(x + 1, y).Concat(world.ObjectsAt(this, x + 1, y)).ToList()
            };
            return near;
        }

        public IEnumerable<BaseObject> ObjectsAt(int x, int y)
        {
            return AllObjects.Where(o => o.X == x && o.Y == y);
        }

        public IEnumerable<BaseObject> OutOfBoundsObjects()
        {
            return AllObjects.Where(IsObjectOutOfBounds);
        }

        public bool IsObjectOutOfBounds(BaseObject obj)
        {
            if (obj.X < WorldVariables.MapWidth * MapX) return true;
            else if (obj.X >= WorldVariables.MapWidth * (MapX + 1)) return true;
            else if (obj.Y < WorldVariables.MapHeight * MapY) return true;
            else if (obj.Y >= WorldVariables.MapHeight * (MapY + 1)) return true;
            return false;
        }

        public void AddObject(BaseObject obj)
        {
            if (AllObjects.Contains(obj))
            {
                throw new Exception();
            }
            obj.MapData = this;
            AllObjects.Add(obj);
        }

        public void TakeAction(string action)
        {
            var intentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();
            var newIntentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();

            var cache = new QueryCache(AllObjects, engine);

            if (action != "wait")
            {
                foreach (var you in cache.GetUnitsWithEffect("you"))
                    intentsToMove.Add(directionMoveObject(you, DirectionExtensions.FromString(action)));
            }

            foreach (var move in cache.GetUnitsWithEffect("move"))
                intentsToMove.Add(directionMoveObject(move, move.Facing));

            bool canMove(int oldX, int oldY, int newX, int newY)
            {
                if (oldX == newX && oldY == newY) return false;

                if (!world.ExistsMapDataAtTileCoords(newX, newY)) return false;

                var dx = newX - oldX;
                var dy = newY - oldY;
                var dir = directionFromDelta(dx, dy);

                if (dir == null) return false;

                var nearObjects = GetObjectsNear(oldX, oldY)[dir.Value];
                var query = new QueryCache(nearObjects, engine);

                var pushAtDest = query.GetUnitsWithEffect("push");

                if (query.GetUnitsWithEffect("stop").Except(pushAtDest).Any()) return false;

                if (pushAtDest.Any())
                {
                    if (!canMove(newX, newY, newX + dx, newY + dy))
                        return false;
                    newIntentsToMove.AddRange(pushAtDest.Select(p => (newX, newY, newX + dx, newY + dy, p)));
                }

                var stickyAtDest = query.GetUnitsWithEffect("sticky");
                if (stickyAtDest.Any())
                {
                    if (!canMove(newX, newY, newX + dx, newY + dy))
                        return false;
                    foreach (var s in stickyAtDest)
                    {
                        canMove(s.X, s.Y, s.X + dx, s.Y + dy);
                    }
                    newIntentsToMove.AddRange(stickyAtDest.Select(p => (newX, newY, newX + dx, newY + dy, p)));
                }

                return true;
            }

            foreach (var (ox, oy, nx, ny, obj) in intentsToMove.ToList())
            {
                if (canMove(ox, oy, nx, ny))
                {
                    moveObjectInGridAndWorld(ox, oy, nx, ny, obj);
                }
                else if (cache.GetUnitsWithEffect("move").Any(j => j == obj))
                {
                    obj.AboutFace();
                }
                else
                {
                    obj.FaceDirection(directionFromDelta(nx - ox, ny - oy));
                }
            }
            foreach (var (ox, oy, nx, ny, obj) in newIntentsToMove.ToList())
            {
                moveObjectInGridAndWorld(ox, oy, nx, ny, obj);
            }

            intentsToMove.Clear();
            newIntentsToMove.Clear();


            #region Fear
            if (engine.FindFeature(null, "fear", null).Any())
            {
                var fear = cache.GetUnitVerbTargets("fear");
                var scaryNear = fear.Where(pair => Math.Abs(pair.b.X - pair.a.X) + Math.Abs(pair.b.Y - pair.a.Y) == 1).ToList();

                if (scaryNear.Any())
                {
                    foreach (var (aah, boo) in scaryNear)
                    {
                        if (boo.X == aah.X) intentsToMove.Add((aah.X, aah.Y, aah.X, aah.Y + (aah.Y - boo.Y), aah)); // aah.MoveY(aah.Y - boo.Y);
                        else if (boo.Y == aah.Y) intentsToMove.Add((aah.X, aah.Y, aah.X + (aah.X - boo.X), aah.Y, aah)); //aah.MoveX(aah.X - boo.X);
                    }
                }
            }
            #endregion

            #region Shift
            foreach (var shift in cache.GetUnitsWithEffect("shift"))
            {
                foreach (var shifted in AllObjects.Where(obj => obj.X == shift.X && obj.Y == shift.Y && obj != shift))
                {
                    intentsToMove.Add(directionMoveObject(shifted, shift.Facing));
                }
            }
            #endregion


            foreach (var (ox, oy, nx, ny, obj) in intentsToMove.ToList())
            {
                if (canMove(ox, oy, nx, ny))
                {
                    moveObjectInGridAndWorld(ox, oy, nx, ny, obj);
                }
            }
            foreach (var (ox, oy, nx, ny, obj) in newIntentsToMove.ToList())
            {
                moveObjectInGridAndWorld(ox, oy, nx, ny, obj);
            }

            if (action != "wait")
            {
                foreach (var you in cache.GetUnitsWithEffect("you"))
                {
                    if (you.MapData != this)
                    {
                        EventChannels.MapChange.SendAsyncMessage(new MapChange
                        {
                            X = you.MapData.MapX,
                            Y = you.MapData.MapY,
                            Direction = directionFromDelta(you.MapData.MapX - MapX, you.MapData.MapY - MapY),
                        });
                    }
                }
            }
        }

        private void moveObjectInGridAndWorld(int oldX, int oldY, int newX, int newY, BaseObject obj)
        {
            obj.SetX(newX);
            obj.SetY(newY);

            if (IsObjectOutOfBounds(obj))
            {
                // move the object
                AllObjects.Remove(obj);
                if (world.GetNeighborByTileCoord(this, newX, newY) is MapData neighbor)
                {
                    neighbor.AddObject(obj);
                    obj.SetX(newX);
                    obj.SetY(newY);
                }
            }
        }

        private (int, int, int, int, BaseObject) directionMoveObject(BaseObject obj, Direction direction)
        {
            return direction switch
            {
                Direction.Up => (obj.X, obj.Y, obj.X, obj.Y - 1, obj),
                Direction.Down => (obj.X, obj.Y, obj.X, obj.Y + 1, obj),
                Direction.Left => (obj.X, obj.Y, obj.X - 1, obj.Y, obj),
                Direction.Right => (obj.X, obj.Y, obj.X + 1, obj.Y, obj),
                _ => (0, 0, 0, 0, obj),
            };
        }

        private Direction? directionFromDelta(int dx, int dy)
        {
            if (dx < 0) return Direction.Left;
            if (dx > 0) return Direction.Right;
            if (dy < 0) return Direction.Up;
            if (dy > 0) return Direction.Down;
            return null;
        }

        public void DoJoinable()
        {
            foreach (var obj in AllObjects)
            {
                JoinableObjectUpdate(obj);
            }
        }

        public void JoinableObjectUpdate(BaseObject obj)
        {
            if (obj?.Joinable == true)
            {
                var flag = 0;
                var near = GetObjectsNear(obj.X, obj.Y);
                if (near[Direction.Up].Any(o => o.Name == obj.Name) == true) flag += 2;
                if (near[Direction.Right].Any(o => o.Name == obj.Name) == true) flag += 1;
                if (near[Direction.Down].Any(o => o.Name == obj.Name) == true) flag += 8;
                if (near[Direction.Left].Any(o => o.Name == obj.Name) == true) flag += 4;
                obj.SetJoinWithNeighbors(flag.ToString());
            }
        }


        class QueryCache
        {
            public List<BaseObject> SearchThese { get; }
            public WordEngine Engine { get; }
            public Dictionary<string, List<BaseObject>> Cache { get; }

            public QueryCache(List<BaseObject> searchThese, WordEngine engine)
            {
                SearchThese = searchThese;
                Engine = engine;
                Cache = new Dictionary<string, List<BaseObject>>();
            }

            public List<BaseObject> GetUnitsWithEffect(string effect)
            {
                if (Cache.ContainsKey(effect)) 
                    return Cache[effect];

                Cache[effect] = new List<BaseObject>();

                var group = Engine.FindFeature(null, "is", effect).Where(item => !ContainsNoun(BriefNounList, item.TargetName));

                IEnumerable<BaseObject> iter()
                {
                    foreach (var item in group)
                    {
                        foreach (var unit in GetAllUnitsOfType(item.TargetName, item.TargetNot))
                        {
                            if (_testCondition(item.TargetCondition, unit, item.TargetNot) && !unit.Dead)
                            {
                                yield return unit;
                            }
                        }
                    }
                }

                var results = iter().ToList();
                Cache[effect].AddRange(results);
                return results;
            }
            
            public IEnumerable<(BaseObject a, BaseObject b)> GetUnitVerbTargets(string verb)
            {
                if (Engine.FeatureIndex.ContainsKey(verb))
                {
                    foreach (var feat in Engine.FeatureIndex[verb])
                    {
                        if (feat.TargetCondition?.FullModifier != "never" && feat.TargetName != "empty")
                        {
                            foreach (var item in GetAllUnitsOfType(feat.TargetName, feat.TargetNot))
                            {
                                if (_testCondition(feat.TargetCondition, item, feat.TargetNot)) { 
                                    foreach (var scary in GetAllUnitsOfType(feat.Feature, feat.FeatureNot))
                                        yield return (item, scary);
                                }

                            }
                        }
                    }
                }
            }


            public IEnumerable<BaseObject> GetAllUnitsOfType(string type, bool not = false)
            {
                if (Cache.ContainsKey(type))
                    return Cache[type];

                IEnumerable<BaseObject> iter()
                {
                    if (type == "empty")
                    {

                    }
                    else if (type == "all")
                    {

                    }
                    else if (type == "level")
                    {

                    }
                    else if (type == "group")
                    {
                        foreach (var memberKind in Engine.FindFeature(null, "is", "group"))
                        {
                            foreach (var unit in GetAllUnitsOfType(memberKind.TargetName, not)
                                .Where(obj => _testCondition(memberKind.TargetCondition, obj, memberKind.TargetNot)))
                            {
                                yield return unit;
                            }
                        }
                    }
                    else
                    {
                        foreach (var unit in SearchThese)
                        {
                            if (unit.Type == ObjectType.Text ? type == "text" : (unit.Name == type) != not)
                                yield return unit;
                        }
                    }
                }
                return iter();
            }

            private bool _testCondition(TargetCondition? cond, BaseObject obj, bool targetNot, IEnumerable<BaseObject>? ignore = null)
            {
                if (cond == null) return true;
                return false;
            }
        }
    }
}
