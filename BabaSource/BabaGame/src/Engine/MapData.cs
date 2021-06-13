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

        public MapData(string mapName, WordEngine engine, WorldStructure world)
        {
            tiledMap = AllMaps.GetMapHandle(mapName);

            WorldVariables.TileWidth = tiledMap.TileWidth;
            WorldVariables.TileHeight = tiledMap.TileHeight;

            gridx = tiledMap.Width;
            gridy = tiledMap.Height;

            AllObjects = new List<BaseObject>();

            this.engine = engine;
            this.world = world;
        }

        public IEnumerable<BaseObject> Initialize()
        {
            var objects = new List<BaseObject>();

            foreach (var layer in tiledMap.TileLayers)
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
                            var f = new BaseObject(name, tile.X, tile.Y);
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

                            var f = new BaseObject(name, tile.X, tile.Y, direction);
                            objects.Add(f);
                            AddObject(f);
                            f.Graphics.zindex = stats.layer;
                        }
                    }
                }
            }

            foreach (var obj in AllObjects)
            {
                if (obj.Joinable)
                {
                    var flag = 0;
                    var near = GetObjectsNear(obj);
                    if (near[Direction.Up].Any(o => o.Name == obj.Name) == true) flag += 2;
                    if (near[Direction.Right].Any(o => o.Name == obj.Name) == true) flag += 1;
                    if (near[Direction.Down].Any(o => o.Name == obj.Name) == true) flag += 8;
                    if (near[Direction.Left].Any(o => o.Name == obj.Name) == true) flag += 4;
                    obj.SetJoinWithNeighbors(flag.ToString());
                }
            }

            return objects;
        }

        public Dictionary<Direction, List<BaseObject>> GetObjectsNear(BaseObject obj)
        {
            var near = new Dictionary<Direction, List<BaseObject>>();
            near[Direction.Up] = AllObjects.Where(o => o.X == obj.X && o.Y == obj.Y - 1).Concat(world.ObjectsAt(this, obj.X, obj.Y - 1)).ToList();
            near[Direction.Down] = AllObjects.Where(o => o.X == obj.X && o.Y == obj.Y + 1).Concat(world.ObjectsAt(this, obj.X, obj.Y + 1)).ToList();
            near[Direction.Left] = AllObjects.Where(o => o.X == obj.X - 1 && o.Y == obj.Y).Concat(world.ObjectsAt(this, obj.X - 1, obj.Y)).ToList();
            near[Direction.Right] = AllObjects.Where(o => o.X == obj.X + 1 && o.Y == obj.Y).Concat(world.ObjectsAt(this, obj.X + 1, obj.Y)).ToList();
            return near;
        }

        public void AddObject(BaseObject obj)
        {
            if (AllObjects.Contains(obj))
            {
                throw new Exception();
            }
            AllObjects.Add(obj);
        }

        public void TakeAction(string action)
        {
            var intentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();
            var newIntentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();

            foreach (var you in _getUnitsWithEffect("you"))
                intentsToMove.Add(directionMoveObject(you, action[0]));

            foreach (var move in _getUnitsWithEffect("move"))
                intentsToMove.Add(directionMoveObject(move, move.Facing));

            bool canMove(int oldX, int oldY, int newX, int newY)
            {
                if (oldX == newX && oldY == newY) return false;

                if (newX >= gridx || newY >= gridy || newX < 0 || newY < 0) return false;

                if (_getUnitsWithEffect("stop").Any(obj => obj.X == newX && obj.Y == newY)) return false;

                var dx = newX - oldX;
                var dy = newY - oldY;

                var pushAtDest = _getUnitsWithEffect("push").Where(a => a.X == newX && a.Y == newY).ToList();
                if (pushAtDest.Any())
                {
                    if (!canMove(newX, newY, newX + dx, newY + dy))
                        return false;
                    newIntentsToMove.AddRange(pushAtDest.Select(p => (newX, newY, newX + dx, newY + dy, p)));
                }

                var stickyAtDest = _getUnitsWithEffect("sticky").Where(a => a.X == newX && a.Y == newY).ToList();
                if (stickyAtDest.Any())
                {
                    if (!canMove(newX, newY, newX + dx, newY + dy))
                        return false;
                    foreach (var s in _getUnitsWithEffect("sticky").Where(a => Math.Abs(a.X - newX) != Math.Abs(a.Y - newY)))
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
                else if (_getUnitsWithEffect("move").Any(j => j == obj))
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

            intentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();
            newIntentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();


            #region Scary / Scared
            //if (_getUnitsWithEffect("scary").ToList() is List<BaseObject> b && b.Any())
            //{

            //    var scared = getCache(cache, "scared");
            //    var scary = getCache(cache, "scary");
            //    var scaryNear = (
            //        from aah in scared
            //        from boo in scary
            //        select new { boo = boo.Object, aah = aah.Object }).Where(pair => Math.Abs(pair.boo.X - pair.aah.X) + Math.Abs(pair.boo.Y - pair.aah.Y) == 1).ToList();

            //    if (scaryNear.Any())
            //    {
            //        foreach (var s in scaryNear)
            //        {
            //            var boo = s.boo;
            //            var aah = s.aah;
            //            if (boo.X == aah.X) intentsToMove.Add((aah.X, aah.Y, aah.X, aah.Y + (aah.Y - boo.Y), aah)); // aah.MoveY(aah.Y - boo.Y);
            //            else if (boo.Y == aah.Y) intentsToMove.Add((aah.X, aah.Y, aah.X + (aah.X - boo.X), aah.Y, aah)); //aah.MoveX(aah.X - boo.X);
            //        }
            //    }
            //}
            #endregion

            #region Shift
            foreach (var shift in _getUnitsWithEffect("shift"))
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

            //foreach (var obj in Objects.Where(o => o.Joinable))
            //{
            //    var flag = 0;
            //    if (obj.Y > 0 && Grid[obj.X, obj.Y - 1]?.Any(o => o.Name == obj.Name) == true) flag += 2;
            //    if (obj.X < gridx - 1 && Grid[obj.X + 1, obj.Y]?.Any(o => o.Name == obj.Name) == true) flag += 1;
            //    if (obj.Y < gridy - 1 && Grid[obj.X, obj.Y + 1]?.Any(o => o.Name == obj.Name) == true) flag += 8;
            //    if (obj.X > 0 && Grid[obj.X - 1, obj.Y]?.Any(o => o.Name == obj.Name) == true) flag += 4;
            //    obj.SetJoinWithNeighbors(flag.ToString());
            //}
        }

        private void moveObjectInGridAndWorld(int oldX, int oldY, int newX, int newY, BaseObject obj)
        {
            obj.SetX(newX);
            obj.SetY(newY);
        }

        private (int, int, int, int, BaseObject) directionMoveObject(BaseObject obj, char direction)
        {
            if (direction == 'u')
            {
                return (obj.X, obj.Y, obj.X, obj.Y - 1, obj);
            }
            else if (direction == 'd')
            {
                return (obj.X, obj.Y, obj.X, obj.Y + 1, obj);
            }
            else if (direction == 'l')
            {
                return (obj.X, obj.Y, obj.X - 1, obj.Y, obj);
            }
            else if (direction == 'r')
            {
                return (obj.X, obj.Y, obj.X + 1, obj.Y, obj);
            }
            return (0, 0, 0, 0, obj);
        }

        private char directionFromDelta(int dx, int dy)
        {
            if (dx < 0) return 'l';
            if (dx > 0) return 'r';
            if (dy < 0) return 'u';
            if (dy > 0) return 'd';
            return ' ';
        }

        private IEnumerable<BaseObject> _getUnitsWithEffect(string property, IEnumerable<BaseObject>? ignore = null)
        {
            var group = engine.FindFeature(null, "is", property).Where(item => !ContainsNoun(BriefNounList, item.TargetName));

            foreach (var item in group)
            {
                if (item.TargetName != "empty")
                {
                    foreach (var unit in GetAllUnitsOfType(item.TargetName))
                    {
                        if (_testCondition(item.TargetCondition, unit) && !unit.Dead && (ignore?.Contains(unit) ?? true))
                        {
                            yield return unit;
                        }
                    }
                }
            }
        }

        private bool _testCondition(TargetCondition? conds, BaseObject obj, IEnumerable<BaseObject>? ignore = null)
        {
            if (conds == null) 
                return true;
            return false;
        }

        public IEnumerable<BaseObject> GetAllUnitsOfType(string type)
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
                foreach (var memberKind in engine.FindFeature(null, "is", "group"))
                {
                    foreach (var unit in GetAllUnitsOfType(memberKind.TargetName).Where(obj => _testCondition(memberKind.TargetCondition, obj))) {
                        yield return unit;
                    }
                }
            }
            else
            {
                foreach (var unit in AllObjects)
                {
                    if (unit.Type == ObjectType.Text ? type == "text" : unit.Name == type)
                        yield return unit;
                }
            }
        }


        struct CacheItem
        {
            public BaseObject Object;
            public int BeginningX;
            public int BeginningY;
        }
    }
}
