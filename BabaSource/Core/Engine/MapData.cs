using Core.Content;
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
        public List<ObjectData> WorldObjects { get; private set; } = new();

        public short MapId;
        public short northNeighbor;
        public short eastNeighbor;
        public short southNeighbor;
        public short westNeighbor;

        public short upLayer;
        public short region;

        public short width;
        public short height;

        public string Name = string.Empty;

        public MapData(IEnumerable<ObjectData> objectDatas)
        {
            WorldObjects.AddRange(objectDatas);
        }

        public MapData()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is MapData map)
                return WorldObjects.Compare(map.WorldObjects) && MapId == map.MapId && northNeighbor == map.northNeighbor && 
                    westNeighbor == map.westNeighbor && map.eastNeighbor == eastNeighbor && southNeighbor == map.southNeighbor && 
                    upLayer == map.upLayer && region == map.region && Name == map.Name && map.width == width && map.height == height;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => $$"""
            new MapData(new ObjectData[] { {{string.Join(",\n", WorldObjects.Select(x => "\n" + x.ToString().Indent(1)))}} }) {
                {{nameof(MapId)}} = {{MapId}},
                {{nameof(Name)}} = "{{Name}}",
                {{nameof(northNeighbor)}} = {{northNeighbor}},
                {{nameof(southNeighbor)}} = {{southNeighbor}},
                {{nameof(eastNeighbor)}} = {{eastNeighbor}},
                {{nameof(westNeighbor)}} = {{westNeighbor}},
                {{nameof(upLayer)}} = {{upLayer}},
                {{nameof(region)}} = {{region}},
                {{nameof(width)}} = {{width}},
                {{nameof(height)}} = {{height}},
            }
            """;


        public string Serialize()
        {
            return SerializeBytes.JoinSerializedStrings(SerializeBytes.SerializeObjects(new[] { this }), SerializeBytes.SerializeObjects(WorldObjects));
        }

        public static MapData Deserialize(string str)
        {
            var parts = SerializeBytes.SplitSerializedStrings(str);
            var map = SerializeBytes.DeserializeObjects<MapData>(parts[0])[0];
            var data = SerializeBytes.DeserializeObjects<ObjectData>(parts[1]);

            map.WorldObjects = data.ToList();

            return map;
        }
    }
}
