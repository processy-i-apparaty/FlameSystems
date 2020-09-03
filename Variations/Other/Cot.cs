using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cot : VariationModel
    {
        public override int Id { get; } = 91;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var sin = Math.Sin(2.0 * p.X);
            var cos = Math.Cos(2.0 * p.X);
            var sinh = Math.Sinh(2.0 * p.Y);
            var n2 = 1.0 / (Math.Cosh(2.0 * p.Y) - cos);
            var x = W * n2 * sin;
            var y = W * n2 * -1.0 * sinh;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}