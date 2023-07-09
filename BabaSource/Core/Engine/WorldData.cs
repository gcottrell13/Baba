using Core.Content;
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
    public List<ScreenData> Screens = new();
    public List<PositionData> Positions = new();

    public short[] GlobalWordMapIds;
    public string Name = string.Empty;
    public Dictionary<ObjectTypeId, int> Inventory = new();

    public WorldData()
    {
        GlobalWordMapIds = Array.Empty<short>(); 
    }

    public string Serialize()
    {
        return $"""
            {outputWorldData(this)}

            {string.Join("\n\n", Regions.Select(outputRegion))}

            {string.Join("\n\n", Screens.Select(outputScreen))}
            """;
    }

    public override bool Equals(object? obj)
    {
        if (obj is WorldData world)
            return Regions.Compare(world.Regions) && Screens.Compare(world.Screens) && world.GlobalWordMapIds.Compare(GlobalWordMapIds) && world.Name == Name && Inventory.Compare(world.Inventory);
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString() => $$"""
        new {{nameof(WorldData)}}() {
            {{nameof(GlobalWordMapIds)}} = new short[] { {{ string.Join(", ", GlobalWordMapIds) }} },
            {{nameof(Name)}} = "{{Name}}",
            {{nameof(Screens)}} = new() { {{string.Join(",", Screens.Select(x => "\n" + x.ToString().Indent(2)))}} },
            {{nameof(Regions)}} = new() { {{string.Join(",", Regions.Select(x => "\n" + x.ToString().Indent(2)))}} },
            {{nameof(Positions)}} = new() { {{string.Join(",", Positions.Select(x => x.ToString())) }},
            {{nameof(Inventory)}} = new() {{{
        (Inventory.Count > 0 ? (
            " \n" + string.Join(",\n", 
                Inventory.Select(x => "{ ObjectTypeId." + x.Key.ToString() + ", " + x.Value.ToString() + " }")
            ).Indent(2) + "\n"
        ) : "")
        }} },
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

        var mapStartIndex = str.IndexOf(beginScreen);
        while (mapStartIndex != -1)
        {
            var endIndex = str.IndexOf(endScreen, mapStartIndex);
            mapStartIndex += beginScreen.Length;
            world.Screens.Add(ScreenData.Deserialize(str.Substring(mapStartIndex, endIndex - mapStartIndex)));
            mapStartIndex = str.IndexOf(beginScreen, endIndex);
        }

        return world;
    }

    private const string beginWorld = "---- BEGIN WORLD ----";
    private const string endWorld = "---- END WORLD ----";
    private const string beginRegion = "---- BEGIN REGION ----";
    private const string endRegion = "---- END REGION ----";
    private const string beginScreen = "---- BEGIN SCREEN ----";
    private const string endScreen = "---- END SCREEN ----";

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

    private static string outputScreen(ScreenData screenData)
    {
        return $"""
                # {screenData.ScreenId} {screenData.Name}
                {beginScreen}
                {screenData.Serialize()}
                {endScreen}
                """;
    }
}
