using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class ColorExtensions
    {
        public static string ToHexTriple(this Color c)
        {
            return $"[{c.R:x2},{c.G:x2},{c.B:x2}]";
        }
    }
}
