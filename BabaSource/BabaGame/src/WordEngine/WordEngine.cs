using BabaGame.src.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src.WordEngine
{


    public class WordEngine
    {
        private Dictionary<string, SelectorType[]> _properties; // indexed by property
        private List<string[]> _phrases;
        private List<BaseObject> objects;

        public WordEngine()
        {
            objects = new List<BaseObject>();
            _phrases = new List<string[]>
            {
                new[] { "baba", "is", "you" }
            };
            _properties = new Dictionary<string, SelectorType[]>
            {
                { "you", new []{ nounSelect("baba"), nounSelect("me") } },
                { "stop", new[]{ nounSelect("wall") } }
            };
        }

        public void AddObject(BaseObject obj)
        {
            if (objects.Contains(obj))
            {
                throw new Exception();
            }
            objects.Add(obj);
        }

        public void TakeAction(string action)
        {
            // query for all "you" objects
            foreach (var you in queryProperty("you")) you.Move(action[0]);

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
    }
}
