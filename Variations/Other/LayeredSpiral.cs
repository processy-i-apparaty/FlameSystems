using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class LayeredSpiral : VariationModel
    {
        public LayeredSpiral()
        {
            SetParameters(new[] {1.0}, new[] {"radius"});
        }

        public override int Id { get; } = 145;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = p.X * P1;
            var n3 = VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y) + 1.0E-8;
            var x = W * n2 * Math.Cos(n3);
            var y = W * n2 * Math.Sin(n3);
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}