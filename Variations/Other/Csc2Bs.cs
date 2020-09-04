using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Csc2Bs : VariationModel
    {
        public Csc2Bs()
        {
            SetParameters(new[] {1.0, 1.0, 1.0, 1.0}, new[] {"x1", "x2", "y1", "y2"});
        }

        public override int Id { get; } = 96;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(p.X * P1);
            var cos = Math.Cos(p.X * P2);
            var sinh = Math.Sinh(p.Y * P3);
            var cosh = Math.Cosh(p.Y * P4);
            var n2 = Math.Cosh(2.0 * p.Y) - Math.Cos(2.0 * p.X);
            if (Math.Abs(n2) <= double.Epsilon) n2 = VariationHelper.SmallDouble;

            var n3 = 2.0 / n2;
            var x = W * n3 * sin * cosh;
            var y = -W * n3 * cos * sinh;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}