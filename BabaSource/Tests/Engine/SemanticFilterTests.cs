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
        [TestCaseSource(nameof(GetTestCases))]
        public void Filter_ShouldNotSkipWords(List<List<Item?>> input, List<string> expected)
        {
            var rules = SemanticFilter.FindRulesAndFilterInvalid(input);
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
                        new() { "text_baba", "text_and", "text_stop", "text_is", "text_win" },
                    },
                    new() { }
                );
                yield return (
                    "should not include invalid rules",
                    new() {
                        new() { "text_stop", "text_and", "text_baba", "text_is", "text_win" },
                    },
                    new() {
                        "text_baba text_is text_win",
                    }
                );
                yield return (
                    "should not include object feeling object",
                    new() {
                        new() { "text_baba", "text_feeling", "text_baba", "text_is", "text_win" },
                    },
                    new() { }
                );

                yield return (
                    "parse letters as word",
                    new() {
                        new() { "text_b", "text_a", "text_b", "text_a", "text_is", "text_win" },
                    },
                    new() {
                        "text_baba text_is text_win",
                    }
                );

                yield return (
                    "multiple rules",
                    new() {
                        new() { "text_baba", "text_and", "text_lonely", "text_rock", "text_is", "text_win", "text_and", "text_stop" },
                    },
                    new() {
                        "text_lonely text_rock text_is text_win",
                        "text_lonely text_rock text_is text_stop", 
                        "text_baba text_is text_win", 
                        "text_baba text_is text_stop",
                    }
                );
            } }
    }
}
