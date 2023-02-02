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
        public static ObjectTypeId AppendText_ToName(string str) => (ObjectTypeId)Enum.Parse(typeof(ObjectTypeId), str);

        /// <summary>
        /// constructs a set of strings that all begin with "text_", adding it to the string if it's missing
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HashSet<ObjectTypeId> AppendText_ToNames(IEnumerable<string> values) => new(values.Select(AppendText_ToName));

        public static readonly HashSet<ObjectTypeId> relations = AppendText_ToNames(new[] { "on", "nextto", "feeling", "above", "below" });
        public static readonly HashSet<ObjectTypeId> verbs = AppendText_ToNames(new[] { "is", "has", "make", "write", "fear", "eat", "follow", "mimic" });
        public static readonly HashSet<ObjectTypeId> conjunctions = AppendText_ToNames(new[] { "and" });
        public static readonly HashSet<ObjectTypeId> modifiers = AppendText_ToNames(new[] { "not", "lonely", "idle", "powered", "powered2", "powered3" });

        public static readonly Dictionary<ObjectTypeId, string> text_characters = new[]
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
            "_0", "_1", "_2", "_3", "_4", "_5", "_6", "_7", "_8", "_9",
            "apos", "comma", "colon", "hyphen", "fwslash", "plus", "underscore", "rsqbr", "lsqbr", "quote", "period",
        }.ToDictionary(AppendText_ToName);

        public static readonly HashSet<ObjectTypeId> adjectives = new(Enum.GetValues<ObjectTypeId>().Where(x =>
        {
            var name = Enum.GetName(typeof(ObjectTypeId), x);
            if (name == "all") return false;
            return name != null && !ObjectInfo.Info.ContainsKey(name) && !relations.Contains(x) && !verbs.Contains(x) && !conjunctions.Contains(x) && !modifiers.Contains(x) && !text_characters.ContainsKey(x);
        }));

        public static readonly HashSet<ObjectTypeId> nouns = AppendText_ToNames(ObjectInfo.Info.Keys.Where(x => !x.StartsWith("text_")).Append("all"));

        private static readonly HashSet<ObjectTypeId> nounOnlyVerbs = AppendText_ToNames(new[] { "eat", "has", "make", "mimic", "follow", "fear" });
        private static readonly HashSet<ObjectTypeId> nounOnlyRelations = AppendText_ToNames(new[] { "on", "nextto", "above", "below" });

        private static readonly HashSet<ObjectTypeId> complexSubjectNouns = AppendText_ToNames(new[] { "eat", "fear", "follow" });
        private static readonly HashSet<ObjectTypeId> subjectCannotBeNegatedNouns = AppendText_ToNames(new[] { "has", "make" });

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

            var vocabulary = new Vocabulary<ObjectTypeId>() { modifiers=modifiers, conjunctions=conjunctions, adjectives=adjectives, nouns=nouns, relations=relations, verbs=verbs, characters=text_characters };
            var sentences = ParseSentences.GetSentences(grid, vocabulary);

            foreach (var sentence in sentences)
            {
                var objects = new List<ISpecifier<T>>();
                foreach (var obj in sentence.Object.Items.Select(x => x.Item).Reverse().Append(sentence.Object.First))
                {
                    if (obj is NA_WithRelationship<T> wr)
                    {
                        if (!_checkNA(wr)) break;
                    }
                    else if (obj is NounAdjective<T> na)
                    {
                        // adjectives are not allowed as the object of a sentence, use "feeling"
                        if (adjectives.Contains(na.Value.Name)) break;
                    }
                    objects.Add(obj);
                }

                if (objects.Count == 0) continue;

                var subjects = new List<ISpecifier<T>>();
                foreach (var sub in sentence.Subject.Items.Select(x => x.Item).Reverse().Append(sentence.Subject.First).Reverse())
                {
                    // the subject of a sentence can only be complex with some verbs
                    if (sub is NA_WithRelationship<T> wr)
                    {
                        if (complexSubjectNouns.Contains(sentence.Verb.Name) == false) break;
                        if (!_checkNA(wr)) break;
                        subjects.Add(sub);
                        continue;
                    }

                    if (sub is not NounAdjective<T> na) break; // shouldn't happen though

                    // some verbs only make sense for nouns
                    if (nounOnlyVerbs.Contains(sentence.Verb.Name) && adjectives.Contains(na.Value.Name)) break;

                    // the only modifier allowed in the subject is "not"
                    if (sub.Modifier?.Name != null && sub.Modifier.Name != ObjectTypeId.not && adjectives.Contains(na.Value.Name)) break;

                    // you cannot HAS NOT something or MAKE NOT something
                    if (subjectCannotBeNegatedNouns.Contains(sentence.Verb.Name) && sub.Not) break;

                    subjects.Add(sub);
                }

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

        private static bool _checkNA<T>(NA_WithRelationship<T> wr) where T : INameable
        {
            var na = wr.RelatedTo as NounAdjective<T>;
            if (na == null) return false;

            // an object can only be "feeling" an adjective
            if (wr.Relation.Name == ObjectTypeId.feeling && nouns.Contains(na.Value.Name)) return false;

            // relations to objects only on the relations that allow objects
            if (adjectives.Contains(na.Value.Name) && nounOnlyRelations.Contains(wr.Relation.Name)) return false;
            return true;
        }
    }

    public struct Rule<T> where T : INameable
    {
        public ISpecifier<T> LHS;
        public Word<T> Verb;
        public ISpecifier<T> RHS; 

        public Rule(ISpecifier<T> LHS, Word<T> Verb, ISpecifier<T> RHS)
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
