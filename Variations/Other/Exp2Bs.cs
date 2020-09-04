using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Exp2Bs : VariationModel
    {
        public Exp2Bs()
        {
            SetParameters(new[] {1.0, 1.0, 1.0}, new[] {"x1", "y1", "y2"});
        }

        public override int Id { get; } = 124;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var exp = Math.Exp(p.X * P1);
            var sin = Math.Sin(p.Y * P2);
            var x = W * exp * Math.Cos(p.Y * P3);
            var y = W * exp * sin;
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}