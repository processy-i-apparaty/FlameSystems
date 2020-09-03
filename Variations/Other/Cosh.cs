using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cosh : VariationModel
    {
        public override int Id { get; } = 89;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(p.Y);
            var cos = Math.Cos(p.Y);
            var sinh = Math.Sinh(p.X);
            var x = W * Math.Cosh(p.X) * cos;
            var y = W * sinh * sin;
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}