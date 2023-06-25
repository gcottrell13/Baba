using Core.Content;
using Core.Engine;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves
{
    public static class CompileMap
    {
        [DebuggerDisplay("{originalMapId} -> {data.MapId}: {x} {y}")]
        private class MapTemp
        {
            internal int x;
            internal int y;
            internal MapData data;
            internal int originalMapId;
            internal int originalRegionId;

            public MapTemp(MapData data)
            {
                this.data = data;
            }
        }

        public static WorldData CompileWorld(SaveFormatWorld world)
        {
            var mapTemps = new List<MapTemp>();
            var mapMap = new Dictionary<int, short>();
            var regionMap = new Dictionary<int, short>();
            var regionDatas = new List<RegionData>();

            short mapTempId = 1;

            foreach (var instance in world.WorldLayout)
            {
                var data = world.MapDatas.FirstOrDefault(x => x.id == instance.mapDataId);
                if (data == null) continue;

                var md = fromMapLayer(data.layer1, instance.x, instance.y, instance.mapDataId, ref mapTempId);

                md.data.Name = data.name;
                md.originalRegionId = data.regionId;

                mapMap[instance.instanceId] = md.data.MapId;

                if (data.layer2.objects.Count > 0)
                {
                    var wordLayer = fromMapLayer(data.layer2, -1, -1, -1, ref mapTempId);
                    wordLayer.data.Name = $"{md.data.MapId} uplayer - {data.name}";
                    mapTemps.Add(wordLayer);
                    md.data.upLayer = wordLayer.data.MapId;
                }
                mapTemps.Add(md);
            }

            var globalMapIds = world.globalObjectInstanceIds.Where(mapMap.ContainsKey).Select(x => mapMap[x]);

            short regionIds = 0;
            foreach (var region in world.Regions)
            {
                var mapIds = region.regionObjectInstanceIds.Where(mapMap.ContainsKey).Select(x => mapMap[x]);
                regionMap[region.id] = regionIds;
                regionDatas.Add(new()
                {
                    Music = region.musicName,
                    RegionId = regionIds++,
                    Theme = region.theme,
                    WordLayerIds = mapIds.ToArray(),
                    Name = region.name,
                });
            }

            foreach (var mapTemp in mapTemps)
            {
                var x = mapTemp.x;
                var y = mapTemp.y;
                mapTemp.data.northNeighbor = findNeighbor(mapTemps, world, x, y, 0, -1)?.data.MapId ?? 0;
                mapTemp.data.southNeighbor = findNeighbor(mapTemps, world, x, y, 0, 1)?.data.MapId ?? 0;
                mapTemp.data.eastNeighbor = findNeighbor(mapTemps, world, x, y, 1, 0)?.data.MapId ?? 0;
                mapTemp.data.westNeighbor = findNeighbor(mapTemps, world, x, y, -1, 0)?.data.MapId ?? 0;

                mapTemp.data.region = regionMap.TryGetValue(mapTemp.originalRegionId, out var regionId) ? regionId : (short)0;
            }

            return new WorldData()
            {
                Maps = mapTemps.Select(x => x.data).ToList(),
                Regions = regionDatas,
                GlobalWordMapIds = globalMapIds.ToArray(),
                Name = world.worldName,
            };
        }

        private static MapTemp? findNeighbor(List<MapTemp> maps, SaveFormatWorld world, int x0, int y0, int dx, int dy)
        {
            if (x0 == 0 && dx < 0) return null;
            if (y0 == 0 && dy < 0) return null;

            var x = x0 + dx;
            var y = y0 + dy;

            if (mapExistsAt(maps, x, y) is MapTemp nearMap) return nearMap;

            var warps = world.Warps.Where(w => (w.x1 == x && w.y1 == y) || (w.x2 == x && w.y2 == y));
            foreach (var warp in warps)
            {
                int xf; int yf;
                if (warp.x1 == x && warp.y1 == y)
                    (xf, yf) = (warp.x2, warp.y2);
                else
                    (xf, yf) = (warp.x1, warp.y1);

                if (mapExistsAt(maps, xf, yf) is MapTemp farMap) return farMap;

                if (findNeighbor(maps, world, xf, yf, dx, dy) is MapTemp f) return f;
            }

            return null;
        }

        private static MapTemp? mapExistsAt(List<MapTemp> maps, int x, int y)
        {
            return maps.FirstOrDefault(w => w.x == x && w.y == y);
        }


        private static MapTemp fromMapLayer(SaveMapLayer layer, int x, int y, int id, ref short mapTempId)
        {
            var md = new MapData()
            {
                MapId = mapTempId++,
                width = (short)layer.width,
                height = (short)layer.height,
            };
            md.WorldObjects.AddRange(layer.objects.Where(obj => obj.x < layer.width && obj.y < layer.height).Select(x => new ObjectData()
            {
                Color = (short)x.color,
                x = x.x,
                y = x.y,
                Kind = x.name.StartsWith("text_") ? ObjectKind.Text : ObjectKind.Object,
                Name = Enum.Parse<ObjectTypeId>(x.name.Replace("text_", "")),
                Text = x.text,
                Present = true,
                Facing = (Direction)x.state,
            }).ToArray());
            return new MapTemp(md)
            {
                x = x,
                y = y,
                originalMapId = id,
            };
        }

    }
}
