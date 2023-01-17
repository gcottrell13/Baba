using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class EnumerableExtensions
    {
        public static string Repeat(this string str, int count)
        {
            return string.Join("", Repeat<char>(str, count));
        }
        public static string Repeat(this string str, uint count)
        {
            return string.Join("", Repeat<char>(str, (int)count));
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> c, int count)
        {
            for (var i = 0; i < count; i++) { 
                foreach (var item in c) { yield return item; }
            }
        }

        private static Regex colrow = new Regex(@"([a-zA-Z]+)(\d+)");
        public static bool TryRowColToInt(this string size, out Vector2 dims)
        {
            dims = new(0, 0);

            var match = colrow.Match(size);
            if (!match.Success) return false;
            var col = (int)match.Groups[1].Value.Reverse().Select((c, i) => (c.ToString().ToLower()[0] - 'a' + 1) * Math.Pow(26, i)).Sum();
            var row = int.Parse(match.Groups[2].Value);
            dims = new(col, row);
            return true;
        }

        public static string ToColString(uint c)
        {
            var col = ((char)('A' + (c % 26))).ToString();
            if (c > 25)
            {
                col = ((char)('A' + (c / 26) - 1)).ToString() + col;
            }
            return col;
        }

        public static IEnumerable<string> ZipMany(this IEnumerable<string> sequences)
        {
            return ZipMany<char>(sequences).Select(c => string.Join("", c));
        }

        public static IEnumerable<ICollection<T>> ZipMany<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            var enumerators = sequences.Select(x => x.GetEnumerator()).ToList();
            while (enumerators.Select(e => e.MoveNext()).ToList().All(x => x))
            {
                yield return enumerators.Select(e => e.Current).ToList();
            }
        }


        public static IEnumerable<(T1, T2)> Product<T1, T2>(this IEnumerable<T1> t1s, IEnumerable<T2> t2s)
        {
            foreach (var one in t1s)
            {
                foreach (var two in t2s)
                {
                    yield return (one, two);
                }
            }
        }
    }
}
