using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Engine;


public class SerializeBytes
{
    private static readonly Dictionary<Type, Func<object?, byte[]>> serializers = new()
    {
        { typeof(int),  getSerializerMethod<int>() },
        { typeof(short),  getSerializerMethod<short>() },
        { typeof(long),  getSerializerMethod<long>() },
        { typeof(bool),  getSerializerMethod<bool>() },
        { typeof(string),  getSerializerMethod<string>() },
    };
    private static readonly Dictionary<Type, Func<Enumerator<byte>, object>> deserializers = new()
    {
        { typeof(int),  (Enumerator<byte> x) => BitConverter.ToInt32(take(x, 4)) },
        { typeof(short),  (Enumerator<byte> x) => BitConverter.ToInt16(take(x, 2)) },
        { typeof(long),  (Enumerator<byte> x) => BitConverter.ToInt64(take(x, 8)) },
        { typeof(bool),  (Enumerator<byte> x) => BitConverter.ToBoolean(take(x, 1)) },
    };

    private static Func<object?, byte[]> getSerializerMethod<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return (object? x) => serializeString(x as string);
        }
        var method = typeof(BitConverter).GetMethods().First(x => x.Name == "GetBytes" && x.GetParameters().First().ParameterType == typeof(T));
        return (object? x) => (byte[])method.Invoke(null, new[] { x })!;
    }

    private static byte[] serializeString(string? str) => str == null ? BitConverter.GetBytes((int)0) : BitConverter.GetBytes(str.Length).Concat(Encoding.UTF8.GetBytes(str)).ToArray();

    private static string? deserializeString(Enumerator<byte> input)
    {
        var len = BitConverter.ToInt32(take(input, 4));
        if (len == 0) return null;
        var bytes = take(input, len);
        return Encoding.UTF8.GetString(bytes);
    }

    private static T[] take<T>(Enumerator<T> enumerator, int count)
    {
        var bytes = new T[count];
        if (count == 0) 
            return bytes;

        var c = 0;
        while (enumerator.MoveNext())
        {
            bytes[c++] = enumerator.Current;
            if (c >= count) break;
        }

        return bytes;
    }

    private static Type _underlying(Type t) => t.IsEnum ? Enum.GetUnderlyingType(t) : t;

    public static string SerializeObjects<T>(IEnumerable<T> objects)
    {
        var fields = typeof(T).GetFields();
        var props = typeof(T).GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();

        var bytes = new List<byte>();

        foreach (var obj in objects)
        {
            foreach (var field in fields)
            {
                if (serializers.TryGetValue(_underlying(field.FieldType), out var serializer))
                    bytes.AddRange(serializer(field.GetValue(obj)));
            }
            foreach (var prop in props)
            {
                if (serializers.TryGetValue(_underlying(prop.PropertyType), out var serializer))
                    bytes.AddRange(serializer(prop.GetValue(obj)));
            }
        }

        var str = Convert.ToBase64String(bytes.ToArray());
        return formatStr(str);
    }

    private static string normalize(string str) => string.Join("", str.Split('\n'));

    private static string formatStr(string str)
    {
        var output = new StringBuilder();

        for (var i = 0; i < str.Length; i += 64)
        {
            if (str.Length - i < 64) output.Append(str[i..]);
            else output.Append(str[i..(i + 64)] + "\n");
        }

        return output.ToString().Trim();
    }

    public static List<T> DeserializeObjects<T>(string? str) where T : new()
    {
        if (str == null) return new();

        var actualString = string.Join("", str.Trim().Split('\n', '\r'));
        List<byte> bytes;
        bytes = Convert.FromBase64String(actualString.Replace("\n", "").Replace("\r", "")).ToList();

        var fields = typeof(T).GetFields();
        var props = typeof(T).GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();

        var enumerator = new Enumerator<byte>(bytes);

        var arr = new List<T>();

        while (enumerator.HasNext())
        {
            var item = (object)new T();
            foreach (var field in fields)
            {
                object? value;
                if (deserializers.TryGetValue(_underlying(field.FieldType), out var deserializer))
                {
                    value = deserializer(enumerator);
                }
                else if (field.FieldType == typeof(string))
                {
                    value = deserializeString(enumerator);
                }
                else
                    continue;
                field.SetValue(item, value);
            }
            foreach (var prop in props)
            {
                object? value;
                if (deserializers.TryGetValue(_underlying(prop.PropertyType), out var deserializer))
                {
                    value = deserializer(enumerator);
                }
                else if (prop.PropertyType == typeof(string))
                {
                    value = deserializeString(enumerator);
                }
                else
                    continue;
                prop.SetValue(item, value);
            }
            arr.Add((T)item);
        }

        return arr;
    }

    public static string JoinSerializedStrings(params string[] strings)
    {
        var sb = new StringBuilder();
        foreach (var str in strings)
        {
            var norm = normalize(str);
            var len = BitConverter.GetBytes(norm.Length);
            sb.Append(Convert.ToBase64String(len).TrimEnd('='));
            sb.Append(norm);
        }
        return formatStr(sb.ToString());
    }

    public static string[] SplitSerializedStrings(string joined)
    {
        var enumerator = new Enumerator<char>(normalize(joined));
        var strings = new List<string>();
        while (enumerator.HasNext())
        {
            var s = new string(take(enumerator, 6));
            var bytes = Convert.FromBase64String(s.PadRight(8, '='));
            var len = BitConverter.ToInt32(bytes);
            strings.Add(new string(take(enumerator, len)).Replace("\n", ""));
        }
        return strings.ToArray();
    }

    [DebuggerDisplay("Enumerator at {index} = {Current}")]
    private class Enumerator<T>
    {
        private readonly List<T> items;
        public int index = -1;

        public T Current => items[index];

        public Enumerator(IEnumerable<T> items)
        {
            this.items = items.ToList();
        }

        public bool MoveNext()
        {
            return ++index < items.Count;
        }

        public bool HasNext() => index < items.Count - 1;
    }
}
