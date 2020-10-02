using System;

namespace FlameSystems.Compon.Base
{
    public struct ValueModel
    {
        public string Name { get; }
        public object Data { get; set; }
        public Type TypeOf { get; }

        public ValueModel(string name, object data, Type typeOf)
        {
            Name = name;
            Data = data;
            TypeOf = typeOf;
        }
    }
}