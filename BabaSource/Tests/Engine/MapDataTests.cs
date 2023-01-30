using Core.Content;
using Core.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine
{
    [TestFixture]
    public class MapDataTests
    {
        [Test]
        [TestCaseSource(nameof(_serializeTestCases))]
        public void WorldData_Serialize(ObjectData[] input)
        {
            var data = new MapData(input);

            var serialized = data.Serialize();
            var deserialized = MapData.Deserialize(serialized).WorldObjects;
            Assert.AreEqual(input, deserialized);
        }

        static IEnumerable<TestCaseData> _serializeTestCases => _serializeCaseGenerator.Select(x => new TestCaseData(x.input).SetName(x.name));

        static IEnumerable<(string name, ObjectData[] input)> _serializeCaseGenerator { get
        {
                yield return (
                    "empty",
                    Array.Empty<ObjectData>()
                );
                yield return (
                    "one",
                    new[]
                    {
                        new ObjectData() { Deleted= true, Color=2, Name=ObjectTypeId.amongi },
                    }
                );
                yield return (
                    "three",
                    new[]
                    {
                        new ObjectData() { Deleted= true, Color=2, Name=ObjectTypeId.amongi },
                        new ObjectData() { Deleted= true, Color=1, Name=ObjectTypeId.badbad, x=10, y=15 },
                        new ObjectData() { Deleted= true, Color=3, Name=ObjectTypeId.rubble, Facing=(int)Core.Utils.Direction.Left, Kind=ObjectKind.Text },
                    }
                );
            } 
        }
    }
}
