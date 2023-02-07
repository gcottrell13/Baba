using BabaGame.Engine;
using Core.Content;
using Core.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaGameTests;


[TestFixture]
public class MapSimulatorTests
{
    [Test]
    [TestCaseSource(nameof(_test_on_is_stop_cases))]
    public void test_on_is_stop(List<string> strRules, List<ObjectData> objects, List<ObjectData> expectedStopObjects)
    {
        var babaWorld = new BabaWorld(new() {
            GlobalWordMapId = 0,
            Maps = new()
            {
                new(objects.ToArray()) { MapId=1 },
                new(strRules.SelectMany((line, y) => line.Split(" ").Select((word, x) => new ObjectData() { Name=Enum.Parse<ObjectTypeId>(word), x=x, y=y, Kind=ObjectKind.Text } )).ToArray()) { MapId=0 },
            },
        });
        var sim = babaWorld.Simulators[1];
        var global = babaWorld.Simulators[0];

        var givenRules = global.parseRules(new());
        var allRules = sim.parseRules(givenRules);

        var stopObjects = sim.findObjectsThatAre(ObjectTypeId.stop, allRules).ToList();
        Assert.AreEqual(expectedStopObjects, stopObjects);
    }

    public static IEnumerable<TestCaseData> _test_on_is_stop_cases => _test_on_is_stop_test_cases.Select(x => new TestCaseData(x.strRules, x.objects, x.expected).SetName($"ON - {x.name}"));

    public static IEnumerable<(string name, List<string> strRules, List<ObjectData> objects, List<ObjectData> expected)> _test_on_is_stop_test_cases { get
        {
            yield return (
                "object cannot trigger ON by itself",
                new() { "baba on baba is stop" },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                },
                new() { }
            );
            yield return (
                "ON triggered by two of same objects",
                new() { "baba on baba is stop" },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 2 },
                },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1 },
                }
            );
            yield return (
                "NOT ON not triggered when on top",
                new() { "baba not on tile is stop" },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 1, index = 2 },
                },
                new()
                {
                }
            );
            yield return (
                "NOT ON triggered when not on top",
                new() { "baba not on tile is stop" },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 2, index = 2 },
                },
                new()
                {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                }
            );
            yield return (
                "not baba not on not tile is stop",
                new() { "not baba not on not tile is stop" },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 3 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 2, index = 2 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 3, index = 4 },
                },
                new()
                {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 2 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 3, index = 4 },
                }
            );
            yield return (
                "recursive ON - disallowed by semantic filter",
                new() { "rock not on baba on tile is stop" },
                new() {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.rock, x = 1, y = 1, index = 3 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.rock, x = 1, y = 2, index = 2 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.tile, x = 1, y = 1, index = 4 },
                },
                new()
                {
                }
            );
        } }


    [Test]
    public void objects_at_within_map()
    {
        var babaWorld = new BabaWorld(new()
        {
            GlobalWordMapId = 0,
            Maps = new()
            {
                new(new ObjectData[]
                {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.boat, x = 1, y = 2, index = 1 },
                }) { MapId = 1, width = 15, height = 15 },
                new(new ObjectData[]
                {
                    new() { Kind=ObjectKind.Text, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Text, Name=ObjectTypeId.@is, x = 1, y = 2, index = 1 },
                    new() { Kind=ObjectKind.Text, Name=ObjectTypeId.you, x = 1, y = 3, index = 1 },
                }) { MapId = 0, width = 15, height = 15 },
            }
        });
        var actual = babaWorld.Simulators[1].objectsAt(1, 1);
        Assert.AreEqual(new ObjectData[]
        {
            new() { Kind=ObjectKind.Object, Name=ObjectTypeId.baba, x = 1, y = 1, index = 1 },
        }, actual);
    }

    [Test]
    public void pull_within_map()
    {
        var babaWorld = new BabaWorld(new()
        {
            GlobalWordMapId = 0,
            Maps = new()
            {
                new(new ObjectData[]
                {
                    new() { Kind=ObjectKind.Object, Name=ObjectTypeId.boat, x = 1, y = 2, index = 1 },
                }) { MapId = 1, width = 15, height = 15 },
                new(new ObjectData[]
                {
                    new() { Kind=ObjectKind.Text, Name=ObjectTypeId.boat, x = 2, y = 1, index = 1 },
                    new() { Kind=ObjectKind.Text, Name=ObjectTypeId.@is, x = 2, y = 2, index = 1 },
                    new() { Kind=ObjectKind.Text, Name=ObjectTypeId.pull, x = 2, y = 3, index = 1 },
                }) { MapId = 0, width = 15, height = 15 },
            }
        });

        var global = babaWorld.Simulators[0].parseRules(new());

        babaWorld.Simulators[1].pull(1, 3, 0, 1, global);
        Assert.AreEqual(new ObjectData[]
        {
            new() { Kind=ObjectKind.Object, Name=ObjectTypeId.boat, x = 1, y = 3, index = 1 },
        }, babaWorld.Simulators[1].objectsAt(1, 3));
    }
}
