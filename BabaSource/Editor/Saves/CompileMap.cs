using Core.Content;
using Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves
{
    public static class CompileMap
    {
        private class MapTemp
        {
            internal int x;
            internal int y;
            internal MapData data;
        }

        public static WorldData CompileWorld(SaveFormatWorld world)
        {
            var mapTemps = new List<MapTemp>();

            short mapTempId = 1;

            MapData fromMapLayer(SaveMapLayer layer)
            {
                var md = new MapData(layer.objects.Select(x => new ObjectData()
                {
                    Color = (short)x.color,
                    x = x.x,
                    y = x.y,
                    ObjectId = ObjectInfo.NameToId[x.name],
                    Name = x.name,
                }).ToArray())
                {
                    MapId = mapTempId++,
                };
                return md;
            }

            var globalLayer = fromMapLayer(world.globalObjectLayer);
            globalLayer.Name = "global";
            mapTemps.Add(new()
            {
                data = globalLayer,
            });

            var regionMap = new Dictionary<int, short>();
            var regionDatas = new List<RegionData>();

            short regionIds = 0;
            foreach (var region in world.Regions)
            {
                var md = fromMapLayer(region.regionObjectLayer);
                md.Name = $"region {regionIds} - {region.name}";
                var temp = new MapTemp() { 
                    data = md,
                    x = -1,
                    y = -1,
                };
                mapTemps.Add(temp);
                regionMap[region.id] = regionIds;
                regionDatas.Add(new()
                {
                    Music=region.musicName,
                    RegionId=regionIds++,
                    Theme=region.theme,
                    WordLayerId=md.MapId,
                    Name=region.name,
                });
            }

            foreach (var instance in world.WorldLayout)
            {
                var data = world.MapDatas.FirstOrDefault(x => x.id == instance.mapDataId);
                if (data == null) continue;

                var md = fromMapLayer(data.layer1);
                var wordLayer = fromMapLayer(data.layer2);
                wordLayer.Name = $"{md.MapId} uplayer - {data.name}";
                mapTemps.Add(new()
                {
                    data = wordLayer,
                    x = -1,
                    y = -1,
                });

                md.region = regionMap.TryGetValue(data.regionId, out var regionMapTemp) ? regionMapTemp : (short)0;
                md.Name = data.name;
                md.upLayer = wordLayer.MapId;

                mapTemps.Add(new()
                {
                    data = md,
                    x = instance.x,
                    y = instance.y,
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
            }

            globalLayer.eastNeighbor = 0;
            globalLayer.westNeighbor = 0;
            globalLayer.northNeighbor = 0;
            globalLayer.southNeighbor = 0;

            return new WorldData()
            {
                Maps = mapTemps.Select(x => x.data).ToList(),
                Regions = regionDatas,
                GlobalWordMapId = globalLayer.MapId,
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
    }
}
