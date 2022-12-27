using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Configuration
{
    public interface IResourceHandle
    {
        string? Name { get; }
        object? Value { get; }
    }

    public class ResourceHandle<T> : IResourceHandle
        where T : class
    {
        public ResourceHandle(string? name)
        {
            Name = name;
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public T? Value { get; private set; }

        object? IResourceHandle.Value => Value;

        public string? Name { get; }
    }
}
