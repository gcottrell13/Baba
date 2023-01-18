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
        public void WorldData_Serialize(WorldObject[] input, string expected)
        {
            var data = new WorldData(input);

            var serialized = data.Serialize();
            Assert.AreEqual(expected, serialized);

            var deserialized = WorldData.Deserialize(serialized).WorldObjects;
            Assert.AreEqual(input, deserialized);
        }

        static IEnumerable<TestCaseData> _serializeTestCases => _serializeCaseGenerator.Select(x => new TestCaseData(x.input, x.expected).SetName(x.name));

        static IEnumerable<(string name, WorldObject[] input, string expected)> _serializeCaseGenerator { get
        {
                yield return (
                    "empty",
                    Array.Empty<WorldObject>(),
                    ""
                );
                yield return (
                    "one",
                    new[]
                    {
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Blue, Kind=WorldObjectKind.Text, ObjectId=1, Name="amongi" },
                    },
                    "\0\0\0Ȁ\0Ā"
                );
                yield return (
                    "three",
                    new[]
                    {
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Blue, Kind=WorldObjectKind.Text, ObjectId=1, Name="amongi" },
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Red, Kind=WorldObjectKind.Text, ObjectId=5, x=10, y=15, Name="badbad" },
                        new WorldObject() { Occupied= true, Color=WorldObjectColor.Faded | WorldObjectColor.Pink, Kind=WorldObjectKind.Object, ObjectId=400, Facing=Core.Utils.Direction.Left, Name="text_rubble" },
                    },
                    "\0\0\0Ȁ\0Ā਀ༀ\0Ā\0Ԁ\0\0Ѐ䤀Ā送"
                );
            } }


    }
}
