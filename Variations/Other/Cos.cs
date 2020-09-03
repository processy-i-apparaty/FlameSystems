using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cos : VariationModel
    {
        public Cos()
        {
            // SetParameters(new[] {0.0, 0.0, 0.0}, new[] {"", "", ""});
        }

        public override int Id { get; } = 87;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(p.X);
            var cos = Math.Cos(p.X);
            var sinh = Math.Sinh(p.Y);
            var x = W * cos * Math.Cosh(p.Y);
            var y = -W * sin * sinh;
            
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}