using Core.Content;
using Core.Utils;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Engine
{
    public class ScreenData
    {
        public short ScreenId;
        public short RegionId;

        public short northNeighborId;
        public short eastNeighborId;
        public short southNeighborId;
        public short westNeighborId;

        public short width;
        public short height;

        public int x;
        public int y;

        public string Name = string.Empty;

        // has the player seen this map yet?
        public bool visited = false;

        public override bool Equals(object? obj)
        {
            if (obj is ScreenData map)
                return ScreenId == map.ScreenId && northNeighborId == map.northNeighborId && 
                    westNeighborId == map.westNeighborId && map.eastNeighborId == eastNeighborId && southNeighborId == map.southNeighborId && visited == map.visited &&
                    RegionId == map.RegionId && Name == map.Name && map.width == width && map.height == height;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => $$"""
            new {{nameof(ScreenData)}}() {
                {{nameof(ScreenId)}} = {{ScreenId}},
                {{nameof(Name)}} = "{{Name}}",
                {{nameof(northNeighborId)}} = {{northNeighborId}},
                {{nameof(southNeighborId)}} = {{southNeighborId}},
                {{nameof(eastNeighborId)}} = {{eastNeighborId}},
                {{nameof(westNeighborId)}} = {{westNeighborId}},
                {{nameof(RegionId)}} = {{RegionId}},
                {{nameof(width)}} = {{width}},
                {{nameof(height)}} = {{height}},
                {{nameof(visited)}} = {{visited.ToString().ToLower()}},
            }
            """;


        public string Serialize()
        {
            return SerializeBytes.SerializeObjects(new[] { this });
        }

        public static ScreenData Deserialize(string str)
        {
            var parts = SerializeBytes.SplitSerializedStrings(str);
            var map = SerializeBytes.DeserializeObjects<ScreenData>(parts[0])[0];
            var data = SerializeBytes.DeserializeObjects<ObjectData>(parts[1]);
            return map;
        }
    }
}
