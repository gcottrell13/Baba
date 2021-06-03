using BabaGame.src.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BabaGame.src.WordEngine
{
    public class WordEngine
    {
        private Dictionary<string, SelectorType[]> _properties; // indexed by property
        private List<string> _phrases;
        private List<BaseObject> objects;

        private BaseObject[,] grid;

        private static string[] adjectives = new[]
        {
            "you", "defeat",

            "sink", "weak",

            "stop", "push",

            "hot", "melt",
            "scary", "scared",
        };

        private static string[] operators = new[]
        {
            "is",
            "not",
            "and",
            "lonely",
            "on",
            "without",
            "eat",
        };
        private readonly int gridx;
        private readonly int gridy;

        public WordEngine(int gridx, int gridy)
        {
            objects = new List<BaseObject>();
            _phrases = new List<string>
            {
                "baba is you",
                "baba is scary",
                "me is scared",
            };
            _properties = new Dictionary<string, SelectorType[]>
            {
                { "you", new []{ nounSelect("baba") } },
                { "stop", new[]{ nounSelect("wall") } },
                { "scary", new[]{ nounSelect("baba") } },
                { "scared", new[]{ nounSelect("me") } },
                { "move", new[]{ nounSelect("me") } },
                { "push", new[]{ nounSelect("rock") } },
                { "shift", new[]{ nounSelect("belt") } },
            };

            grid = new BaseObject[gridx, gridy];
            this.gridx = gridx;
            this.gridy = gridy;
        }

        public void AddObject(BaseObject obj)
        {
            if (objects.Contains(obj))
            {
                throw new Exception();
            }
            objects.Add(obj);
            grid[obj.X, obj.Y] = obj;
        }

        public void TakeAction(string action)
        {
            var cache = _properties.Keys.ToDictionary(key => key, key => queryProperty(key).Select(obj => new
            {
                Object = obj,
                obj.X,
                obj.Y,
            }));

            IEnumerable<T> getCache<T>(Dictionary<string, IEnumerable<T>> c, string key) { return c.TryGetValue(key, out var i) ? i : new List<T>(); }

            var intentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();
            var newIntentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();

            foreach (var you in getCache(cache, "you"))
                intentsToMove.Add(directionMoveObject(you.Object, action[0]));

            foreach (var move in getCache(cache, "move"))
                intentsToMove.Add(directionMoveObject(move.Object, move.Object.Facing));

            bool canMove(int oldX, int oldY, int newX, int newY)
            {
                if (oldX == newX && oldY == newY) return false;

                if (newX >= gridx || newY >= gridy || newX < 0 || newY < 0) return false;

                if (cache["stop"].Any(obj => obj.X == newX && obj.Y == newY)) return false;

                var dx = newX - oldX;
                var dy = newY - oldY;

                var pushAtDest = getCache(cache, "push").Where(a => a.X == newX && a.Y == newY).ToList();
                if (pushAtDest.Any())
                {
                    if (!canMove(newX, newY, newX + dx, newY + dy))
                        return false;
                    newIntentsToMove.AddRange(pushAtDest.Select(p => (newX, newY, newX + dx, newY + dy, p.Object)));
                }

                var stickyAtDest = getCache(cache, "sticky").Where(a => a.X == newX && a.Y == newY).ToList();
                if (stickyAtDest.Any())
                {
                    if (!canMove(newX, newY, newX + dx, newY + dy))
                        return false;
                    foreach (var s in getCache(cache, "sticky").Where(a => Math.Abs(a.X - newX) != Math.Abs(a.Y - newY)))
                    {
                        canMove(s.X, s.Y, s.X + dx, s.Y + dy);
                    }
                    newIntentsToMove.AddRange(stickyAtDest.Select(p => (newX, newY, newX + dx, newY + dy, p.Object)));
                }

                return true;
            }

            foreach (var (ox, oy, nx, ny, obj) in intentsToMove.ToList())
            {
                if (canMove(ox, oy, nx, ny))
                {
                    obj.SetX(nx);
                    obj.SetY(ny);
                }
                else if (getCache(cache, "move").Any(j => j.Object == obj))
                {
                    obj.AboutFace();
                }
            }
            foreach (var (ox, oy, nx, ny, obj) in newIntentsToMove.ToList())
            {
                obj.SetX(nx);
                obj.SetY(ny);
            }

            intentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();
            newIntentsToMove = new List<(int oldX, int oldY, int newX, int newY, BaseObject obj)>();


            #region Scary / Scared
            if (cache.ContainsKey("scary")) {

                var scared = getCache(cache, "scared");
                var scary = getCache(cache, "scary");
                var scaryNear = (
                    from aah in scared
                    from boo in scary
                    select new { boo=boo.Object, aah=aah.Object }).Where(pair => Math.Abs(pair.boo.X - pair.aah.X) + Math.Abs(pair.boo.Y - pair.aah.Y) == 1).ToList();

                if (scaryNear.Any())
                {
                    foreach (var s in scaryNear)
                    {
                        var boo = s.boo;
                        var aah = s.aah;
                        if (boo.X == aah.X) intentsToMove.Add((aah.X, aah.Y, aah.X, aah.Y + (aah.Y - boo.Y), aah)); // aah.MoveY(aah.Y - boo.Y);
                        else if (boo.Y == aah.Y) intentsToMove.Add((aah.X, aah.Y, aah.X + (aah.X - boo.X), aah.Y, aah)); //aah.MoveX(aah.X - boo.X);
                    }
                }
            }
            #endregion

            #region Shift
            if (cache.ContainsKey("shift")) {
                foreach (var shift in getCache(cache, "shift"))
                {
                    foreach (var shifted in objects.Where(obj => obj.X == shift.X && obj.Y == shift.Y))
                    {
                        intentsToMove.Add(directionMoveObject(shifted, shift.Object.Facing));
                    }
                }
            }
            #endregion


            foreach (var (ox, oy, nx, ny, obj) in intentsToMove.ToList())
            {
                if (canMove(ox, oy, nx, ny))
                {
                    obj.SetX(nx);
                    obj.SetY(ny);
                }
            }
            foreach (var (ox, oy, nx, ny, obj) in newIntentsToMove.ToList())
            {
                obj.SetX(nx);
                obj.SetY(ny);
            }

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

        private IEnumerable<BaseObject> queryProperty(string property)
        {
            return objects.Where(obj => _properties[property].Any(s => s(obj, objects)));
        }

        private SelectorType nounSelect(string noun) => (obj, all) => obj.Name == noun;

        private Dictionary<string, SelectorType> selectorPieces = new Dictionary<string, SelectorType>()
        {
        };

        delegate bool SelectorType(BaseObject obj, List<BaseObject> allObjects);

        private class Effect
        {
            public BaseObject Target;
            public object Move;
            public object Die;
            public object Create;
            public object Particles;
            public object Turn; // without moving

        }



        private bool tryParsePhrase(IEnumerable<string> words, out SelectorType selector)
        {

            var first = words.First(); 
            switch (first)
            {
                case "not":

                    var next = words.Skip(1).First();

                    if (adjectives.Contains(next))
                    {
                        if (tryParsePhrase(words.Skip(1), out var inner))
                        {
                            selector = (obj, all) => !inner(obj, all);
                            return true;
                        }
                    }
                    else if (next == "lonely" || next == "without")
                    {
                        // the only 2 acceptable operators for 'not'

                    }
                    break;

            }

            selector = null;
            return false;
        }
    }
}
