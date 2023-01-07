using Core.Utils;
using Editor.SaveFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Utils
{
    internal static class GridHelpers
    {
        /// <summary>
        /// produces a list of lines in the fashion of "A", "B", ... "Z", "AA", "AB", "AC" ...
        /// but the most significant letter is in the last line, the second most sig. in the second-to-last line, etc.
        /// where A is 0, Z is 25.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<string> GetColumnHeaders(uint min, uint max, uint forceLength = 0)
        {
            var magnitude = (int)Math.Log(max, 26) + 1;
            var lines = " ".Repeat(max - min)
                .Select((c, i) => EnumerableExtensions.ToColString((uint)i + min).PadLeft(magnitude))
                .ZipMany().ToList();

            if (forceLength > 0)
            {
                lines = new[] { "" }.Repeat((int)forceLength - lines.Count).Concat(lines).ToList();
            }

            return lines;
        }

        /// <summary>
        /// produces a list of strings counting up from min to max.
        /// all numbers will be +1.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<string> GetRowHeaders(uint min, uint max, uint forceLength = 0)
        {
            var magnitude = (int)Math.Log(max, 10) + 1;
            var lines = " ".Repeat(max - min)
                .Select((c, i) => (i + min + 1).ToString().PadLeft((int)Math.Max(magnitude, forceLength)) + " ");

            return lines.ToList();
        }
    }
}
