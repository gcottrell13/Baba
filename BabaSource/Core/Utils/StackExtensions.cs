using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils;

public static class StackExtensions
{
    public static void PushMany<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        foreach (var item in items) { stack.Push(item); }
    }
}
