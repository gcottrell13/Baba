
namespace Core.Engine;

public class RegionData
{
    public short RegionId;
    public short WordLayerId;
    public string Theme = string.Empty;
    public string Music = string.Empty;

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
            return r.RegionId == RegionId && r.WordLayerId == WordLayerId && r.Theme == Theme && r.Music == Music;
        return base.Equals(obj);
    }

    public override string ToString() => $$"""
        new RegionData() {
            RegionId = {{RegionId}},
            WordLayerId = {{WordLayerId}},
            Theme = "{{Theme}}",
            Music = "{{Music}}"
        }
        """;

    public static RegionData Deserialize(string str)
    {
        return SerializeBytes.DeserializeObjects<RegionData>(str)[0];
    }
}
