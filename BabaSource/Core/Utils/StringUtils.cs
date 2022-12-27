using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class StringUtils
    {
        public static string Repeat(this string str, int count)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++) { sb.Append(str); }
            return sb.ToString();
        }
    }
}
