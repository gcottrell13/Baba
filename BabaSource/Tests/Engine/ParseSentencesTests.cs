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
    internal class ParseSentencesTests
    {

        [Test]
        [TestCaseSource(nameof(GetChainsTestCases))]
        public void GetChains_Test(List<List<Item?>> input, List<List<Item>> expected)
        {
            var chains = ParseSentences.GetWordChains(new Grid(input));
            Assert.AreEqual(expected.Select(x => x.ToArray()), chains);
        }

        static IEnumerable<TestCaseData> GetChainsTestCases => _getChainsTestCases.Select(
            x => new TestCaseData(x.input, x.expected)
            .SetName(x.name)
        );

        static IEnumerable<(string name, List<List<Item?>> input, List<List<Item>> expected)> _getChainsTestCases { get
            {
                yield return (
                    "aaa",
                    new() {
                        new() { null, null, null, "a" },
                        new() { null, null, null, "a" },
                        new() { null, null, null, "a" },
                    },
                    new() {
                        new() { "a", "a", "a" },
                    });
                yield return (
                    "separate",
                    new() {
                        new() { "b", null, null, "a" },
                        new() { "a", null, null, "a" },
                        new() { "c", null, null, "a" },
                    },
                    new() {
                        new() { "b", "a", "c" },
                        new() { "a", "a", "a" },
                    });
                yield return (
                    "cross",
                    new() {
                        new() { null, "b", null },
                        new() { "a", "b", "a" },
                        new() { null, "a", null },
                    },
                    new() {
                        new() {"b", "b", "a"},
                        new() {"a", "b", "a"},
                    });
                yield return (
                    "cbacbacba",
                    new() {
                        new() { "c", "b", "a" },
                        new() { "c", "b", "a" },
                        new() { "c", "b", "a" },
                        new() { "c", "b", "a" },
                    },
                    new() {
                        new() { "c", "b", "a" },
                        new() { "c", "c", "c", "c" },
                        new() { "b", "b", "b", "b" },
                        new() { "a", "a", "a", "a" },
                        new() { "c", "b", "a" },
                        new() { "c", "b", "a" },
                        new() { "c", "b", "a" },
                    });
            } }

        [Test]
        public void GetWordChains_ReturnsSameReference()
        {
            var input = new List<List<Item?>>()
            {
                new() { "a", "b", "c", "b", "a" },
            };

            var chains = ParseSentences.GetWordChains(new Grid(input));

            Assert.That(chains[0].Length == 5);
            Assert.That(chains[0].Zip(input[0]).All(t => object.ReferenceEquals(t.First, t.Second)));
        }


        [Test]
        [TestCaseSource(nameof(GetSentencesTestCases))]
        public void GetSentences_ShouldParse(List<List<Item?>> input, List<string> expected)
        {
            var sentences = ParseSentences.GetSentences(new Grid(input), new()
            {
                nouns = new() { ObjectTypeId.baba, ObjectTypeId.rock, ObjectTypeId.flag, ObjectTypeId.box, ObjectTypeId.water },
                verbs = new() { ObjectTypeId.@is, ObjectTypeId.has },
                modifiers = new() { ObjectTypeId.not, ObjectTypeId.lonely },
                adjectives = new() { ObjectTypeId.you, ObjectTypeId.win },
                conjunctions = new() { ObjectTypeId.and },
                relations= new() { ObjectTypeId.on, ObjectTypeId.near },
                characters = new() { { ObjectTypeId.b, "b" }, { ObjectTypeId.a, "a" } },
            });

            Assert.AreEqual(expected, sentences.Select(x => x.ToString()).ToList());
        }

        static IEnumerable<TestCaseData> GetSentencesTestCases => _getSentencesTestCases.Select(
            x => new TestCaseData(x.input, x.expected)
            .SetName(x.name));

        public static IEnumerable<(string name, List<List<Item?>> input, List<string> expected)> _getSentencesTestCases { get
            {
                yield return (
                    "parse several at once",
                    new()
                    {
                        new(){"rock", "baba", "baba"},
                        new(){"rock", "has", "flag"},
                        new(){"rock", "rock", "is"},
                        new(){"rock", "baba", "you"},
                        new(){"rock", "has", null},
                        new(){"rock", "box", "you"},
                        new(){"rock", "baba", "you"},
                    },
                    new()
                    {
                        "rock has flag",
                        "flag is you",
                        "baba has rock",
                        "baba has box",
                    }
                );

                foreach (var sentence in new[]
                {
                    "flag not on water is win",
                    "lonely baba is rock",
                    "baba and flag on water is win and not box",
                    "not lonely baba not on not flag is you",
                })
                {
                    yield return (
                        sentence,
                        new() { sentence.Split(" ").Select(x => new Item() { Name = Enum.Parse<ObjectTypeId>(x) }).ToList()! },
                        new() { sentence }
                    );
                }

                yield return (
                    "parse characters",
                    new()
                    {
                        "b a b a is you".Split(" ").Select(x => new Item() {Name = Enum.Parse<ObjectTypeId>(x) }).ToList()!,
                    },
                    new()
                    {
                        "baba is you",
                    }
                );
            } }
    }
}
