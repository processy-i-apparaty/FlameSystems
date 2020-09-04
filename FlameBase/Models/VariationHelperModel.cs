using System;
using System.Windows;

namespace FlameBase.Models
{
    public class VariationHelperModel
    {
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
        public double Omega => _random.NextDouble() * Math.PI;
        public double Psi => _random.NextDouble();
        public double TwoPi { get; } = Math.PI * 2.0;
        public double HalfPi { get; } = Math.PI * .5;
        public double OneRadOfQuadrant { get; } = 1.0 / (Math.PI * .5);
        public double SmallDouble => 1.0E-300;
        public double SmallDouble2 => 1.0E-8;


        public double R(Point p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        public int RandomInt(int from, int to)
        {
            return _random.Next(from, to);
        }

        public double Phi(Point p)
        {
            return Math.Atan(p.Y / p.X);
        }

        public string GetString(VariationModel variation)
        {
            return $"[{variation.Id:00}] {variation.Name}";
        }

        public double PreAtan(Point p)
        {
            return Math.Atan2(p.X, p.Y);
        }

        public double PreAtanYx(Point p)
        {
            return Math.Atan2(p.Y, p.X);
        }

        public double PreSqrt(Point p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y) + 1.0E-300;
        }

        public double PreSinA(Point p, double preSqrt)
        {
            return p.X / preSqrt;
        }

        public double PreCosA(Point p, double preSqrt)
        {
            return p.Y / preSqrt;
        }

        public double PreSumSq(Point p)
        {
            return p.X * p.X + p.Y * p.Y;
        }

        public int RandomNext(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public double Frac(double value)
        {
            return value - Math.Truncate(value);
        }

        public double Sqr(double value)
        {
            return value * value;
        }

        public double Fmod(double value1, double value2)
        {
            return value1 % value2;
        }

        public double Rint(double value)
        {
            var v = Frac(value);
            return v <= .5 ? Math.Floor(value) : Math.Ceiling(value);
        }

        public double Map(double value, double iStart, double iStop, double oStart, double oStop)
        {
            return oStart + (oStop - oStart) * ((value - iStart) / (iStop - iStart));
        }

        public double Acosh(double n)
        {
            return Math.Log(Math.Sqrt(Math.Pow(n, 2.0) - 1.0) + n);
        }

        public double SqrtSafe(double n)
        {
            return n <= 0.0 ? 0.0 : Math.Sqrt(n);
        }

        public double Hypot(double abs, double abs2)
        {
            abs = Math.Abs(abs);
            abs2 = Math.Abs(abs2);
            if (abs2 < abs)
            {
                var n = abs;
                abs = abs2;
                abs2 = n;
            }
            else if (abs2 < abs)
            {
                if (double.IsPositiveInfinity(abs) || double.IsPositiveInfinity(abs2)) return double.PositiveInfinity;
                return double.NaN;
            }

            if (Math.Abs(abs2 - abs - abs2) <= SmallDouble) return abs2;
            double n2;
            if (abs > 2.9073548971824276E135)
            {
                abs *= 1.688508503057271E-226;
                abs2 *= 1.688508503057271E-226;
                n2 = 5.922386521532856E225;
            }
            else if (abs2 < 3.4395525670743494E-136)
            {
                abs *= 5.922386521532856E225;
                abs2 *= 5.922386521532856E225;
                n2 = 1.688508503057271E-226;
            }
            else
            {
                n2 = 1.0;
            }

            return n2 * Math.Sqrt(abs * abs + abs2 * abs2);
        }
    }
}