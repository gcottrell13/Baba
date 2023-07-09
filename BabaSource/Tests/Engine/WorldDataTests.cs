using Core.Engine;
using NUnit.Framework;

namespace Tests.Engine;

[TestFixture]
public class WorldDataTests
{
    [Test]
    public void Deserialize()
    {
        var deserialized = WorldData.Deserialize(WorldDataDeserialized.expectedCompiledMap.Serialize());

        Assert.AreEqual(WorldDataDeserialized.expectedCompiledMap, deserialized);
    }

}
