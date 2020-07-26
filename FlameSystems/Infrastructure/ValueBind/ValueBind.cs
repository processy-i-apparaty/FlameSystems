using System;

namespace FlameSystems.Infrastructure.ValueBind
{
    internal class ValueBind<T> : IValueBind
    {
        private readonly Action<string> _notifyAction;

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
        private bool _isLooped;
        private bool _isRanged;
        private T _max;
        private T _min;

        private T _value;

        public ValueBind(string name, Action<string> notify)
        {
            Name = name;
            _notifyAction = notify;
        }

        public bool IsNotify { get; set; } = true;
        public Action<string, object> Action { get; set; }
        public bool IsAction { get; set; } = false;
        public bool IsFrozen { get; set; } = false;
        public string Name { get; }

        public object Value
        {
            get => _value;
            set
            {
                _value = SetValue(IsFrozen ? _value : value);

                if (IsNotify)
                    _notifyAction(Name);

                if (IsAction && !IsFrozen)
                    Action?.Invoke(Name, _value);
            }
        }

        public void SetAttribute(ValueBindAttribute valueBindAttribute)
        {
            if (!double.IsNaN(valueBindAttribute.InitialDouble) && ValueType == typeof(double))
            {
                _value = (T) (object) valueBindAttribute.InitialDouble;
                if (double.IsNaN(valueBindAttribute.MinDouble) || double.IsNaN(valueBindAttribute.MaxDouble)) return;
                _max = (T) (object) valueBindAttribute.MaxDouble;
                _min = (T) (object) valueBindAttribute.MinDouble;
                _isLooped = valueBindAttribute.Looped;
                _isRanged = true;
            }
            else if (valueBindAttribute.InitialInt != int.MinValue && ValueType == typeof(int))
            {
                _value = (T) (object) valueBindAttribute.InitialInt;
                if (valueBindAttribute.MinInt == int.MinValue || valueBindAttribute.MaxInt == int.MinValue) return;
                _max = (T) (object) valueBindAttribute.MaxInt;
                _min = (T) (object) valueBindAttribute.MinInt;
                _isLooped = valueBindAttribute.Looped;
                _isRanged = true;
            }
            else if (!string.IsNullOrEmpty(valueBindAttribute.InitialString) && ValueType == typeof(string))
            {
                _value = (T) (object) valueBindAttribute.InitialString;
            }
            else if (ValueType == typeof(bool))
            {
                _value = (T) (object) valueBindAttribute.InitialBool;
            }
        }

        public void Randomize()
        {
            if (!_isRanged) return;
            if (ValueType == typeof(int)) Value = _random.Next((int) (object) _min, (int) (object) _max + 1);

            if (ValueType == typeof(double))
            {
                var min = (double) (object) _min;
                var max = (double) (object) _max;
                var dif = max - min;
                Value = _random.NextDouble() * dif + min;
            }
        }


        public Type ValueType => typeof(T);

        private T SetValue(object value)
        {
            var val = (T) value;
            switch (_isRanged)
            {
                case true:
                {
                    if (ValueType == typeof(int))
                    {
                        var valueInt = (int) value;
                        var mi = (int) (object) _min;
                        var ma = (int) (object) _max;
                        if (valueInt < mi && _isLooped)
                            valueInt = ma - (valueInt - mi);
                        else if (valueInt < mi && !_isLooped)
                            valueInt = mi;
                        else if (valueInt > ma && _isLooped)
                            valueInt = mi + (valueInt - ma);
                        else if (valueInt > ma && !_isLooped)
                            valueInt = ma;
                        return (T) (object) valueInt;
                    }

                    if (ValueType == typeof(double))
                    {
                        var valueDouble = (double) value;
                        var min = (double) (object) _min;
                        var max = (double) (object) _max;

                        if (_isLooped)
                        {
                            while (valueDouble < min) valueDouble = max - Math.Abs(min - valueDouble);
                            while (valueDouble > max) valueDouble = min + Math.Abs(max - valueDouble);
                        }
                        else
                        {
                            if (valueDouble < min) valueDouble = min;
                            if (valueDouble > max) valueDouble = max;
                        }

                        return (T) (object) valueDouble;
                    }

                    return val;
                }

                default:
                    return val;
            }
        }
    }
}