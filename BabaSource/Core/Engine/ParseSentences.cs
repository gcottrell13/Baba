using Core.Content;
using Core.UI;
using Core.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Engine;

public interface INameable
{
    ObjectTypeId Name { get; }
    int X { get; }
    int Y { get; }
}

public class Word<T> where T : INameable
{
    private readonly T[]? Characters;
    private readonly T? Value;

    private Word(string name)
    {
        Name = Enum.Parse<ObjectTypeId>(name);
    }

    private Word(ObjectTypeId id)
    {
        Name = id;
    }

    public Word(T value, string name) : this(name)
    {
        Value = value;
    }

    public Word(IEnumerable<T> values, string name) : this(name)
    {
        Characters = values.ToArray();
    }

    public Word(T value) : this(value.Name)
    {
        Value = value;
    }

    public Word(IEnumerable<T> values) : this(string.Join("", values.Select(x => x.Name)))
    {
        Characters = values.ToArray();
    }

    public IEnumerable<T> Objects => Characters ?? new[] { Value! };

    public ObjectTypeId Name { get; private set; }

    public override bool Equals(object? obj)
    {
        if (obj is Word<T> word) return Name == word.Name;
        return base.Equals(obj);
    }

    public static implicit operator Word<T>(T s) => new(s);

    public override string ToString() => Name.ToString();

    public override int GetHashCode() => Name.GetHashCode();
}

public interface ISpecifier<T>
    where T : INameable
{
    bool Not { get; set; }
    Word<T>? Modifier { get; set; }

    IEnumerable<T> GetSentenceMembers();
}

public class NounAdjective<T> : ISpecifier<T>
    where T : INameable
{
    public Word<T> Value;
    public Word<T>? Modifier { get; set; }
    public bool Not { get; set; } = false;

    public NounAdjective(Word<T> value)
    {
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is NounAdjective<T> na) 
            return Equals(Value, na.Value) && Equals(Modifier, na.Modifier);
        return base.Equals(obj);
    }

    public override string ToString() => string.Join(" ", $"{_not()} {Modifier} {Value}".Split(" ", System.StringSplitOptions.RemoveEmptyEntries));

    public override int GetHashCode() => ToString().GetHashCode();

    private string _not() => Not ? "not" : "";

    public IEnumerable<T> GetSentenceMembers()
    {
        foreach (var u in Value.Objects)
            yield return u;
        if (Modifier != null)
            foreach (var u in Modifier.Objects)
                yield return u;
    }
}

public class NA_WithRelationship<T> : ISpecifier<T>
    where T : INameable
{
    public NounAdjective<T> Target;
    public bool Not { get; set; } = false;
    public Word<T> Relation;
    public Word<T>? Modifier { get; set; }
    public ISpecifier<T> RelatedTo;

    public NA_WithRelationship(Word<T> relation, ISpecifier<T> relatedTo)
    {
        Target = null!;
        Relation = relation;
        RelatedTo = relatedTo;
    }

    public override bool Equals(object? obj)
    {
        if (obj is NA_WithRelationship<T> na) 
            return Equals(Relation, na.Relation) && RelatedTo.Equals(na.RelatedTo) && Equals(Modifier, na.Modifier) && Not == na.Not && Target.Equals(na.Target);
        return base.Equals(obj);
    }

    private string _not() => Not ? "not" : "";

    public override string ToString() => string.Join(" ", $"{Target} {_not()} {Modifier} {Relation} {RelatedTo}".Split(" ", System.StringSplitOptions.RemoveEmptyEntries));

    public override int GetHashCode() => ToString().GetHashCode();

    public IEnumerable<T> GetSentenceMembers()
    {
        foreach (var u in Target.GetSentenceMembers())
            yield return u;
        if (Modifier != null)
            foreach (var u in Modifier.Objects)
                yield return u;
        foreach (var u in Relation.Objects)
            yield return u;
        foreach (var u in RelatedTo.GetSentenceMembers())
            yield return u;
    }
}

public class Conjunction<T> where T : INameable
{
    public Word<T> Conj;
    public ISpecifier<T> Item;

    public Conjunction(Word<T> conj, ISpecifier<T> item)
    {
        Conj = conj;
        Item = item;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Conjunction<T> c) 
            return Conj?.Equals(c.Conj) == true && Item.Equals(c.Item);
        return base.Equals(obj);
    }

    public override string ToString() => $"{Conj} {Item}";

    public override int GetHashCode() => ToString().GetHashCode();
}

public class Clause<T> where T : INameable
{
    public ISpecifier<T> First;
    public List<Conjunction<T>> Items = new();

    public Clause(ISpecifier<T> first)
    {
        First = first;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Clause<T> c) 
            return c.First?.Equals(First) == true && Items.Zip(c.Items).All(x => x.First.Equals(x.Second));
        return base.Equals(obj);
    }

    public override string ToString() => ($"{First} " + string.Join(" ", Items)).Trim();

    public override int GetHashCode() => ToString().GetHashCode();
}

