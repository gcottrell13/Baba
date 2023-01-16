using g3;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Engine;

public interface INameable
{
    string Name { get; }
}

public class NounAdjective<T>
{
    public T Value;
    public T? Modifier;

    public NounAdjective(T value)
    {
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is NounAdjective<T> na) 
            return Equals(Value, na.Value) && Equals(Modifier, na.Modifier);
        return base.Equals(obj);
    }

    public override string ToString() => $"{Modifier}{Value}";

    public override int GetHashCode() => ToString().GetHashCode();
}

public class NA_WithRelationship<T> : NounAdjective<T>
{
    public T Relation;
    public NounAdjective<T> RelatedTo;

    public NA_WithRelationship(T value, T relation, NounAdjective<T> relatedTo) : base(value)
    {
        Relation = relation;
        RelatedTo = relatedTo;
    }

    public override bool Equals(object? obj)
    {
        if (obj is NA_WithRelationship<T> na) 
            return Equals(Relation, na.Relation) && RelatedTo.Equals(na.RelatedTo) && base.Equals(obj);
        return base.Equals(obj);
    }

    public override string ToString() => $"{base.ToString()} {Relation} {RelatedTo}";

    public override int GetHashCode() => ToString().GetHashCode();
}

public class Conjunction<T>
{
    public T Conj;
    public NounAdjective<T> Item;

    public Conjunction(T conj, NounAdjective<T> item)
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

public class Clause<T>
{
    public NounAdjective<T>? First;
    public List<Conjunction<T>> Items = new();

    public override bool Equals(object? obj)
    {
        if (obj is Clause<T> c) 
            return c.First?.Equals(First) == true && Items.Zip(c.Items).All(x => x.First.Equals(x.Second));
        return base.Equals(obj);
    }

    public override string ToString() => $"{First}" + string.Join(" ", Items);

    public override int GetHashCode() => ToString().GetHashCode();
}

public class Sentence<T>
{
    public Clause<T> Object;
    public T Verb;
    public Clause<T> Subject;

    public Sentence(Clause<T> @object, T verb, Clause<T> subject)
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


internal class VocabSet : HashSet<string?>
{
    public VocabSet(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static VocabSet operator &(VocabSet v, HashSet<string?> one)
    {
        var c = new VocabSet(v.Name);
        foreach (var item in v.Union(one))
        {
            c.Add(item);
        }
        return c;
    }
}

public class Vocabulary
{
    internal VocabSet Verbs = new("Verbs");
    internal VocabSet Adjectives = new("Adjectives");
    internal VocabSet Nouns = new("Nouns");
    internal VocabSet Modifiers = new("Modifiers");
    internal VocabSet Conjunctions = new("Conjunctions");
    internal VocabSet Relations = new("Relations");
    internal VocabSet Total = new("Total");

    public HashSet<string> verbs { set { Verbs &= value!; Total &= value!; } }
    public HashSet<string> adjectives { set { Adjectives &= value!; Total &= value!; } }
    public HashSet<string> nouns { set { Nouns &= value!; Total &= value!; } }
    public HashSet<string> modifiers { set { Modifiers &= value!; Total &= value!; } }
    public HashSet<string> conjunctions { set { Conjunctions &= value!; Total &= value!; } }
    public HashSet<string> relations { set { Relations &= value!; Total &= value!; } }
}


public class ParseSentences
{

    private static IEnumerable<(int index, T item)> enumerate<T>(IEnumerable<T> collection)
    {
        foreach (var (index, item) in collection.Select((t, i) => (i, t)))
        {
            yield return (index, item);
        }
    }

    public static List<T[]> GetWordChains<T>(List<List<T?>> grid, HashSet<string?> words) where T : INameable
    {
        List<(int x, int y, string dir)> starts = new();
        foreach (var (x, col) in enumerate(grid))
        {
            foreach (var (y, word) in enumerate(col))
            {
                if (word == null || words.Contains(word.Name) == false) continue;

                if (x < grid.Count - 2 && words.Contains(grid[x + 1][y]?.Name))
                    starts.Add((x, y, "right"));

                if (y < grid[x].Count - 2 && words.Contains(grid[x][y + 1]?.Name))
                    starts.Add((x, y, "down"));
            }
        }

        HashSet<(int, int, string)> consumed = new();
        List<T[]> chains = new();

        foreach (var m0 in starts)
        {
            var (x, y, dir) = m0;
            var chain = new List<T>();

            var m = (x, y, dir);

            while (x < grid.Count && y < grid[x].Count && grid[x][y] is T t && words.Contains(t.Name))
            {
                if (consumed.Contains(m))
                    goto Done;

                consumed.Add(m);

                chain.Add(t);

                if (dir == "down")
                    y += 1;
                else if (dir == "right")
                    x += 1;

                m = (x, y, dir);
            }

            chains.Add(chain.ToArray());

            Done: { }
        }

        return chains;
    }

