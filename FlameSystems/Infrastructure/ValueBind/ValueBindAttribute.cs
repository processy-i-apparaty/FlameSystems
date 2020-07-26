using System;

namespace FlameSystems.Infrastructure.ValueBind
{
    internal class ValueBindAttribute : Attribute
    {
        public ValueBindAttribute(double initialDouble)
        {
            InitialDouble = initialDouble;
        }

        public ValueBindAttribute(int initialInt)
        {
            InitialInt = initialInt;
        }

        public ValueBindAttribute(string initialString)
        {
            InitialString = initialString;
        }

        public ValueBindAttribute(bool initialBool)
        {
            InitialBool = initialBool;
        }

        public ValueBindAttribute()
        {
        }

        public ValueBindAttribute(double initialDouble, double minDouble, double maxDouble,
            bool looped = false)
        {
            InitialDouble = initialDouble;
            MinDouble = minDouble;
            MaxDouble = maxDouble;
            Looped = looped;
        }

        public ValueBindAttribute(int initialInt, int minInt, int maxInt, bool looped = false)
        {
            InitialInt = initialInt;
            MaxInt = maxInt;
            MinInt = minInt;
            Looped = looped;
        }

        public bool InitialBool { get; set; }
        public int MinInt { get; set; } = int.MinValue;
        public int MaxInt { get; set; } = int.MinValue;
        public int InitialInt { get; } = int.MinValue;
        public double MaxDouble { get; } = double.NaN;
        public double MinDouble { get; } = double.NaN;
        public double InitialDouble { get; } = double.NaN;
        public string InitialString { get; }
        public bool Looped { get; }

        public override string ToString()
        {
            return $"InitialDouble: {InitialDouble}, InitialInt: {InitialInt}, InitialString: {InitialString}";
        }
    }
}