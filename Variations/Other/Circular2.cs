using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Circular2 : VariationModel
    {
        private double _seed;
        private double _xx;
        private double _yy;

        public Circular2()
        {
            SetParameters(new[] {VariationHelper.HalfPi, 0.0, 9.0, 9.5}, new[] {"angle", "seed", "xx", "yy"});
        }

        public override int Id { get; } = 82;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            // var n2 = P1 * Math.PI / 180.0;
            var n3 = Math.Sin(p.X * _xx + p.Y * _yy + _seed) * 43758.5453;
            var n4 = (2.0 * (VariationHelper.Psi + (n3 - (int) n3)) - 2.0) * P1;
            var sqrt = Math.Sqrt(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y));
            var atan2 = Math.Atan2(p.Y, p.X);
            var sin = Math.Sin(atan2 + n4);
            var x = W * (Math.Cos(atan2 + n4) * sqrt);
            var y = W * (sin * sqrt);
            return new Point(x, y);
        }

        public override void Init()
        {
            _seed = VariationHelper.Map(P2, -10, 10, -5, 5);
            _xx = VariationHelper.Map(P3, -10, 10, -5, 14);
            _yy = VariationHelper.Map(P4, -10, 10, -5, 79);
        }
    }
}