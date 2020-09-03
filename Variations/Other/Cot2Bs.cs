using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cot2Bs : VariationModel
    {
        public Cot2Bs()
        {
            SetParameters(new[] {2.0, 2.0, 2.0, 2.0}, new[] {"x1", "x2", "y1", "y2"});
        }

        public override int Id { get; } = 92;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(P1 * p.X);
            var cos = Math.Cos(P2 * p.X);
            var sinh = Math.Sinh(P3 * p.Y);
            var n2 = 1.0 / (Math.Cosh(P4 * p.Y) - cos);
            var x = W * n2 * sin;
            var y = W * n2 * -1.0 * sinh;
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}