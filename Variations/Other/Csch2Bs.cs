using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Csch2Bs : VariationModel
    {
        public Csch2Bs()
        {
            SetParameters(new[] {1.0, 1.0, 1.0, 1.0}, new[] {"x1", "x2", "y1", "y2"});
        }

        public override int Id { get; } = 97;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(p.Y * P3);
            var cos = Math.Cos(p.Y * P4);
            var sinh = Math.Sinh(p.X * P1);
            var cosh = Math.Cosh(p.X * P2);
            var n2 = Math.Cosh(2.0 * p.X) - Math.Cos(2.0 * p.Y);
            if (Math.Abs(n2) <= double.Epsilon) n2 = VariationHelper.SmallDouble;


            var n3 = 2.0 / n2;
            var x = W * n3 * sinh * cos;
            var y = -W * n3 * cosh * sin;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}