using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlameBase.FlameMath;

namespace FlameBase.Models
{
    public class TransformModel
    {
        private readonly double[] _coefficients = new double[6];
        private readonly double[] _values = new double[7];

        public bool SetFromValues(double[] values, double probability, Color color, double colorPosition, bool isFinal)
        {
            if (values.Length != _values.Length) return false;
            for (var i = 0; i < _values.Length; i++) _values[i] = values[i];
            Probability = probability;
            Color = color;
            ColorPosition = colorPosition;
            CountCoefficients();
            IsFinal = isFinal;
            return true;
        }

        public bool SetFromCoefficients(double[] coefficients, double probability, Color color, bool isFinal,
            double colorPosition 
        )
        {
            if (coefficients.Length < _coefficients.Length) return false;
            for (var i = 0; i < _coefficients.Length; i++) _coefficients[i] = coefficients[i];
            Probability = probability;
            Color = color;
            ColorPosition = colorPosition;
            var values = CountValues();
            if (values == null) return false;
            for (var i = 0; i < _values.Length - 1; i++) _values[i] = values[i];
            AngleRadians = values[values.Length - 1];
            //todo: isFinal
            IsFinal = isFinal;
            return true;
        }

        public TransformModel Copy()
        {
            var copy = new TransformModel();
            copy.SetFromValues(_values, Probability, Color, ColorPosition, IsFinal);
            return copy;
        }

        #region private

        private double[] CountValues(double tolerance = Math.PI / 180.0)
        {
            double[] answer = null;
            var minDelta = double.MaxValue;
            for (var angle = 0.0; angle <= Math.PI * 2.0; angle += tolerance)
            {
                var values = SolveValues(angle, this);
                if (!CheckValues(values)) continue;
                var coefficients = CountCoefficients(values);
                var delta = CountDelta(values, coefficients);
                if (!(delta < minDelta)) continue;
                minDelta = delta;
                answer = values.ToArray();
            }

            return answer;
        }

        public static bool CheckValues(double[] values)
        {
            for (var i = 0; i < values.Length - 1; i++)
                if (Math.Abs(values[i]) > 2.0)
                    return false;
            return true;
        }

        private static double[] SolveValues(double angle, TransformModel t)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);
            var cos2 = Math.Cos(2.0 * angle);
            var sin2 = Math.Sin(2.0 * angle);
            var cosSq = cos * cos;

            var n1 = t.A * t.D + t.B * t.C + t.A * t.D * cos2 - t.B * t.C * cos2;
            var n2 = 2.0 * t.D * cos - 2.0 * t.C * sin;
            var scaleX = n1 / n2;
            FixNaN(ref scaleX);

            n2 = 2.0 * t.A * cos + 2.0 * t.B * sin;
            var scaleY = n1 / n2;
            FixNaN(ref scaleY);

            n1 = 2.0 * t.B * t.D + t.A * t.D * sin2 - t.B * t.C * sin2;
            n2 = 2.0 * t.B * t.C + 2 * t.A * t.D * cosSq - 2.0 * t.B * t.C * cosSq;
            var shearX = n1 / n2;
            FixNaN(ref shearX);

            n1 = 2.0 * t.A * t.C - t.A * t.D * sin2 + t.B * t.C * sin2;
            var shearY = n1 / n2;
            FixNaN(ref shearY);

            return new[] {t.E, t.F, scaleX, scaleY, shearX, shearY, angle};
        }

        private static void FixNaN(ref double value)
        {
            if (double.IsNaN(value)) value = 0;
        }


        private static double[] CountCoefficients(IReadOnlyList<double> values)
        {
            var shiftX = values[0];
            var shiftY = values[1];
            var scaleX = values[2];
            var scaleY = values[3];
            var shearX = values[4];
            var shearY = values[5];
            var angle = values[6];
            var result = new double[6];
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            result[0] = scaleX * cos + scaleX * -sin * shearY;
            result[1] = scaleX * cos * shearX + scaleX * -sin;
            result[2] = scaleY * sin + scaleY * cos * shearY;
            result[3] = scaleY * sin * shearX + scaleY * cos;
            result[4] = shiftX;
            result[5] = shiftY;
            return result;
        }

        private void CountCoefficients()
        {
            var coefficients = CountCoefficients(new[]
                {TranslateX, TranslateY, ScaleX, ScaleY, ShearX, ShearY, AngleRadians});
            for (var i = 0; i < _coefficients.Length; i++) _coefficients[i] = coefficients[i];
        }

        private static double CountDelta(IReadOnlyList<double> v1, IReadOnlyList<double> v2)
        {
            var dA = Math.Abs(v1[0] - v2[0]);
            var dB = Math.Abs(v1[1] - v2[1]);
            var dC = Math.Abs(v1[2] - v2[2]);
            var dD = Math.Abs(v1[3] - v2[3]);
            var delta = dA + dB + dC + dD;
            return delta;
        }

        #endregion

        #region properties

        public double A
        {
            get => _coefficients[0];
            set => _coefficients[0] = value;
        }

        public double B
        {
            get => _coefficients[1];
            set => _coefficients[1] = value;
        }

        public double C
        {
            get => _coefficients[2];
            set => _coefficients[2] = value;
        }

        public double D
        {
            get => _coefficients[3];
            set => _coefficients[3] = value;
        }

        public double E
        {
            get => _coefficients[4];
            set => _coefficients[4] = value;
        }

        public double F
        {
            get => _coefficients[5];
            set => _coefficients[5] = value;
        }

        public double Probability { get; set; }

        public Color Color { get; set; }

        public double TranslateX
        {
            get => _values[0];
            set => _values[0] = value;
        }

        public double TranslateY
        {
            get => _values[1];
            set => _values[1] = value;
        }

        public double ScaleX
        {
            get => _values[2];
            set => _values[2] = value;
        }

        public double ScaleY
        {
            get => _values[3];
            set => _values[3] = value;
        }

        public double ShearX
        {
            get => _values[4];
            set => _values[4] = value;
        }

        public double ShearY
        {
            get => _values[5];
            set => _values[5] = value;
        }

        public double Angle
        {
            get => _values[6];
            set => _values[6] = value;
        }

        public double AngleRadians
        {
            get => Angle * Trigonometry.ToRadians;
            set => Angle = value * Trigonometry.ToDegrees;
        }

        public Brush Brush => new SolidColorBrush(Color);
        public double ColorPosition { get; set; } = .5;

        public bool IsFinal { get; set; } = false;

        #endregion

        public bool CheckContractive()
        {
            var aSquare = A * A;
            var dSquare = D * D;
            if (aSquare + dSquare >= 1.0) return false;
            var bSquare = B * B;
            var eSquare = E * E;
            if (bSquare + eSquare >= 1.0) return false;
            var aedb = Math.Pow(A * E - D * B, 2.0);
            if (aSquare + bSquare + dSquare + eSquare >= 1.0 + aedb) return false;

            return true;
        }
    }
}