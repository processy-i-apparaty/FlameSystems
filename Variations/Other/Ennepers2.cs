using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Ennepers2 : VariationModel
    {
        public Ennepers2()
        {
            SetParameters(new[] {1.0, 0.333, 0.075}, new[] {"a", "b", "c"});
        }

        public override int Id { get; } = 121;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = p.X;
            var y = p.Y;
            var n2 = 1.0 / (VariationHelper.Sqr(x) + VariationHelper.Sqr(y));
            var n3 = VariationHelper.Sqr(P1 * x) - VariationHelper.Sqr(P2 * y);
            var dx = W * x * (VariationHelper.Sqr(P1) - n3 * n2 - P3 * Math.Sqrt(Math.Abs(x)));
            var dy = W * y * (VariationHelper.Sqr(P2) - n3 * n2 - P3 * Math.Sqrt(Math.Abs(y)));


            return new Point(dx, dy);
        }

        public override void Init()
        {
        }
    }
}