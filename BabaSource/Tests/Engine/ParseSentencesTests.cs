using Core.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Engine.ParseSentences;

namespace Tests.Engine
{
    [TestFixture]
    internal class ParseSentencesTests
    {

        public class Item : INameable
        {
            public string Name { get; set; }
            public static implicit operator Item(string s) { return new() { Name = s }; }
            public override string ToString() => Name;
            public override bool Equals(object? obj)
            {
                if (obj is string s) return s == Name;
                if (obj is Item i) return i.Name == Name;
                return base.Equals(obj);
            }

            public override int GetHashCode() => Name.GetHashCode();
        }

        [Test]
        [TestCaseSource(nameof(GetChainsTestCases))]
        public void GetChains_Test(List<List<Item?>> input, List<List<Item>> expected)
        {
            var chains = GetWordChains(input, new HashSet<string?> { "a", "b", "c" });
            Assert.AreEqual(expected, chains);
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
                        new() { "c", "c", "c", "c" },
                        new() { "c", "b", "a" },
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

            var chains = GetWordChains(input, new HashSet<string?> { "a", "b", "c" });

            Assert.That(chains[0].Count == 5);
            Assert.That(chains[0].Zip(input[0]).All(t => object.ReferenceEquals(t.First, t.Second)));
        }
    }
}
