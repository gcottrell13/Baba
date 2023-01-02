using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string ToRowColString(this Vector2 dims)
        {
            var col = "";
            for (var x = 1; x < dims.X; x *= 26) 
            {
                col += 'A' - 1 + ((dims.X / x) % 26);
            }
            return $"{col}{dims.Y}";
        }
    }
}
