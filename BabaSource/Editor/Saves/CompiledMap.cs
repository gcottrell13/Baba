using Core.Content;
using Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves
{
    public class CompiledMap
    {
        private class MapTemp
        {
            internal uint x;
            internal uint y;
            internal MapData data;
        }

        public static List<string> CompileWorld(SaveFormatWorld world)
        {
            var mapTemps = new List<MapTemp>();

            short mapTempId = 1;

            MapData fromMapLayer(SaveMapLayer layer)
            {
                var md = new MapData(layer.objects.Select(x => new WorldObject()
                {
                    Color = (short)x.color,
                    x = (int)x.x,
                    y = (int)x.y,
                    ObjectId = ObjectInfo.NameToId[x.name],
                }).ToArray());

                md.id = mapTempId++;
                return md;
            }

            mapTemps.Add(new()
            {
                data = fromMapLayer(world.globalObjectLayer),
            });

            var regionMap = new Dictionary<int, MapTemp>();

            foreach (var region in world.Regions)
            {
                var md = fromMapLayer(region.regionObjectLayer);
                var temp = new MapTemp() { data = md };
                mapTemps.Add(temp);
                regionMap[region.id] = temp;
            }

            foreach (var instance in world.WorldLayout)
            {
                var data = world.MapDatas.FirstOrDefault(x => x.id == instance.mapDataId);
                if (data == null) continue;

                var md = fromMapLayer(data.layer1);

                md.region = regionMap.TryGetValue(data.regionId, out var regionMapTemp) ? regionMapTemp.data.id : (short)0;

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
                mapTemp.data.northNeighbor = findNeighbor(mapTemps, world, x, y, 0, -1)?.data.id ?? 0;
                mapTemp.data.southNeighbor = findNeighbor(mapTemps, world, x, y, 0, 1)?.data.id ?? 0;
                mapTemp.data.eastNeighbor = findNeighbor(mapTemps, world, x, y, 1, 0)?.data.id ?? 0;
                mapTemp.data.westNeighbor = findNeighbor(mapTemps, world, x, y, -1, 0)?.data.id ?? 0;
            }

            return mapTemps.Select(outputMap).ToList();
        }

        private static MapTemp? findNeighbor(List<MapTemp> maps, SaveFormatWorld world, uint x0, uint y0, int dx, int dy)
        {
            if (x0 == 0 && dx < 0) return null;
            if (y0 == 0 && dy < 0) return null;

            var x = (uint)(x0 + dx);
            var y = (uint)(y0 + dy);

            if (mapExistsAt(maps, x, y) is MapTemp nearMap) return nearMap;

            var warps = world.Warps.Where(w => (w.x1 == x && w.y1 == y) || (w.x2 == x && w.y2 == y));
            foreach (var warp in warps)
            {
                uint xf; uint yf;
                if (warp.x1 == x && warp.y1 == y)
                    (xf, yf) = (warp.x2, warp.y2);
                else
                    (xf, yf) = (warp.x1, warp.y1);

                if (mapExistsAt(maps, xf, yf) is MapTemp farMap) return farMap;

                if (findNeighbor(maps, world, xf, yf, dx, dy) is MapTemp f) return f;
            }

            return null;
        }

        private static MapTemp? mapExistsAt(List<MapTemp> maps, uint x, uint y)
        {
            return maps.FirstOrDefault(w => w.x == x && w.y == y);
        }

        private static string outputRegion(RegionData region)
        {
            return $"""
                ---- BEGIN REGION ----
                {region.Serialize()}
                ---- END REGION ----
                """;
        }

        private static string outputMap(MapTemp mapInfo)
        {
            return $"""
                ---- BEGIN MAP ----
                {mapInfo.data.Serialize()}
                ---- END MAP ----
                """;
        }
    }
}
