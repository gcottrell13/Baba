using Core.Content;
using Core.Engine;
using Core.Utils;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Saves
{
    public static class CompileMap
    {
        public static WorldData CompileWorld(SaveFormatWorld world)
        {
            var screenDatas = new Dictionary<short, ScreenData>();
            var regionDatas = new List<RegionData>();
            var positionsByScreen = new Dictionary<short, Dictionary<int, PositionData>>();
            var objects = new List<ObjectData>();

            int globalPositionId = 0;

            foreach (var instance in world.WorldLayout)
            {
                var data = world.ScreenDatas.FirstOrDefault(x => x.id == instance.screenDataId);
                if (data == null) continue;

                var screenData = new ScreenData() { 
                    height=(short)data.layer1.height, 
                    width=(short)data.layer1.width,
                    ScreenId=(short)instance.instanceId,
                    RegionId=(short)data.regionId,
                    Name=data.name,
                };
                screenDatas[screenData.ScreenId] = screenData;

                var p = positionsFromLayer(data.layer1, screenData.ScreenId, ref globalPositionId);
                //positions.AddRange(p.Values);
                positionsByScreen[screenData.ScreenId] = p.Values.ToDictionary(pos => pos.PositionId, pos => pos);

                objects.AddRange(data.layer1.objects.Where(obj => obj.x < data.layer1.width && obj.y < data.layer1.height).Select(x => new ObjectData()
                {
                    Color = (short)x.color,
                    Kind = x.name.StartsWith("text_") ? ObjectKind.Text : ObjectKind.Object,
                    Name = Enum.Parse<ObjectTypeId>(x.name.Replace("text_", "")),
                    Text = x.text,
                    Present = true,
                    Facing = (Direction)x.state,
                    PositionId = p[(x.x, x.y)].PositionId,
                }));
            }

            short regionIds = 0;
            foreach (var region in world.Regions)
            {
                var regionScreenIds = region.regionObjectInstanceIds.Where(id => screenDatas.ContainsKey((short)id)).Select(x => (short)x).ToList();
                regionDatas.Add(new()
                {
                    Music = region.musicName,
                    RegionId = regionIds++,
                    Theme = region.theme,
                    WordLayerIds = regionScreenIds.ToArray(),
                    Name = region.name,
                });
            }
            void stitch(ScreenData thisScreen, short neighborId)
            {
                if (neighborId == 0) return;
                var newValues = stitchPositionsBetweenScreens(thisScreen, screenDatas[neighborId], positionsByScreen[thisScreen.ScreenId].Values, positionsByScreen[neighborId].Values);
                foreach (var val in newValues)
                {
                    positionsByScreen[thisScreen.ScreenId][val.PositionId] = val;
                }
            }

            foreach (var (screenId, screen) in screenDatas)
            {
                var x = screen.x;
                var y = screen.y;
                screen.northNeighborId = findNeighbor(world, x, y, 0, -1);
                screen.southNeighborId = findNeighbor(world, x, y, 0, 1);
                screen.eastNeighborId = findNeighbor(world, x, y, 1, 0);
                screen.westNeighborId = findNeighbor(world, x, y, -1, 0);

                stitch(screen, screen.northNeighborId);
                stitch(screen, screen.southNeighborId);
                stitch(screen, screen.westNeighborId);
                stitch(screen, screen.eastNeighborId);
            }

            var globalScreenIds = world.globalObjectInstanceIds.Select(id => (short)id).Where(id => screenDatas.ContainsKey(id)).ToArray();
            return new WorldData()
            {
                Positions = positionsByScreen.Values.SelectMany(screenPositions => screenPositions.Values).ToList(),
                Screens = screenDatas.Values.ToList(),
                Regions = regionDatas,
                GlobalWordMapIds = globalScreenIds,
                Name = world.worldName,
            };
        }

        private static short findNeighbor(SaveFormatWorld world, int x0, int y0, int dx, int dy)
        {
            if (x0 == 0 && dx < 0) return 0;
            if (y0 == 0 && dy < 0) return 0;

            var x = x0 + dx;
            var y = y0 + dy;

            if (getScreenIdAt(world, x, y) is SaveScreenInstance sv) return (short)sv.instanceId;

            var warps = world.Warps.Where(w => (w.x1 == x && w.y1 == y) || (w.x2 == x && w.y2 == y));
            foreach (var warp in warps)
            {
                int xf; int yf;
                if (warp.x1 == x && warp.y1 == y)
                    (xf, yf) = (warp.x2, warp.y2);
                else
                    (xf, yf) = (warp.x1, warp.y1);

                if (getScreenIdAt(world, xf, yf) is SaveScreenInstance farMap) return (short)farMap.instanceId;

                var n = findNeighbor(world, xf, yf, dx, dy);
                if (n != 0) return n;
            }

            return 0;
        }

        private static SaveScreenInstance? getScreenIdAt(SaveFormatWorld world, int x, int y)
        {
            return world.WorldLayout.FirstOrDefault(w => w.x == x && w.y == y);
        }

        private static Dictionary<(int x, int y), PositionData> positionsFromLayer(SaveMapLayer layer, short screenId, ref int globalPositionId)
        {
            var ids = new Dictionary<(int x, int y), int>();
            var dict = new Dictionary<(int x, int y), PositionData>();
            for (var x = 0; x < layer.width; x ++)
            {
                for (var y = 0; y < layer.height; y++)
                {
                    ids[(x, y)] = globalPositionId++;
                }
            }

            for (short x = 0; x < layer.width; x++)
            {
                for (short y = 0; y < layer.height; y++)
                {
                    ids.TryGetValue((x - 1, y), out var west);
                    ids.TryGetValue((x + 1, y), out var east);
                    ids.TryGetValue((x, y - 1), out var north);
                    ids.TryGetValue((x, y + 1), out var south);

                    dict[(x, y)] = new (ids[(x, y)], screenId, x, y, north, south, east, west);
                }
            }
            return dict;
        }

        private static List<PositionData> stitchPositionsBetweenScreens(ScreenData from, ScreenData to, IEnumerable<PositionData> f, IEnumerable<PositionData> t)
        {
            List<PositionData> fromPositions;
            List<PositionData> toPositions;
            Direction direction;

            if (to.ScreenId == from.northNeighborId)
            {
                fromPositions = f.Where(x => x.ScreenDisplayY == 0).ToList();
                toPositions = t.Where(x => x.ScreenDisplayY == (to.height - 1)).ToList();
                direction = Direction.Up;
            }
            else if (to.ScreenId == from.southNeighborId)
            {
                fromPositions = f.Where(x => x.ScreenDisplayY == (from.height - 1)).ToList();
                toPositions = t.Where(x => x.ScreenDisplayY == 0).ToList();
                direction = Direction.Down;
            }
            else if (to.ScreenId == from.westNeighborId)
            {
                fromPositions = f.Where(x => x.ScreenDisplayX == 0).ToList();
                toPositions = t.Where(x => x.ScreenDisplayX == (to.width - 1)).ToList();
                direction = Direction.Left;
            }
            else if (to.ScreenId == from.eastNeighborId)
            {
                fromPositions = f.Where(x => x.ScreenDisplayX == (from.width - 1)).ToList();
                toPositions = t.Where(x => x.ScreenDisplayX == 0).ToList();
                direction = Direction.Right;
            }
            else return new();

            var ratio = (float)toPositions.Count / (float)fromPositions.Count;
            foreach (var (fromPos, index) in fromPositions.Select((x, i) => (x, i)).ToList())
            {
                var n = (int)(index * ratio);
                var m = toPositions[n];

                var newItem = direction switch
                {
                    Direction.Left => fromPos with { west = m.PositionId },
                    Direction.Right => fromPos with { east = m.PositionId },
                    Direction.Up => fromPos with { north = m.PositionId },
                    Direction.Down => fromPos with { south = m.PositionId },
                    _ => fromPos,
                };
                fromPositions[index] = newItem;
            }
            return fromPositions;
        }
    }
}
