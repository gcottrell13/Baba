using Core.Content;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Engine
{
    public static class SemanticFilter
    {
        public static string AppendText_ToName(string str) => str.StartsWith("text_") ? str : $"text_{str}";
        /// <summary>
        /// constructs a set of strings that all begin with "text_", adding it to the string if it's missing
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HashSet<string> AppendText_ToNames(IEnumerable<string> values) => new(values.Select(AppendText_ToName));

        public static readonly HashSet<string> relations = AppendText_ToNames(new[] { "on", "nextto", "feeling", "above", "below" });
        public static readonly HashSet<string> verbs = AppendText_ToNames(new[] { "is", "has", "make", "write", "fear", "eat", "follow", "mimic" });
        public static readonly HashSet<string> conjunctions = AppendText_ToNames(new[] { "and" });
        public static readonly HashSet<string> modifiers = AppendText_ToNames(new[] { "not", "lonely", "idle", "powered", "powered2", "powered3" });

        public static readonly Dictionary<string, string> text_characters = new[]
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "apos", "comma", "colon", "hyphen", "fwslash", "plus", "underscore", "rsqbr", "lsqbr", "quote", "period",
        }.ToDictionary(AppendText_ToName);

        public static readonly HashSet<string> adjectives = new(ObjectInfo.Info.Keys.Where(x => x.StartsWith("text_") && !ObjectInfo.Info.ContainsKey(x[5..]) && !relations.Contains(x) && !verbs.Contains(x) && !conjunctions.Contains(x) && !modifiers.Contains(x) && !text_characters.ContainsKey(x)));

        public static readonly HashSet<string> nouns = AppendText_ToNames(ObjectInfo.Info.Keys.Where(x => !x.StartsWith("text_")));

        private static readonly HashSet<string> nounOnlyVerbs = AppendText_ToNames(new[] { "eat", "has", "make", "mimic", "follow", "fear" });
        private static readonly HashSet<string> nounOnlyRelations = AppendText_ToNames(new[] { "on", "nextto", "above", "below" });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <returns></returns>
        private static IEnumerable<Rule<T>> findAndFilter<T>(List<T> grid) where T : INameable
        {
            //if (objectIsWord.Count > 0)
            //{
            //    grid = grid.Select(x => x.Select(y => y?.Name == default ? y : objectIsWord.TryGetValue(y.Name, out var replacement) ? replacement : y).ToList()).ToList();
            //}

            var vocabulary = new Vocabulary() { modifiers=modifiers, conjunctions=conjunctions, adjectives=adjectives, nouns=nouns, relations=relations, verbs=verbs, characters=text_characters };
            var sentences = ParseSentences.GetSentences(grid, vocabulary);

            foreach (var sentence in sentences)
            {
                var objects = sentence.Object.Items.Select(x => x.Item).Reverse().Append(sentence.Object.First).TakeWhile(obj =>
                {
                    if (obj is NA_WithRelationship<T> na)
                    {
                        // an object can only be "feeling" an adjective
                        if (nouns.Contains(na.RelatedTo.Value.Name) && na.Relation.Name == "text_feeling") return false;

                        // relations to objects only on the relations that allow objects
                        if (adjectives.Contains(na.RelatedTo.Value.Name) && nounOnlyRelations.Contains(na.Relation.Name)) return false;
                    }
                    // adjectives are not allowed as the object of a sentence, use "feeling"
                    if (adjectives.Contains(obj.Value.Name)) return false;
                    return true;
                }).ToList();

                if (objects.Count == 0) continue;

                var subjects = sentence.Subject.Items.Select(x => x.Item).Reverse().Append(sentence.Subject.First).Reverse().TakeWhile(sub =>
                {
                    // some verbs only make sense for nouns
                    if (nounOnlyVerbs.Contains(sentence.Verb.Name) && adjectives.Contains(sub.Value.Name)) return false;

                    // the subject of a sentence has to be simple, relations don't make sense
                    if (sub is NA_WithRelationship<T>) return false;

                    // the only modifier allowed in the subject is "not"
                    if (sub.Modifier?.Name != null && sub.Modifier.Name != "text_not" && adjectives.Contains(sub.Value.Name)) return false;
                    return true;
                }).ToList();

                if (subjects.Count == 0) continue;

                foreach (var (obj, sub) in objects.Product(subjects))
                {
                    yield return new(obj, sentence.Verb, sub);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static List<Rule<T>> FindRulesAndFilterInvalid<T>(List<T> grid) where T : INameable
        {
            return findAndFilter(grid).ToList();
        }
    }

    public struct Rule<T> where T : INameable
    {
        public NounAdjective<T> LHS;
        public Word<T> Verb;
        public NounAdjective<T> RHS; 

        public Rule(NounAdjective<T> LHS, Word<T> Verb, NounAdjective<T> RHS)
        {
            this.LHS = LHS;
            this.Verb = Verb;
            this.RHS = RHS;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Rule<T> r) return r.LHS.Equals(LHS) && r.RHS.Equals(RHS) && object.Equals(Verb, r.Verb);
            return base.Equals(obj);
        }

        public static bool operator ==(Rule<T> left, Rule<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rule<T> left, Rule<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public override string ToString() => $"{LHS} {Verb} {RHS}";
    }
}
