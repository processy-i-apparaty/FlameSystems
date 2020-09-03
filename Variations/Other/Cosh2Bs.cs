using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cosh2Bs : VariationModel
    {
        public Cosh2Bs()
        {
            SetParameters(new[] {1.0, 1.0, 1.0, 1.0}, new[] {"x1", "x2", "y1", "y2"});
        }

        public override int Id { get; } = 90;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(p.Y * P3);
            var cos = Math.Cos(p.Y * P4);
            var sinh = Math.Sinh(p.X * P1);
            var x = W * Math.Cosh(p.X * P2) * cos;
            var y = W * sinh * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}