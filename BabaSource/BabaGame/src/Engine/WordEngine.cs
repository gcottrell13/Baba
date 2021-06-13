using BabaGame.src.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace BabaGame.src.Engine
{
    public class WordEngine
    {
        public Dictionary<string, List<FullRule>> FeatureIndex;

        public static readonly string[] BriefNounList = new[] { "group", "all" };
        public static readonly string[] FullNounList = new[] { "group", "all", "text", "empty", "level" };
        public static readonly string[] ObjectNounList = new[] { "text", "empty" };
        public static readonly string[] ShortNounList = new[] { "group", "all", "text", "empty" };

        public static readonly string[] Conditions = new[]
        {
            "never",
            "facing",
            "on",
            "near",
            "without",
            "below",
            "above",
            "besideleft",
            "besideright",
            "feeling",
            "lonely",
            "idle",
            "powered",
            "seldom",
            "often",
        };

        public static readonly string[] CompoundModifiers = new[] { "on", "facing", "near", "without", "below", "above", "besideleft", "besideright", "feeling" };

        public static readonly string[] Verbs = new[] { "has", "is", "eat", "fear", "make", "follow", "mimic" };

        public static bool ContainsNoun(IEnumerable<string> list, string noun)
        {
            return list.Any(n => n == noun || (n == "group" && noun.Length >= 5 && noun[0..5] == "group"));
        }


        public WordEngine()
        {
            FeatureIndex = new Dictionary<string, List<FullRule>>();
        }

        public IEnumerable<FullRule> FindFeature(string? target, string verb, string? feature)
        {
            var ruleAsString = string.Join(" ", new[] { target, verb, feature }.Where(f => !string.IsNullOrWhiteSpace(f)));
            var options = new List<FullRule>();

            if (target != null && FeatureIndex.ContainsKey(target))
            {
                foreach (var item in FeatureIndex[target])
                {
                    if (item.TargetCondition?.FullModifier != "never")
                    {
                        if (target == item.TargetName && verb == item.Verb)
                            options.Add(item);
                    }
                }
            }

            if (feature != null && FeatureIndex.ContainsKey(feature) && !FeatureIndex.ContainsKey(target ?? ""))
            {
                foreach (var item in FeatureIndex[feature])
                {
                    if (item.TargetCondition?.FullModifier != "never")
                    {
                        if (item.Feature == feature && item.Verb == verb)
                            options.Add(item);
                    }
                }
            }

            // for stuff like "fear" or "eat" or "has" or "follow"
            if (target == null && feature == null && verb != null && FeatureIndex.ContainsKey(verb))
            {
                // features.lua line 47
            }

            foreach (var fullRule in options)
            {
                var one = fullRule.TargetName;
                var two = $"{fullRule.Verb} {fullRule.Feature}";
                var three = $"{fullRule.TargetName} {fullRule.Verb} {fullRule.Feature}";

                if (one == ruleAsString || two == ruleAsString || three == ruleAsString || (verb == fullRule.Verb && target == null && feature == null))
                {
                    yield return fullRule;
                }
            }
        }

        public void AddRule(FullRule? _rule)
        {
            if (!(_rule is FullRule rule)) return;

            if (!FeatureIndex.ContainsKey(rule.TargetName))
                FeatureIndex[rule.TargetName] = new List<FullRule>();
            if (!FeatureIndex.ContainsKey(rule.Verb))
                FeatureIndex[rule.Verb] = new List<FullRule>();
            if (!FeatureIndex.ContainsKey(rule.Feature))
                FeatureIndex[rule.Feature] = new List<FullRule>();

            FeatureIndex[rule.TargetName].Add(rule);
            FeatureIndex[rule.Verb].Add(rule);
            FeatureIndex[rule.Feature].Add(rule);
        }

        public static FullRule? ParsePhrase(string phrase)
        {
            var verbInPhrase = Verbs.Where(verb => phrase.Contains($" {verb} ")).ToList();
            if (verbInPhrase.Count != 1) return null;

            var phraseParts = phrase.Split($" {verbInPhrase.Single()} ");
            var targetSubPhrase = phraseParts[0];
            var featureSubPhrase = phraseParts[1];

            var rule = new FullRule()
            {
                Verb = verbInPhrase.Single(),
            };

            {
                // target

                if (targetSubPhrase.Split(' ').Count() == 1)
                {
                    rule.TargetName = targetSubPhrase;
                }
                else
                {
                    var cond = new TargetCondition();

                    var allParts = targetSubPhrase.Split(' ').ToList();
                    var parts = allParts.Select((word, index) => (word, index)).Where(i => i.word != "not").ToList();

                    if (parts.Any(i => CompoundModifiers.Contains(i.word)))
                    {
                        if (parts.Count != 3) return null;

                        var target = parts[0];
                        var verb = parts[1];
                        var argument = parts[2];

                        if (target.index > 0 && allParts[target.index - 1] == "not")
                        {
                            rule.TargetNot = true;
                        }
                        if (verb.index > 0 && allParts[verb.index - 1] == "not")
                        {
                            cond.Not = true;
                        }
                        if (argument.index > 0 && allParts[argument.index - 1] == "not")
                        {
                            cond.ArgumentNot = true;
                        }

                        cond.Modifier = verb.word;
                        cond.Argument = argument.word;
                        rule.TargetCondition = cond;
                        rule.TargetName = target.word;
                    }
                    else if (parts.Count == 1)
                    {
                        var (word, index) = parts[0];
                        if (index > 0 && allParts[index - 1] == "not")
                        {
                            rule.TargetNot = true;
                        }
                        rule.TargetName = word;
                    }
                    else
                    {
                        var modifier = parts[0];
                        var target = parts[1];

                        cond.Modifier = modifier.word;
                        rule.TargetName = target.word;

                        if (modifier.index > 0 && allParts[modifier.index - 1] == "not")
                        {
                            cond.Not = true;
                        }
                        if (target.index > 0 && allParts[target.index - 1] == "not")
                        {
                            rule.TargetNot = true;
                        }
                    }

                }
            }

            {
                // Feature
                var featureParts = featureSubPhrase.Split(' ').ToList();
                if (featureParts.Count == 1)
                {
                    rule.Feature = featureSubPhrase;
                }
                else if (featureParts[0] == "not")
                {
                    rule.FeatureNot = true;
                    rule.Feature = featureParts[1];
                }
                else
                {
                    return null;
                }
            }

            return rule;
        }


        public struct FullRule
        {
            public string TargetName;
            public bool TargetNot;

            public TargetCondition? TargetCondition;
            public string Verb;
            public string Feature;
            public bool FeatureNot;

            public string[] Tags;

            public override bool Equals(object? obj)
            {
                if (obj is FullRule fr)
                {
                    return fr.Feature == Feature && fr.TargetName == TargetName && fr.TargetNot == TargetNot && fr.TargetCondition?.Equals(TargetCondition) != false &&
                        fr.Verb == Verb && fr.FeatureNot == FeatureNot;
                }

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(TargetName, TargetNot, TargetCondition, Verb, Feature, FeatureNot, Tags);
            }

            public override string ToString()
            {
                return string.Join(
                    ' ', 
                    new[] { TargetNot ? "not" : "", TargetName, TargetCondition.ToString(), Verb, FeatureNot ? "not" : "", Feature }
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                    );
            }
        }

        public struct TargetCondition
        {
            public bool Not;
            public string Modifier;

            public bool ArgumentNot;
            public string? Argument;

            public string FullModifier => Not ? $"not {Modifier}" : Modifier;

            public override bool Equals(object? obj)
            {
                if (obj is TargetCondition tc)
                {
                    return tc.Not == Not && tc.Modifier == Modifier && tc.ArgumentNot == ArgumentNot && tc.Argument == Argument;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return string.Join(
                    ' ',
                    new[] { Not ? "not" : "", Modifier, ArgumentNot ? "not": "", Argument }
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                    );
            }
        }
    }
}
