using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Coth2Bs : VariationModel
    {
        public Coth2Bs()
        {
            SetParameters(new[] {2.0, 2.0, 2.0, 2.0}, new[] {"x1", "x2", "y1", "y2"});
        }

        public override int Id { get; } = 93;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(P3 * p.Y);
            var cos = Math.Cos(P4 * p.Y);
            var sinh = Math.Sinh(P1 * p.X);
            var n2 = Math.Cosh(P2 * p.X) - cos;
            // if (n2 == 0.0)
            // {
            //     return;
            // }

            var n3 = 1.0 / n2;
            var x = W * n3 * sinh;
            var y = W * n3 * sin;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}