using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Configuration
{
    public interface IResourceHandle
    {
        string Name { get; }
        object Value { get; }
    }

    public class ResourceHandle<T> : IResourceHandle
    {
        internal ResourceHandle(string name)
        {
            Name = name;
        }

        internal void SetValue(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        object IResourceHandle.Value => Value;

        public string Name { get; }
    }
}
