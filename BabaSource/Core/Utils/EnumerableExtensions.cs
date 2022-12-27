using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
