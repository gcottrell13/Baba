using Core.Content;
using Core.Engine;
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

            public MapTemp(MapData data)
            {
                this.data = data;
            }
        }

        public static WorldData CompileWorld(SaveFormatWorld world)
        {
            var mapTemps = new List<MapTemp>();

            short mapTempId = 1;

            MapTemp fromMapLayer(SaveMapLayer layer, int x, int y, int id)
            {
                var md = new MapData(layer.objects.Select(x => new ObjectData()
                {
                    Color = (short)x.color,
                    x = x.x,
                    y = x.y,
                    Kind = x.name.StartsWith("text_") ? ObjectKind.Text : ObjectKind.Object,
                    Name = Enum.Parse<ObjectTypeId>(x.name.Replace("text_", "")),
                    Text = x.text,
                    Present = true,
                }).ToArray())
                {
                    MapId = mapTempId++,
                    width = (short)layer.width,
                    height = (short)layer.height,
                };
                return new MapTemp(md)
                {
                    x = x,
                    y = y,
                    originalMapId = id,
                };
            }

            var globalLayer = fromMapLayer(world.globalObjectLayer, -1, -1, 0);
            globalLayer.data.Name = "global";
            mapTemps.Add(globalLayer);

            var regionMap = new Dictionary<int, short>();
            var regionDatas = new List<RegionData>();

            short regionIds = 0;
            foreach (var region in world.Regions)
            {
                var md = fromMapLayer(region.regionObjectLayer, -1, -1, 0);
                md.data.Name = $"region {regionIds} - {region.name}";
                mapTemps.Add(md);
                regionMap[region.id] = regionIds;
                regionDatas.Add(new()
                {
                    Music=region.musicName,
                    RegionId=regionIds++,
                    Theme=region.theme,
                    WordLayerId=md.data.MapId,
                    Name=region.name,
                });
            }

            foreach (var instance in world.WorldLayout)
            {
                var data = world.MapDatas.FirstOrDefault(x => x.id == instance.mapDataId);
                if (data == null) continue;

                var md = fromMapLayer(data.layer1, instance.x, instance.y, instance.mapDataId);
                var wordLayer = fromMapLayer(data.layer2, -1, -1, -1);
                wordLayer.data.Name = $"{md.data.MapId} uplayer - {data.name}";
                mapTemps.Add(wordLayer);

                md.data.region = regionMap.TryGetValue(data.regionId, out var regionMapTemp) ? regionMapTemp : (short)0;
                md.data.Name = data.name;
                md.data.upLayer = wordLayer.data.MapId;

                mapTemps.Add(md);

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

            globalLayer.data.eastNeighbor = 0;
            globalLayer.data.westNeighbor = 0;
            globalLayer.data.northNeighbor = 0;
            globalLayer.data.southNeighbor = 0;

            return new WorldData()
            {
                Maps = mapTemps.Select(x => x.data).ToList(),
                Regions = regionDatas,
                GlobalWordMapId = globalLayer.data.MapId,
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
