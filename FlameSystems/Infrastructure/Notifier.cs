using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Infrastructure
{
    internal abstract class Notifier : INotifyPropertyChanged
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
            // ConsoleReport.Report($"{propertyName} {PropertyChanged}", MethodBase.GetCurrentMethod());
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected dynamic Get([CallerMemberName] string propertyName = "")
        {
            var obj = BindStorage.Get(propertyName);
            var type = obj?.GetType();
            var dyn = Convert.ChangeType(obj, type ?? throw new InvalidOperationException());
            // ConsoleReport.Report($"{propertyName} [{dyn}]", MethodBase.GetCurrentMethod());
            return dyn;
        }

        protected void Set<T>(T obj, [CallerMemberName] string propertyName = "")
        {
            // ConsoleReport.Report($"{propertyName} [{typeof(T)} : {obj}]", MethodBase.GetCurrentMethod());
            BindStorage.Set(propertyName, obj);
        }

        public static dynamic DynamicCast(dynamic source, Type dest)
        {
            var dyn = Convert.ChangeType(source, dest);
            return dyn;
        }

        // private static dynamic DynamicCast(object entity, Type to)
        // {
        //     if (entity == null || to == null) return null;
        //     var openCast = typeof(DynaCast).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public);
        //     var closeCast = openCast?.MakeGenericMethod(to);
        //     return closeCast?.Invoke(entity, new[] {entity});
        // }
    }
}