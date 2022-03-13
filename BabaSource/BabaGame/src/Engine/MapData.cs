using BabaGame.src.Events;
using BabaGame.src.Objects;
using BabaGame.src.Resources;
using Core.Utils;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static BabaGame.src.Engine.WordEngine;

namespace BabaGame.src.Engine
{
    [DebuggerDisplay("{MapName} ({MapX} ,{MapY})")]
    public class MapData
    {
        public List<BaseObject> AllObjects;
        public ObjectIndex ObjectIndex;

        private WordEngine WorldWordEngine;
        private WordEngine ThisMapEngine;
        private readonly World world;
        private TiledMap? tiledMap;

        public string MapName { get; }
        public int MapX { get; }
        public int MapY { get; }

        public MapData(string mapName, WordEngine engine, World world, int mapX, int mapY)
        {
            tiledMap = AllMaps.GetMapHandle(mapName);

            WorldVariables.TileWidth = tiledMap.TileWidth;
            WorldVariables.TileHeight = tiledMap.TileHeight;

            ObjectIndex = new ObjectIndex();

            AllObjects = new List<BaseObject>();
            MapName = mapName;
            WorldWordEngine = engine;
            ThisMapEngine = new WordEngine();
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
                            ThisMapEngine.AddRule(WordEngine.ParsePhrase(line.Trim()));
                        }
                    }
                }

            }

            foreach (var sentence in JsonValues.GlobalSentences.sentences)
            {
                ThisMapEngine.AddRule(WordEngine.ParsePhrase(sentence.Trim()));
            }

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
            return AllObjects.Where(o => o.TileX == x && o.TileY == y);
        }

        public IEnumerable<BaseObject> OutOfBoundsObjects()
        {
            return AllObjects.Where(obj => IsObjectOutOfBounds(obj.TileX, obj.TileY) != null);
        }

        public Direction? IsObjectOutOfBounds(int x, int y)
        {
            if (x < WorldVariables.MapWidth * MapX) return Direction.Left;
            else if (x >= WorldVariables.MapWidth * (MapX + 1)) return Direction.Right;
            else if (y < WorldVariables.MapHeight * MapY) return Direction.Up;
            else if (y >= WorldVariables.MapHeight * (MapY + 1)) return Direction.Down;
            return null;
        }

        public void AddObject(BaseObject obj)
        {
            if (AllObjects.Contains(obj))
            {
                throw new Exception();
            }
            obj.SetMap(this);
            AllObjects.Add(obj);
            ObjectIndex.IndexObject(obj);
        }

        public void RemoveObject(BaseObject obj)
        {
            ObjectIndex.RemoveObject(obj);
        }

        public void TakeAction(string action)
        {
            var intentsToMove = new List<(int oldX, int oldY, Direction dir, BaseObject obj)>();
            var newIntentsToMove = new List<(int oldX, int oldY, Direction dir, BaseObject obj)>();

            var cache = new QueryCache(AllObjects, ThisMapEngine);

            if (action != "wait")
            {
                foreach (var you in cache.GetUnitsWithEffect("you"))
                    intentsToMove.Add(directionMoveObject(you, DirectionExtensions.FromString(action)));
            }

            foreach (var move in cache.GetUnitsWithEffect("move"))
                intentsToMove.Add(directionMoveObject(move, move.Facing));

            var stickyObjectsMoving = new List<BaseObject>();

            bool canMove(int oldX, int oldY, Direction dir, MapData currentMap)
            {
                if (dir == Direction.None)
                    return false;

                var allNearObjects = GetObjectsNear(oldX, oldY);
                var nearObjectsInDir = allNearObjects[dir];
                var (newX, newY) = GetTileCoordInDirection(oldX, oldY, dir);

                if (newX == oldX && newY == oldY)
                    return false;

                var query = new QueryCache(nearObjectsInDir, currentMap.ThisMapEngine);

                var pushAtDest = query.GetUnitsWithEffect("push");

                if (query.GetUnitsWithEffect("stop").Except(pushAtDest).Any()) 
                    return false;

                if (pushAtDest.Any())
                {
                    if (!canMove(newX, newY, dir, currentMap))
                        return false;

                    newIntentsToMove.AddRange(pushAtDest.Select(p => (newX, newY, dir, p)));
                }

                var stickyAtDest = query.GetUnitsWithEffect("sticky");
                if (stickyAtDest.Any())
                {
                    if (!canMove(newX, newY, dir, currentMap))
                        return false;
                    var s = stickyAtDest.Select(p => (newX, newY, dir, p));
                    newIntentsToMove.AddRange(s);
                    foreach (var stick in stickyAtDest)
                    {
                        if (stickyObjectsMoving.Contains(stick) == false) stickyObjectsMoving.Add(stick);
                    }

                    var sticky = new QueryCache(GetObjectsNear(newX, newY).SelectMany(k => k.Value).ToList(), currentMap.ThisMapEngine);
                    foreach (var ss in sticky.GetUnitsWithEffect("sticky").Except(stickyObjectsMoving))
                    {
                        canMove(ss.TileX, ss.TileY, dir, currentMap);
                    }
                }

                return true;
            }

            foreach (var (ox, oy, dir, obj) in intentsToMove.ToList())
            {
                if (canMove(ox, oy, dir, this))
                {
                    moveObjectInGridAndWorld(ox, oy, dir, obj);
                }
                else if (cache.GetUnitsWithEffect("move").Any(j => j == obj))
                {
                    obj.AboutFace();
                }
                else
                {
                    obj.FaceDirection(dir);
                }
            }
            foreach (var (ox, oy, dir, obj) in newIntentsToMove.ToList())
            {
                moveObjectInGridAndWorld(ox, oy, dir, obj);
            }

            intentsToMove.Clear();
            newIntentsToMove.Clear();


            #region Fear
            if (ThisMapEngine.FindFeature(null, "fear", null).Any())
            {
                var fear = cache.GetUnitVerbTargets("fear");
                var scaryNear = fear.Where(pair => Math.Abs(pair.b.TileX - pair.a.TileX) + Math.Abs(pair.b.TileY - pair.a.TileY) == 1).ToList();

                if (scaryNear.Any())
                {
                    foreach (var (aah, boo) in scaryNear)
                    {
                        if (boo.TileX == aah.TileX) intentsToMove.Add((aah.TileX, aah.TileY, DirectionExtensions.DirectionFromDelta((0, aah.TileY - boo.TileY)), aah)); // aah.MoveY(aah.Y - boo.Y);
                        else if (boo.TileY == aah.TileY) intentsToMove.Add((aah.TileX, aah.TileY, DirectionExtensions.DirectionFromDelta((aah.TileX - boo.TileX, 0)), aah)); //aah.MoveX(aah.X - boo.X);
                    }
                }
            }
            #endregion

            #region Shift
            foreach (var shift in cache.GetUnitsWithEffect("shift"))
            {
                foreach (var shifted in AllObjects.Where(obj => obj.TileX == shift.TileX && obj.TileY == shift.TileY && obj != shift))
                {
                    intentsToMove.Add(directionMoveObject(shifted, shift.Facing));
                }
            }
            #endregion


            foreach (var (ox, oy, dir, obj) in intentsToMove.ToList())
            {
                if (canMove(ox, oy, dir, this))
                {
                    moveObjectInGridAndWorld(ox, oy, dir, obj);
                }
            }
            foreach (var (ox, oy, dir, obj) in newIntentsToMove.ToList())
            {
                moveObjectInGridAndWorld(ox, oy, dir, obj);
            }

            foreach (var you in cache.GetUnitsWithEffect("you"))
            {
                if (you.MapData != this)
                {
                    EventChannels.MapChange.SendAsyncMessage(new MapChange
                    {
                        X = you.MapData.MapX,
                        Y = you.MapData.MapY,
                        Direction = world.GetDirectionOfNeighbor(this, you.MapData),
                    });
                }
            }
        }

        private void moveObjectInGridAndWorld(int oldX, int oldY, Direction dir, BaseObject obj)
        {
            var (newX, newY) = GetTileCoordInDirection(oldX, oldY, dir);
            if (obj.MapData == this && IsObjectOutOfBounds(newX, newY) is Direction _)
            {
                // move the object
                if (world.GetNeighbor(this, dir) is MapData neighbor)
                {
                    world.MoveObjectToMap(this, neighbor, obj);
                    obj.SetX(newX % WorldVariables.MapWidth + neighbor.MapX * WorldVariables.MapWidth);
                    obj.SetY(newY % WorldVariables.MapHeight + neighbor.MapY * WorldVariables.MapHeight);
                }
            }
            else
            {
                obj.SetX(newX);
                obj.SetY(newY);
            }
        }

        private (int, int, Direction, BaseObject) directionMoveObject(BaseObject obj, Direction direction)
        {
            return (obj.TileX, obj.TileY, direction, obj);
        }

        public (int x, int y) GetTileCoordInDirection(int x, int y, Direction dir)
        {
            var (newX, newY) = dir switch
            {
                Direction.Up => (x, y - 1),
                Direction.Down => (x, y + 1),
                Direction.Left => (x - 1, y),
                Direction.Right => (x + 1, y),
                _ => (x, y),
            };

            return IsObjectOutOfBounds(newX, newY) != null ? world.GetNeighbor(this, dir)?.AdjustTileCoord(newX, newY) ?? (x, y) : (newX, newY);
        }

        public void DoJoinable()
        {
            foreach (var obj in AllObjects)
            {
                JoinableObjectUpdate(obj);
            }
        }

        public (int x, int y) AdjustTileCoord(int x, int y)
        {
            return WorldVariables.TileCoordinateRebaseToNewMap(MapX, MapY, x, y);
        }

        public void JoinableObjectUpdate(BaseObject obj)
        {
            if (obj?.Joinable == true)
            {
                var flag = 0;
                var near = GetObjectsNear(obj.TileX, obj.TileY);
                if (near[Direction.Up].Any(o => o.Name == obj.Name) == true) flag += 2;
                if (near[Direction.Right].Any(o => o.Name == obj.Name) == true) flag += 1;
                if (near[Direction.Down].Any(o => o.Name == obj.Name) == true) flag += 8;
                if (near[Direction.Left].Any(o => o.Name == obj.Name) == true) flag += 4;
                obj.SetJoinWithNeighbors(flag.ToString());
            }
        }


        class QueryCache
        {
            public ObjectIndex SearchThese { get; }
            public WordEngine Engine { get; }
            public Dictionary<string, List<BaseObject>> Cache { get; }

            public QueryCache(ObjectIndex searchThese, WordEngine engine)
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