public class Sentence<T> where T : INameable
{
    public Clause<T> Object;
    public Word<T> Verb;
    public Clause<T> Subject;

    public Sentence(Clause<T> @object, Word<T> verb, Clause<T> subject)
    {
        Object = @object;
        Verb = verb;
        Subject = subject;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Sentence<T> s) 
            return s.Object.Equals(Object) && Equals(s.Verb, Verb) && s.Subject.Equals(Subject);
        return base.Equals(obj);
    }

    public override string ToString() => $"{Object} {Verb} {Subject}";

    public override int GetHashCode() => ToString().GetHashCode();
}




internal class VocabSet<K> : HashSet<K?>
{
    public VocabSet(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static VocabSet<K> operator &(VocabSet<K> v, IEnumerable<K?> one)
    {
        var c = new VocabSet<K>(v.Name);
        foreach (var item in v.Union(one))
        {
            c.Add(item);
        }
        return c;
    }
}

public class Vocabulary<K> where K : notnull
{
    internal VocabSet<K> Verbs = new("Verbs");
    internal VocabSet<K> Adjectives = new("Adjectives");
    internal VocabSet<K> Nouns = new("Nouns");
    internal VocabSet<K> Modifiers = new("Modifiers");
    internal VocabSet<K> Conjunctions = new("Conjunctions");
    internal VocabSet<K> Relations = new("Relations");
    internal Dictionary<K, string> Characters = new();
    internal VocabSet<K> Total = new("Total");

    public HashSet<K> verbs { set { Verbs &= value!; Total &= value!; } }
    public HashSet<K> adjectives { set { Adjectives &= value!; Total &= value!; } }
    public HashSet<K> nouns { set { Nouns &= value!; Total &= value!; } }
    public HashSet<K> modifiers { set { Modifiers &= value!; Total &= value!; } }
    public HashSet<K> conjunctions { set { Conjunctions &= value!; Total &= value!; } }
    public HashSet<K> relations { set { Relations &= value!; Total &= value!; } }
    public Dictionary<K, string> characters { set { Characters = value; Total &= Characters.Keys; } }
}

internal class ConsumeCharacters<T> where T : INameable
{
    private readonly T[] chain;
    private readonly Vocabulary<ObjectTypeId> vocabulary;
    private int index = 0;

    public ConsumeCharacters(T[] chain, Vocabulary<ObjectTypeId> vocabulary)
    {
        this.chain = chain;
        this.vocabulary = vocabulary;
    }

    private IEnumerable<T> getNextWord()
    {
        if (index >= chain.Length) yield break;

        yield return chain[index++];
        while (index < chain.Length && vocabulary.Characters.ContainsKey(chain[index].Name))
        {
            yield return chain[index++];
        }
    }

    private Word<T>? next()
    {
        var word = getNextWord().ToArray();
        if (word.Length == 0) return null;
        if (word.Length == 1) return new(word[0]);
        return new(word);
    }

    public IEnumerable<Word<T>> ParseAll()
    {
        while (next() is Word<T> word) yield return word;
    }
}


public class ParseSentences
{
    /// <summary>
    /// The grid is assumed to contain only valid words
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="allObjects"></param>
    /// <returns></returns>
    public static List<T[]> GetWordChains<T>(IEnumerable<T> allObjects) where T : INameable
    {
        var grid = new Dictionary<(int x, int y), List<T>>();

        foreach (var word in allObjects)
        {
            var x = word.X;
            var y = word.Y;
            grid.ConstructDefaultValue((x, y)).Add(word);
        }

        var starts = new List<(T item, string direction)>();
        foreach (var word in allObjects)
        {
            if (grid.ContainsKey((word.X, word.Y - 1)) == false) starts.Add((word, "down"));
            if (grid.ContainsKey((word.X - 1, word.Y)) == false) starts.Add((word, "right"));
        }

        List<T[]> chains = new();

        foreach (var m0 in starts)
        {
            var chainsFromThisStart = new List<List<T>>();

            var (item, dir) = m0;
            var chain = new List<T>();
            var x = item.X;
            var y = item.Y;

            while (grid.TryGetValue((x, y), out var items))
            {
                if (chainsFromThisStart.Count == 0)
                {
                    chainsFromThisStart = items.Select(item => new List<T>() { item }).ToList();
                }
                else
                {
                    chainsFromThisStart = items.SelectMany(a => chainsFromThisStart.Select(list => list.Append(a).ToList())).ToList();
                }

                if (dir == "down")
                    y += 1;
                else if (dir == "right")
                    x += 1;
            }

            chains.AddRange(chainsFromThisStart.Where(x => x.Count >= 3).Select(x => x.ToArray()));
        }

        return chains;
    }

