using System;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Connectors.Base
{
    public class BaseValue : IValue
    {
        public BaseValue(string name, Type typ, object obj)
        {
            NameOf = name;
            TypeOf = typ;
            ObjectOf = obj;

        }

        public BaseValue()
        {
            NameOf = null;
            ObjectOf = null;
            TypeOf = null;
        }

        public string NameOf { get; set; }
        public object ObjectOf { get; set; }
        public Type TypeOf { get; set; }
    }
}