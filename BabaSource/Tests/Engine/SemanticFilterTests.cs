using Core.Content;
using Core.Engine;
using MonoGame.Extended.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine
{
    [TestFixture]
    public class SemanticFilterTests
    {

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void Filter_ShouldNotSkipWords(List<List<Item?>> input, List<string> expected)
        {
            var rules = SemanticFilter.FindRulesAndFilterInvalid(new Grid(input));
            Assert.AreEqual(expected, rules.Select(r => r.ToString()).ToList());
        }


        static IEnumerable<TestCaseData> GetTestCases => _getTestCases.Select(
            x => new TestCaseData(x.input, x.expected)
            .SetName(x.name)
        );

        static IEnumerable<(string name, List<List<Item?>> input, List<string> expected)> _getTestCases { get
            {
                yield return (
                    "should not skip words",
                    new() { 
                        new() { "baba", "and", "stop", "is", "win" },
                    },
                    new() { }
                );
                yield return (
                    "should not include invalid rules",
                    new() {
                        new() { "stop", "and", "baba", "is", "win" },
                    },
                    new() {
                        "baba is win",
                    }
                );
                yield return (
                    "should not include object feeling object",
                    new() {
                        new() { "baba", "feeling", "baba", "is", "win" },
                    },
                    new() { }
                );

                yield return (
                    "parse letters as word",
                    new() {
                        new() { "b", "a", "b", "a", "is", "win" },
                    },
                    new() {
                        "baba is win",
                    }
                );

                yield return (
                    "multiple rules",
                    new() {
                        new() { "baba", "and", "lonely", "rock", "is", "win", "and", "stop" },
                    },
                    new() {
                        "lonely rock is win",
                        "lonely rock is stop",
                        "baba is win",
                        "baba is stop",
                    }
                );

                foreach (var sentence in new[]
                {
                    "baba has flag",
                    "baba eat flag on water",
                    "baba fear flag not on not water",
                    "baba follow me feeling blue"
                })
                {
                    yield return (
                        sentence,
                        new() { sentence.Split(" ").Select(x => new Item() { Name = Enum.Parse<ObjectTypeId>(x) }).ToList()! },
                        new() { sentence }
                    );
                }

                yield return (
                    "all",
                    new() {
                        new() { "all", "is", "win", "and", "stop", "rock", "is", "all" },
                    },
                    new() {
                        "all is win",
                        "all is stop",
                        "rock is all",
                    }
                );

                yield return (
                    "HAS NOT - does not work",
                    new() {
                        new() { "baba", "has", "not", "rock", "is", "all" },
                    },
                    new() {
                        "not rock is all",
                    }
                );
            } }
    }
}
