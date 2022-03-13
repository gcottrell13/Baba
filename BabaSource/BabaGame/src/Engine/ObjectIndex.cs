using BabaGame.src.Objects;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabaGame.src.Engine
{
    public class ObjectIndex : Dictionary<string, HashSet<BaseObject>>
    {
        public void IndexObject(BaseObject obj)
        {
            foreach(var key in PossibleObjectIndexes(obj))
                this.ConstructDefaultValue(key).Add(obj);
        }

        public void RemoveObject(BaseObject obj)
        {
            foreach (var key in PossibleObjectIndexes(obj))
            {
                if (TryGetValue(key, out var set))
                {
                    set.Remove(obj);
                    if (set.Count == 0)
                    {
                        Remove(key);
                    }
                }
            }
        }

        public static IEnumerable<string> PossibleObjectIndexes(BaseObject obj)
        {
            return new[] { obj.Name, obj.Color };
        }
    }
}
