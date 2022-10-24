using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.src.Engine
{
    public class WorldData
    {
        private WorldObject[] worldObjects;
        private uint pointer;

        public WorldData()
        {
            pointer = 0;
            worldObjects = new WorldObject[0];
        }

        public void SetState(WorldObject[] data)
        {
            worldObjects = data;
            pointer = 0;
        }

        public uint AddObject(WorldObject obj)
        {
            var initialPointer = pointer;
            while (++pointer != initialPointer)
            {
                if (pointer >= worldObjects.Length) pointer = 0;

                if (!worldObjects[pointer].Occupied)
                {
                    worldObjects[pointer] = obj;
                    obj.index = pointer;
                    return pointer;
                }
            }

            // ran out of space, allocate more
            var currentObjects = worldObjects;
            worldObjects = new WorldObject[currentObjects.Length * 2];
            pointer = (uint)currentObjects.Length;
            worldObjects[pointer] = obj;
            obj.index = pointer;
            return pointer;
        }

        public string Serialize()
        {
            var bytes = new List<byte>();
            foreach (WorldObject obj in worldObjects)
            {
                if (!obj.Occupied) continue;

                bytes.AddRange(BitConverter.GetBytes(obj.x));
                bytes.AddRange(BitConverter.GetBytes(obj.y));
                bytes.AddRange(BitConverter.GetBytes((int)obj.Facing));
                bytes.AddRange(BitConverter.GetBytes((int)obj.Color));
                bytes.AddRange(BitConverter.GetBytes((int)obj.Kind));
                bytes.AddRange(BitConverter.GetBytes(obj.Name));
            }

            return BitConverter.ToString(bytes.ToArray());
        }
    }
}
