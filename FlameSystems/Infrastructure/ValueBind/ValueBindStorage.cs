using System;
using System.Collections.Generic;
using FlameSystems.Connectors;
using FlameSystems.Connectors.Base;

namespace FlameSystems.Infrastructure.ValueBind
{
    public class ValueBindStorage
    {
        private readonly Action<string> _notifyAction;

        private readonly Dictionary<string, IValueBind> _valueBinds = new Dictionary<string, IValueBind>();
        private readonly Dictionary<string, IAction> _actionBinds = new Dictionary<string, IAction>();

        public ValueBindStorage(Action<string> notifyAction)
        {
            _notifyAction = notifyAction;
        }

        public void Add<T>(string name)
        {
            if (TryGetBind(name, out _)) return;
            var v = new ValueBind<T>(name, _notifyAction);
            _valueBinds.Add(name, v);
        }

        public void Add(IValueBind valueBind)
        {
            if (TryGetBind(valueBind.Name, out _)) return;
            _valueBinds.Add(valueBind.Name, valueBind);
        }

        public void Add<T>(string name, T t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (_valueBinds.ContainsKey(name)) return;
            var v = new ValueBind<T>(name, _notifyAction);
            _valueBinds.Add(name, v);
        }


        public void SetAction(IAction action)
        {
            try
            {
                var nam = action.NameOf;
                if (!_actionBinds.ContainsKey(nam))
                {
                    var act = _actionBinds[nam];
                    _actionBinds.Add(nam, act);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public IAction GetAction(string name)
        {
            try
            {
                if (_actionBinds.ContainsKey(name))
                {
                    var action = _actionBinds[name];
                    var arguments = action.ListArguments;
                    return action;
                }

                return new BaseAction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BaseAction();
            }
        }

        public IValue GetValue(string name)
        {
            try
            {
                if (_valueBinds.ContainsKey(name))
                {
                    var obj = _valueBinds[name];
                    var typ = obj.ValueType;
                    return new BaseValue(name, typ, obj.Value);
                }

                return new BaseValue();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BaseValue();
            }
        }

        public object Get(string name)
        {
            return _valueBinds.ContainsKey(name) ? _valueBinds[name].Value : default;
        }

        public Type Type(string name)
        {
            return _valueBinds.ContainsKey(name) ? _valueBinds[name].ValueType : null;
        }

        public void Remove(string name)
        {
            if (!TryGetBind(name, out var bind)) return;
            _valueBinds.Remove(bind.Name);
        }

        public void Set<T>(string name, T value)
        {
            if (!TryGetBind(name, out var bind)) return;
            if (bind.ValueType == typeof(T)) bind.Value = value;
        }

        public void SetIValue(IValue value)
        {
            if (!TryGetBind(value.NameOf, out var bind)) return;
            if (bind.ValueType == value.TypeOf) bind.Value = value.ObjectOf;
        }

        //value deprecated
        public void Set<T>(string name, T value, Type t)
        {
            if (!TryGetBind(name, out var bind)) return;
            if (bind.ValueType == t) bind.Value = value;
        }

        public void SetActionFor(string name, Action<string, object> action)
        {
            if (!TryGetBind(name, out var bind)) return;
            bind.Action = action;
            bind.IsAction = true;
        }


        public void SetActionFor(Action<string, object> action, List<string> names)
        {
            foreach (var name in names) SetActionFor(name, action);
        }

        public void SetActionFor(Action<string, object> action, params string[] names)
        {
            foreach (var name in names) SetActionFor(name, action);
        }

        public void SetActionAll(Action<string, object> action)
        {
            foreach (var name in _valueBinds.Keys) SetActionFor(name, action);
        }

        public void TurnNotifierFor(string name, bool state)
        {
            if (!TryGetBind(name, out var bind)) return;
            bind.IsNotify = state;
        }

        public void TurnNotifierForAll(bool state)
        {
            foreach (var bind in _valueBinds.Values) bind.IsNotify = state;
        }

        public void TurnActionFor(string name, bool state)
        {
            if (!TryGetBind(name, out var bind)) return;
            bind.IsAction = state;
        }

        public void TurnActionFor(bool state, params string[] names)
        {
            foreach (var name in names) TurnActionFor(name, state);
        }

        public void TurnActionFor(bool state, List<string> names)
        {
            foreach (var name in names) TurnActionFor(name, state);
        }

        public void TurnActionForAll(bool state)
        {
            foreach (var bind in _valueBinds.Values) bind.IsAction = state;
        }

        public void FreezeFor(string name, bool state)
        {
            if (!TryGetBind(name, out var bind)) return;
            bind.IsFrozen = state;
        }

        public void FreezeFor(bool state, params string[] names)
        {
            foreach (var name in names) FreezeFor(name, state);
        }

        private bool TryGetBind(string name, out IValueBind bind)
        {
            bind = null;
            if (!_valueBinds.ContainsKey(name)) return false;
            bind = _valueBinds[name];
            return true;
        }

        public void RandomizeFor(string name)
        {
            if (!TryGetBind(name, out var bind)) return;
            bind.Randomize();
        }

        public void RandomizeFor(string[] names)
        {
            foreach (var name in names) RandomizeFor(name);
        }

        public bool TryGetDoubles(string[] names, out double[] doubles)
        {
            var typeOfDouble = typeof(double);
            doubles = new double[names.Length];
            for (var i = 0; i < doubles.Length; i++)
            {
                if (!TryGetBind(names[i], out var bind)) return false;
                if (bind.ValueType != typeOfDouble) return false;
                doubles[i] = (double) bind.Value;
            }

            return true;
        }
    }
}