using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Line : VariationModel
    {
        public Line()
        {
            SetParameters(new[] {1.0, 0.0}, new[] {"delta", "phi"});
        }

        public override int Id { get; } = 146;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = Math.Cos(P1 * Math.PI) * Math.Cos(P2 * Math.PI);
            var n3 = Math.Sin(P1 * Math.PI) * Math.Cos(P2 * Math.PI);
            var sin = Math.Sin(P2 * Math.PI);
            var sqrt = Math.Sqrt(n2 * n2 + n3 * n3 + sin * sin);
            var n4 = n2 / sqrt;
            var n5 = n3 / sqrt;
            var n6 = sin / sqrt;
            var n7 = VariationHelper.Psi * W;
            var x = n4 * n7;
            var y = n5 * n7;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}