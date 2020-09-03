using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Blade : VariationModel
    {
        public override int Id { get; } = 45;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = VariationHelper.Psi * W * Math.Sqrt(p.X * p.X + p.Y * p.Y);
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            return new Point(W * p.X * (cos + sin), W * p.X * (cos - sin));
        }

        public override void Init()
        {
        }
    }
}