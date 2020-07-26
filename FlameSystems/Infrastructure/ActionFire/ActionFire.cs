using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlameSystems.Infrastructure.ActionFire
{
    public static class ActionFire
    {
        private static readonly Dictionary<string, Delegate> Actions = new Dictionary<string, Delegate>();
        private static readonly Dictionary<string, Type> From = new Dictionary<string, Type>();
        private static readonly Type TypeObj = typeof(object);

        public static bool Add(string name, Delegate action, Type from)
        {
            if (TryGet(name, out _)) return false;
            Actions.Add(name, action);
            From.Add(name, from);
            return true;
        }

        public static void AddOrReplace(string name, Delegate action, Type from)
        {
            Remove(name);
            Actions.Add(name, action);
            From.Add(name, from);
        }

        public static bool Invoke(string name, params object[] parameters)
        {
            if (!TryGet(name, out var action)) return false;
            if (!Check(action, parameters)) return false;
            action.DynamicInvoke(parameters);
            return true;
        }

        public static ActionFireReturn<T> InvokeFunc<T>(string name, params object[] parameters)
        {
            if (!TryGet(name, out var action)) return new ActionFireReturn<T>(default, false);
            if (typeof(T) != action.GetMethodInfo().ReturnType) return new ActionFireReturn<T>(default, false);
            if (!Check(action, parameters)) return new ActionFireReturn<T>(default, false);
            return new ActionFireReturn<T>((T) action.DynamicInvoke(parameters), true);
        }

        public static bool Remove(string name)
        {
            if (!TryGet(name, out _)) return false;
            Actions.Remove(name);
            From.Remove(name);
            return true;
        }

        public static string GetAllActionNames()
        {
            var str = string.Empty;
            foreach (var pair in Actions)
            {
                str += $"{From[pair.Key]}.";
                str += $"{pair.Key}(";
                var parameters = pair.Value.Method.GetParameters();
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    str += parameter.ParameterType;
                    if (i < parameters.Length - 1)
                        str += ", ";
                }

                str += ")\n";
            }

            return str;
        }

        // private static bool Check(Delegate action, IReadOnlyList<object> parameters)
        // {
        //     if (parameters == null) return false;
        //     var methodParameters = action.Method.GetParameters();
        //     if (parameters.Count != methodParameters.Length) return false;
        //     for (var i = 0; i < parameters.Count; i++)
        //         if (parameters[i].GetType() != methodParameters[i].ParameterType)
        //             return false;
        //     return true;
        // }

        private static bool Check(Delegate action, IReadOnlyList<object> parameters)
        {
            if (parameters == null) return false;
            var methodParameters = action.Method.GetParameters();
            if (parameters.Count != methodParameters.Length) return false;
            for (var i = 0; i < parameters.Count; i++)
            {
                var t2 = methodParameters[i].ParameterType;
                if (t2 == TypeObj) continue;
                var t1 = parameters[i].GetType();
                if (t1 != t2) return false;
            }

            return true;
        }

        private static bool TryGet(string name, out Delegate action)
        {
            action = null;
            if (!Actions.ContainsKey(name)) return false;
            action = Actions[name];
            return true;
        }
    }
}