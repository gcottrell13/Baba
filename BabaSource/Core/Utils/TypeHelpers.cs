using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils;

public static class TypeHelpers
{
    public static bool IsDictionary(this Type type, out Type keyType, out Type valueType)
    {
        keyType = null;
        valueType = null;
        if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            keyType = type.GenericTypeArguments[0];
            valueType = type.GenericTypeArguments[1];
            return true;
        }
        return false;
    }
}
