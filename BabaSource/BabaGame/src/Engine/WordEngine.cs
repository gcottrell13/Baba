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
                    if (item.TargetCondition?.Modifier != "never")
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
                    if (item.TargetCondition?.Modifier != "never")
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

        public void AddRule(FullRule rule)
        {
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


        public struct FullRule
        {
            public string TargetName;
            public TargetCondition? TargetCondition;
            public string Verb;
            public string Feature;
            public string[] FeatureModifiers;

            public string[] Tags;
        }

        public struct TargetCondition
        {
            public string Modifier;
            public string? OtherObjectName;
        }
    }
}
