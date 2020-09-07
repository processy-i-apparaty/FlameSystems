using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class JuliaC : VariationModel
    {
        private double _dist;
        private double _im;
        private double _re;

        public JuliaC()
        {
            SetParameters(new[] {-7.5, 0.0, 1.0}, new[] {"re", "im", "dist"});
        }

        public override int Id { get; } = 142;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 1.0 / (_re + 1.0E-300);
            var n3 = _im / 100.0;
            var n4 = Math.Atan2(p.Y, p.X) +
                     VariationHelper.Fmod(VariationHelper.RandomInt(int.MinValue, int.MaxValue), _re) * 2.0 * Math.PI;
            var n5 = _dist * 0.5 * Math.Log(p.X * p.X + p.Y * p.Y);
            var n6 = n4 * n2 + n5 * n3;
            var sin = Math.Sin(n6);
            var cos = Math.Cos(n6);
            var exp = Math.Exp(n5 * n2 - n4 * n3);
            var x = W * exp * cos;
            var y = W * exp * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
            _re = P1;
            _im = P2;
            _dist = P3;
        }
    }
}