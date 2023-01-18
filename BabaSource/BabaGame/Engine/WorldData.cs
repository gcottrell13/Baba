using Core.Content;
using Core.Utils;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabaGame.Engine
{
    public class WorldData
    {
        public WorldObject[] WorldObjects { get; private set; }
        private Stack<uint> pointers = new();

        public WorldData(int size = 0)
        {
            WorldObjects = new WorldObject[size];
            pointers.Push(0);
        }

        public WorldData(WorldObject[] data)
        {
            WorldObjects = data;
            pointers.Push(0);
        }

        public uint AddObject(WorldObject obj)
        {
            var initialPointer = pointers.Pop();
            var pointer = initialPointer;
            while (pointers.Count > 0)
            {
                if (!WorldObjects[pointer].Occupied)
                {
                    WorldObjects[pointer] = obj;
                    obj.Occupied = true;
                    obj.index = pointer;
                    return pointer;
                }

                pointer = (pointer + 1) % (uint)WorldObjects.Length;
                if (pointers.Contains(pointer)) pointers.TryPop(out pointer);
            }

            // ran out of space, allocate more
            var currentObjects = WorldObjects;
            WorldObjects = new WorldObject[currentObjects.Length * 2];
            pointer = (uint)currentObjects.Length;
            WorldObjects[pointer] = obj;
            obj.index = pointer;
            currentObjects.CopyTo(WorldObjects, 0);
            return pointer;
        }

        public string Serialize()
        {
            var bytes = new List<byte>();
            foreach (WorldObject obj in WorldObjects)
            {
                if (!obj.Occupied) continue;

                bytes.AddRange(BitConverter.GetBytes((short)obj.x));
                bytes.AddRange(BitConverter.GetBytes((short)obj.y));
                bytes.AddRange(BitConverter.GetBytes((short)obj.Facing));
                bytes.AddRange(BitConverter.GetBytes((short)obj.Color));
                bytes.AddRange(BitConverter.GetBytes((short)obj.Kind));
                bytes.AddRange(BitConverter.GetBytes((short)obj.ObjectId));
            }

            return Encoding.BigEndianUnicode.GetString(bytes.ToArray());
        }

        public static WorldData Deserialize(string str)
        {
            var bytes = Encoding.BigEndianUnicode.GetBytes(str);
            var data = new List<WorldObject>();
            var intSize = sizeof(short);
            for (var i = 0; i < bytes.Length; i += intSize * 6)
            {
                var d = new WorldObject()
                {
                    Occupied = true,
                    x = (uint)BitConverter.ToInt16(bytes, i),
                    y = (uint)BitConverter.ToInt16(bytes, i + intSize),
                    Facing = (Direction)BitConverter.ToInt16(bytes, i + intSize * 2),
                    Color = (WorldObjectColor)BitConverter.ToInt16(bytes, i + intSize * 3),
                    Kind = (WorldObjectKind)BitConverter.ToInt16(bytes, i + intSize * 4),
                    ObjectId = BitConverter.ToInt16(bytes, i + intSize * 5),
                };
                d.Name = ObjectInfo.IdToName[d.ObjectId];
                data.Add(d);
            }
            return new(data.ToArray());
        }
    }
}
