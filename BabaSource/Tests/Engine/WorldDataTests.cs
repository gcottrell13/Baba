using Core.Engine;
using NUnit.Framework;

namespace Tests.Engine;

[TestFixture]
public class WorldDataTests
{
    [Test]
    public void Serialize()
    {
        var serialized = WorldDataDeserialized.expectedCompiledMap.Serialize();

        Assert.AreEqual(WorldDataSerialized.expectedSerialized, serialized);
    }

    [Test]
    public void Deserialize()
    {
        var deserialized = WorldData.Deserialize(WorldDataSerialized.expectedSerialized);

        Assert.AreEqual(WorldDataDeserialized.expectedCompiledMap, deserialized);
    }

}
