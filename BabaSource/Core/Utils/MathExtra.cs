using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class MathExtra
    {
        public static int MathMod(int a, int b)
        {
            return (Math.Abs(a * b) + a) % b;
        }
    }
}
