using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils;

public static class Serializers
{
    public static string Indent(this string s, int indent)
    {
        var t = "\t".Repeat(indent);
        return string.Join("\n", s.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(x => t + x));
    }

    public static string ToPrettyName(this Type type)
    {
        if (type.IsGenericType)
        {
            var args = type.GenericTypeArguments.Select(ToPrettyName).ToArray();
            var t = string.Join(", ", args);
            return $"{type.GetGenericTypeDefinition().Name}<{t}>";
        }
        return type.Name;
    }
}
