using System;
using Core.Utils;

namespace Core.Engine;

public class RegionData
{
    public short RegionId;
    public short[] WordLayerIds;
    public string Theme = string.Empty;
    public string Music = string.Empty;
    public string Name = string.Empty;

    public RegionData()
    {
        WordLayerIds = Array.Empty<short>();
    }

    public string Serialize()
    {
        return SerializeBytes.SerializeObjects(new[] { this });
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is RegionData r) 
            return r.RegionId == RegionId && r.WordLayerIds.Compare(WordLayerIds) && r.Theme == Theme && r.Music == Music;
        return base.Equals(obj);
    }

    public override string ToString() => $$"""
        new RegionData() {
            {{nameof(RegionId)}} = {{RegionId}},
            {{nameof(WordLayerIds)}} = new {{WordLayerIds.GetType().Name}} { {{ string.Join(", ", WordLayerIds) }} },
            {{nameof(Theme)}} = "{{Theme}}",
            {{nameof(Music)}} = "{{Music}}",
            {{nameof(Name)}} = "{{Name}}"
        }
        """;

    public static RegionData Deserialize(string str)
    {
        return SerializeBytes.DeserializeObjects<RegionData>(str)[0];
    }
}
