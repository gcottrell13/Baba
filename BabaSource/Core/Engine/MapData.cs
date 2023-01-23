﻿using Core.Content;
using Core.Utils;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Engine
{
    public class MapData
    {
        public ObjectData[] WorldObjects { get; private set; }
        private Stack<int> pointers = new();
        public short MapId;
        public short northNeighbor;
        public short eastNeighbor;
        public short southNeighbor;
        public short westNeighbor;

        public short upLayer;
        public short region;

        public string Name = string.Empty;

        public MapData()
        {
            WorldObjects = Array.Empty<ObjectData>();
        }


        public MapData(ObjectData[] data)
        {
            WorldObjects = data;
            pointers.Push(0);
        }

        public override bool Equals(object? obj)
        {
            if (obj is MapData map)
                return WorldObjects.Compare(map.WorldObjects) && MapId == map.MapId && northNeighbor == map.northNeighbor && 
                    westNeighbor == map.westNeighbor && map.eastNeighbor == eastNeighbor && southNeighbor == map.southNeighbor && 
                    upLayer == map.upLayer && region == map.region && Name == map.Name;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => $$"""
            new MapData(new ObjectData[] { {{string.Join(",\n", WorldObjects.Select(x => "\n" + x.ToString().Indent(1)))}} }) {
                MapId = {{MapId}},
                Name = "{{Name}}",
                northNeighbor = {{northNeighbor}},
                southNeighbor = {{southNeighbor}},
                eastNeighbor = {{eastNeighbor}},
                westNeighbor = {{westNeighbor}},
                upLayer = {{upLayer}},
                region = {{region}},
            }
            """;

        public int AddObject(ObjectData obj)
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

                pointer = (pointer + 1) % WorldObjects.Length;
                if (pointers.Contains(pointer)) pointers.TryPop(out pointer);
            }

            // ran out of space, allocate more
            var currentObjects = WorldObjects;
            WorldObjects = new ObjectData[currentObjects.Length * 2];
            pointer = currentObjects.Length;
            WorldObjects[pointer] = obj;
            obj.index = pointer;
            currentObjects.CopyTo(WorldObjects, 0);
            return pointer;
        }

        public string Serialize()
        {
            return SerializeBytes.JoinSerializedStrings(SerializeBytes.SerializeObjects(new[] { this }), SerializeBytes.SerializeObjects(WorldObjects));
        }

        public static MapData Deserialize(string str)
        {
            var parts = SerializeBytes.SplitSerializedStrings(str);
            var map = SerializeBytes.DeserializeObjects<MapData>(parts[0])[0];
            var data = SerializeBytes.DeserializeObjects<ObjectData>(parts[1]);

            map.WorldObjects = data.ToArray();

            return map;
        }
    }
}