    public static List<Sentence<T>> GetSentences<T>(List<List<T?>> grid, Vocabulary vocabulary) where T : INameable
    {
        var chains = new Stack<T[]>(GetWordChains(grid, vocabulary.Total).ToList());
        var sentences = new List<Sentence<T>>();
        while (chains.Count > 0)
        {
            var chain = chains.Pop();
            var i = 0;
            var current = chain;
            while (current.Length >= 3)
            {
                if (matchSentence(current, vocabulary) is Sentence<T> match)
                {
                    sentences.Add(match);
                    chains.Push(chain[(i + current.Length)..]);
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


    private static readonly Regex specifierRegex = new Regex(@"$M?[NA](RM?[NA])?^", RegexOptions.IgnoreCase);

    private static NounAdjective<T>? matchSpecifier<T>(T[] chain, HashSet<string?> exclude, Vocabulary vocabulary) where T : INameable
    {
        var re = new List<char>();
        foreach (var word in chain)
        {
            if (exclude.Contains(word.Name)) return null;
            else if (vocabulary.Nouns.Contains(word.Name)) re.Add('n');
            else if (vocabulary.Adjectives.Contains(word.Name)) re.Add('a');
            else if (vocabulary.Modifiers.Contains(word.Name)) re.Add('m');
            else if (vocabulary.Relations.Contains(word.Name)) re.Add('r');
            else return null;
        }
        var str = string.Join("", re);

        return str switch
        {
            "n" or "a" => new NounAdjective<T>(chain[0]),
            "mn" or "ma" => new NounAdjective<T>(chain[1]) { Modifier = chain[0] },
            "nrn" or "ara" or "arn" or "nra" => new NA_WithRelationship<T>(chain[0], chain[1], new(chain[2])),
            "mnrn" or "mara" or "marn" or "mnra" => new NA_WithRelationship<T>(chain[1], chain[2], new(chain[3])) { Modifier = chain[0] },
            "nrmn" or "arma" or "armn" or "nrma" => new NA_WithRelationship<T>(chain[0], chain[1], new(chain[3]) { Modifier = chain[2] }),
            "mnrmn" or "marma" or "marmn" or "mnrma" => new NA_WithRelationship<T>(chain[1], chain[2], new(chain[4]) { Modifier = chain[3] }) { Modifier = chain[0] },
            _ => null,
        };
    }

    private static Clause<T>? matchClause<T>(T[] chain, HashSet<string?> exclude, Vocabulary vocabulary) where T : INameable
    {
        var clause = new Clause<T>();
        int lastI = 0;
        T? lastConjunction = default;

        void addSpec(NounAdjective<T> spec, T? conj)
        {

            if (lastConjunction == null)
            {
                clause.First = spec;
            }
            else
            {
                clause.Items.Add(new(lastConjunction, spec));
            }
            lastConjunction = conj;
        }

        foreach (var (index, word) in chain.Select((t, i) => (i, t)))
        {
            if (!vocabulary.Conjunctions.Contains(word.Name)) continue;

            var spec = matchSpecifier(chain[lastI..index], exclude, vocabulary);
            if (spec == null) return null;
            addSpec(spec, word);
            lastI = index + 1;
        }

        var last = chain[lastI..];
        if (last.Length == 0) 
            return null;
        else
        {
            var spec = matchSpecifier(last, exclude, vocabulary);
            if (spec == null) return null;
            addSpec(spec, default);
        }

        return clause;
    }

    private static Sentence<T>? matchSentence<T>(T[] chain, Vocabulary vocabulary) where T : INameable
    {
        var alist = chain.ToList();
        if (alist.FindAll(x => vocabulary.Verbs.Contains(x.Name)).Count > 1) return null;

        var verbIndex = alist.FindIndex(0, x => vocabulary.Verbs.Contains(x.Name));
        if (verbIndex == -1) return null;

        var first = chain[..verbIndex];
        var second = chain[(verbIndex + 1)..];
        if (matchClause(first, new(), vocabulary) is Clause<T> m1 && matchClause(second, vocabulary.Relations, vocabulary) is Clause<T> m2)
        {
            return new(m1, chain[verbIndex], m2);
        }
        return null;
    }
}
