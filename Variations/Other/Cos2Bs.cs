using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cos2Bs : VariationModel
    {
        public Cos2Bs()
        {
            SetParameters(new[] {1.0, 1.0, 1.0, 1.0}, new[] {"x1", "x2", "y1", "y2"});
        }

        public override int Id { get; } = 88;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(p.X * P1);
            var cos = Math.Cos(p.X * P2);
            var sinh = Math.Sinh(p.Y * P3);
            var x = W * cos * Math.Cosh(p.Y * P4);
            var y = -W * sin * sinh;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}