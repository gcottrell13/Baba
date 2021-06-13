using BabaGame.src.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static BabaGame.src.Engine.WordEngine;

namespace Tests.Engine
{
    [TestFixture]
    public class WordEngineTests
    {

        public static IEnumerable<TestCaseData> TestPhraseParsingCases()
        {
            yield return new TestCaseData("baba is you", new FullRule
            {
                TargetName = "baba",
                Verb = "is",
                Feature = "you",
            });

            yield return new TestCaseData("baba is not you", new FullRule
            {
                TargetName = "baba",
                Verb = "is",
                FeatureNot = true,
                Feature = "you",
            });

            yield return new TestCaseData("lonely rock is defeat", new FullRule
            {
                TargetCondition = new TargetCondition { Modifier = "lonely" },
                TargetName = "rock",
                Verb = "is",
                Feature = "defeat",
            });

            yield return new TestCaseData("not baba not on not rock is win", new FullRule
            {
                TargetCondition = new TargetCondition { 
                    Not = true, 
                    Modifier = "on",
                    Argument = "rock",
                    ArgumentNot = true,
                },
                TargetNot = true,
                TargetName = "baba",
                Verb = "is",
                Feature = "win",
            });
        }


        [TestCaseSource(nameof(TestPhraseParsingCases))]
        [Test]
        public void TestPhraseParsing(string phrase, FullRule expected)
        {
            var actualRule = WordEngine.ParsePhrase(phrase);

            Assert.AreEqual(expected, actualRule);
        }

    }
}
