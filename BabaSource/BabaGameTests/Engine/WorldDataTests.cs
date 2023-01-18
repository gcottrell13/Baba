using BabaGame.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGameTests.Engine
{
    [TestFixture]
    public class WorldDataTests
    {
        [Test]
        [TestCaseSource(nameof(_serializeTestCases))]
        public void WorldData_Serialize(WorldObject[] input)
        {
            var data = new WorldData(input);

            var serialized = data.Serialize();
            var deserialized = WorldData.Deserialize(serialized).WorldObjects;
            Assert.AreEqual(input, deserialized);
        }

        static IEnumerable<TestCaseData> _serializeTestCases => _serializeCaseGenerator.Select(x => new TestCaseData(x.input).SetName(x.name));

        static IEnumerable<(string name, WorldObject[] input)> _serializeCaseGenerator { get
        {
                yield return (
                    "empty",
                    Array.Empty<WorldObject>()
                );
                yield return (
                    "one",
                    new[]
                    {
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Blue, Kind=WorldObjectKind.Text, ObjectId=1, Name="amongi" },
                    }
                );
                yield return (
                    "three",
                    new[]
                    {
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Blue, Kind=WorldObjectKind.Text, ObjectId=1, Name="amongi" },
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Red, Kind=WorldObjectKind.Text, ObjectId=5, x=10, y=15, Name="badbad" },
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Faded | WorldObjectColor.Pink, Kind=WorldObjectKind.Object, ObjectId=400, Facing=Core.Utils.Direction.Left, Name="text_rubble" },
                    }
                );
            } }


    }
}