    public static List<Sentence<T>> GetSentences<T>(List<T> grid, Vocabulary<ObjectTypeId> vocabulary) where T : INameable
    {
        var chains = new Stack<T[]>(GetWordChains(grid.Where(item => vocabulary.Total.Contains(item.Name))));
        var sentences = new List<Sentence<T>>();
        while (chains.Count > 0)
        {
            var chain = chains.Pop().ToArray();
            var i = 0;
            var current = chain;
            while (current.Length >= 3)
            {
                var words = new ConsumeCharacters<T>(current, vocabulary).ParseAll().ToList();

                var matchedSentences = matchSentences(words, vocabulary);
                if (matchedSentences.Count > 0)
                {
                    sentences.AddRange(matchedSentences);
                    chains.Push(chain[(i + current.Length)..]);
                    chains.Push(chain[..i]);
                    break;
                }
                else if (current.Length > 3)
                {
                    current = current[..^1];
                }
                else
                {
                    i++;
                    current = chain[i..];
                }
            }
        }
        return sentences;
    }

    private static ISpecifier<T>? matchSpecifier<T>(Word<T>[] words, Vocabulary<ObjectTypeId> vocabulary) where T : INameable
    {
        var re = new List<char>();

        foreach (var word in words)
        {
            var name = word.Name;

            if (name == ObjectTypeId.not) re.Add('n');
            else if (vocabulary.Nouns.Contains(name)) re.Add('a');
            else if (vocabulary.Adjectives.Contains(name)) re.Add('a');
            else if (vocabulary.Modifiers.Contains(name)) re.Add('m');
            else if (vocabulary.Relations.Contains(name)) re.Add('r');
            else return null;
        }

        if (re.Count == 0) return null;

        if (re[^1] != 'a') return null;

        ISpecifier<T> current = new NounAdjective<T>(words[^1]);

        var i = re.Count - 1;
        while (i-- > 0)
        {
            var wr = current as NA_WithRelationship<T>;
            ISpecifier<T> target = wr?.Target != null ? wr.Target : current;

            switch (re[i])
            {
                case 'n': { target.Not = true; break; }
                case 'a': { if (wr != null) wr.Target = new(words[i]); else return null; break; }
                case 'r': { current = new NA_WithRelationship<T>(words[i], current); break; }
                case 'm': { target.Modifier = words[i]; break; }
                default: { return null; }
            }
        }
        return current;
    }

    private static Clause<T>? matchClause<T>(Word<T>[] words, Vocabulary<ObjectTypeId> vocabulary) where T : INameable
    {
        Clause<T>? clause = null;
        int lastI = 0;
        Word<T>? lastConjunction = default;

        void addSpec(ISpecifier<T> spec, Word<T>? conj)
        {
            if (clause == null)
            {
                clause = new(spec);
            }
            else
            {
                clause.Items.Add(new(lastConjunction!, spec));
            }
            lastConjunction = conj;
        }

        foreach (var (index, word) in words.Select((t, i) => (i, t)))
        {
            if (!vocabulary.Conjunctions.Contains(word.Name)) continue;

            var spec = matchSpecifier(words[lastI..index], vocabulary);
            if (spec == null) return null;
            addSpec(spec, word);
            lastI = index + 1;
        }

        var last = words[lastI..];
        if (last.Length == 0) 
            return null;
        else
        {
            var spec = matchSpecifier(last, vocabulary);
            if (spec == null) return null;
            addSpec(spec, default);
        }

        if (clause?.First == null) return null;

        return clause;
    }

    private static IList<Sentence<T>> matchSentences<T>(List<Word<T>> alist, Vocabulary<ObjectTypeId> vocabulary) where T : INameable
    {
        if (alist.FindAll(x => vocabulary.Verbs.Contains(x.Name)).Count < 1) return Array.Empty<Sentence<T>>();

        var clauses = new List<Clause<T>>();
        var verbs = new List<Word<T>>();

        var chain = alist.ToArray();

        var lastIndex = 0;
        int verbIndex;

        while ((verbIndex = alist.FindIndex(lastIndex, x => vocabulary.Verbs.Contains(x.Name))) != -1)
        {
            var clauseWords = chain[lastIndex..verbIndex];
            if (matchClause(clauseWords, vocabulary) is Clause<T> clause)
            {
                clauses.Add(clause);
                verbs.Add(chain[verbIndex]);
            }
            else
                return Array.Empty<Sentence<T>>();
            lastIndex = verbIndex + 1;
        }

        if (matchClause(chain[lastIndex..], vocabulary) is Clause<T> dclause)
        {
            clauses.Add(dclause);
        }

        return clauses.ZipThree(verbs, clauses.Skip(1)).Select(triple => new Sentence<T>(triple.Item1, triple.Item2, triple.Item3)).ToList();
    }
}
