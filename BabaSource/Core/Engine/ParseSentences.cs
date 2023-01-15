using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Engine;

public class ParseSentences
{
    public interface INameable
    {
        string Name { get; }
    }

    private static IEnumerable<(int index, T item)> enumerate<T>(IEnumerable<T> collection)
    {
        foreach (var (index, item) in collection.Select((t, i) => (i, t)))
        {
            yield return (index, item);
        }
    }

    public static List<List<T>> GetWordChains<T>(List<List<T?>> grid, HashSet<string?> words) where T : INameable
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
        List<List<T>> chains = new();

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

            chains.Add(chain);

            Done: { }
        }

        return chains;
    }
}
