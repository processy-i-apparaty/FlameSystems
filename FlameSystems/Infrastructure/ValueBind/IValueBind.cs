using System;

namespace FlameSystems.Infrastructure.ValueBind
{
    internal interface IValueBind
    {
        Type ValueType { get; }
        string Name { get; }
        object Value { get; set; }
        bool IsNotify { get; set; }
        Action<string, object> Action { get; set; }
        bool IsAction { get; set; }
        bool IsFrozen { get; set; }
        void SetAttribute(ValueBindAttribute valueBindAttribute);
        void Randomize();
    }
}