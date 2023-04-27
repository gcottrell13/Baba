using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Engine;

public class WorldData
{
    public List<RegionData> Regions = new();
    public List<MapData> Maps = new();
    public short[] GlobalWordMapIds;
    public string Name = string.Empty;

    public WorldData()
    {
        GlobalWordMapIds = Array.Empty<short>(); 
    }

    public string Serialize()
    {
        return $"""
            {outputWorldData(this)}

            {string.Join("\n\n", Regions.Select(outputRegion))}

            {string.Join("\n\n", Maps.Select(outputMap))}
            """;
    }

    public override bool Equals(object? obj)
    {
        if (obj is WorldData world)
            return Regions.Compare(world.Regions) && Maps.Compare(world.Maps) && world.GlobalWordMapIds.Compare(GlobalWordMapIds) && world.Name == Name;
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString() => $$"""
        new WorldData() {
            GlobalWordMapIds = new short[] { {{ string.Join(", ", GlobalWordMapIds) }} },
            Name = "{{Name}}",
            Maps = new() { {{string.Join(",", Maps.Select(x => "\n" + x.ToString().Indent(2)))}} },
            Regions = new() { {{string.Join(",", Regions.Select(x => "\n" + x.ToString().Indent(2)))}} }
        }
        """;

    public static WorldData Deserialize(string str)
    {
        var worldStartIndex = str.IndexOf(beginWorld);
        if (worldStartIndex == -1) throw new Exception("could not find world section");

        var worldEndIndex = str.IndexOf(endWorld, worldStartIndex);
        if (worldEndIndex == -1) throw new Exception("could not find end of world section");

        worldStartIndex += beginWorld.Length;
        var world = SerializeBytes.DeserializeObjects<WorldData>(str.Substring(worldStartIndex, worldEndIndex - worldStartIndex))[0];

        var regionStartIndex = str.IndexOf(beginRegion);
        while (regionStartIndex != -1)
        {
            var endIndex = str.IndexOf(endRegion, regionStartIndex);
            regionStartIndex += beginRegion.Length;
            world.Regions.Add(RegionData.Deserialize(str.Substring(regionStartIndex, endIndex - regionStartIndex)));
            regionStartIndex = str.IndexOf(beginRegion, endIndex);
        }

        var mapStartIndex = str.IndexOf(beginMap);
        while (mapStartIndex != -1)
        {
            var endIndex = str.IndexOf(endMap, mapStartIndex);
            mapStartIndex += beginMap.Length;
            world.Maps.Add(MapData.Deserialize(str.Substring(mapStartIndex, endIndex - mapStartIndex)));
            mapStartIndex = str.IndexOf(beginMap, endIndex);
        }

        return world;
    }

    private const string beginWorld = "---- BEGIN WORLD ----";
    private const string endWorld = "---- END WORLD ----";
    private const string beginRegion = "---- BEGIN REGION ----";
    private const string endRegion = "---- END REGION ----";
    private const string beginMap = "---- BEGIN MAP ----";
    private const string endMap = "---- END MAP ----";

    private static string outputWorldData(WorldData data)
    {
        return $"""
            # world {data.Name}
            {beginWorld}
            {SerializeBytes.SerializeObjects(new[] { data })}
            {endWorld}
            """;
    }

    private static string outputRegion(RegionData region)
    {
        return $"""
                # {region.RegionId} {region.Name}
                {beginRegion}
                {region.Serialize()}
                {endRegion}
                """;
    }

    private static string outputMap(MapData mapInfo)
    {
        return $"""
                # {mapInfo.MapId} {mapInfo.Name}
                {beginMap}
                {mapInfo.Serialize()}
                {endMap}
                """;
    }
}
