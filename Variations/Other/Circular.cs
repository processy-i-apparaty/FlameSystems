using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Circular : VariationModel
    {
        public Circular()
        {
            SetParameters(new[] {VariationHelper.HalfPi, 0.0}, new[] {"angle", "seed"});
        }

        public override int Id { get; } = 81;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            // double n2 = angle * Math.PI / 180.0;
            var n3 = Math.Sin(p.X * 12.9898 + p.Y * 78.233 + P2) * 43758.5453;
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
        }
    }
}