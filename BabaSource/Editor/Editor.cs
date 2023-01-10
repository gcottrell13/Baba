using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Content;

namespace Editor
{
    internal class Editor
    {
        public static readonly Editor EDITOR = new(LoadSaveFiles.LoadAllWorlds());

        private readonly ReadonlySavesList savesList;
        public SaveFormat? currentWorld { get; private set; }
        public MapData? currentMap { get; private set; }
        public MapLayer? currentMapLayer { get; private set; }
        public Region? currentRegion { get; private set; }
        
        public Editor(ReadonlySavesList savesList)
        {
            this.savesList = savesList;
        }

        public bool HasWorldLoaded() => currentWorld != null;
        public bool HasMapLoaded() => currentMap != null;

        public void SaveAll()
        {
            if (currentWorld == null) return;
            LoadSaveFiles.SaveAll(currentWorld);
        }

        public SaveFormat NewWorld()
        {
            var newWorld = new SaveFormat() { worldName = $"new world {savesList.Count + 1}" };
            LoadSaveFiles.AddNewSave(newWorld);
            LoadSaveFiles.SaveAll(newWorld);
            return newWorld;
        }

        public MapData NewMap()
        {
            var maxId = mapDatas.Count > 0 ? mapDatas.Max(x => x.id) : 0;
            var newMap = new MapData() { name = $"new map {mapDatas.Count + 1}", id = maxId + 1 };
            currentWorld!.MapDatas.Add(newMap);
            LoadSaveFiles.SaveAll(currentWorld);
            return newMap;
        }

        public Region NewRegion()
        {
            var maxId = regions.Count > 0 ? regions.Max(x => x.id) : 0;
            var newRegion = new Region() { name = $"new region {maxId + 1}", id = maxId + 1 };
            currentWorld!.Regions.Add(newRegion);
            LoadSaveFiles.SaveAll(currentWorld);
            return newRegion;
        }

        public void LoadWorld(SaveFormat? world)
        {
            if (world == null) throw new ArgumentNullException(nameof(world));
            if (savesList.Contains(world) != true) throw new Exception("world does not exist in the current list");
            currentWorld = world;
        }

        public void LoadMap(MapData? map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (mapDatas.Contains(map) != true) throw new Exception("map does not exist in the current list");
            currentMap = map;
        }

        public void LoadRegion(Region? region)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            if (regions.Contains(region) != true) throw new Exception("region does not exist in the current list");
            currentRegion = region;
        }

        public List<MapData> mapDatas => currentWorld?.MapDatas ?? new List<MapData>();

        public List<Region> regions => currentWorld?.Regions ?? new List<Region>();

        public static ObjectData? ObjectAtPosition(uint x, uint y, MapLayer? map)
        {
            if (map == null) return null;

            x %= map.width;
            y %= map.height;

            return map.objects.FirstOrDefault(item => item.x == x && item.y == y);
        }

        public string? GetRegionTheme(int regionId)
        {
            return regions.FirstOrDefault(x => x.id == regionId)?.theme;
        }

    }
}
