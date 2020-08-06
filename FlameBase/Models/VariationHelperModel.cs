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


        public double R(Point p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        public double Random()
        {
            return _random.NextDouble();
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
    }
}