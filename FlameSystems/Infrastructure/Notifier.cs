using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Infrastructure
{
    public abstract class Notifier : INotifyPropertyChanged
    {
        protected ValueBindStorage BindStorage;

        protected Notifier()
        {
            BindStorage = new ValueBindStorage(Notify);

            var act = new Action<string>(Notify);
            var props = GetType().GetProperties();
            var attr = new ValueBindAttribute();
            foreach (var propertyInfo in props)
            {
                var attributes = propertyInfo.GetCustomAttributes(true);
                if (attributes.FirstOrDefault(x => x.GetType() == typeof(ValueBindAttribute)) == null) continue;

                var propertyType = propertyInfo.PropertyType;
                var dataType = new[] {propertyType};
                var genericBase = typeof(ValueBind<>);
                var combinedType = genericBase.MakeGenericType(dataType);
                var instance = (IValueBind) Activator.CreateInstance(combinedType, propertyInfo.Name, act);
                if (instance == null) continue;
                var vba = (ValueBindAttribute) attributes.FirstOrDefault(x => x.GetType() == attr.GetType());
                if (vba != null) instance.SetAttribute(vba);

                BindStorage.Add(instance);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected dynamic Get([CallerMemberName] string propertyName = "")
        {
            var obj = BindStorage.Get(propertyName);
            var type = obj?.GetType();
            if (type == null) return null;

            var dyn = Convert.ChangeType(obj, type);
            return dyn;
        }

        protected void Set<T>(T obj, [CallerMemberName] string propertyName = "")
        {
            BindStorage.Set(propertyName, obj);
        }

    }
}