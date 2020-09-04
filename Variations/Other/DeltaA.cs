using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class DeltaA : VariationModel
    {
        public override int Id { get; } = 113;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = W * (Math.Sqrt(VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(p.X + 1.0)) /
                          Math.Sqrt(VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(p.X - 1.0)));

            var a = (Math.Atan2(p.Y, p.X - 1.0) - Math.Atan2(p.Y, p.X + 1.0)) * .5;
            var sin = Math.Sin(a);
            var cos = Math.Cos(a);

            var x = n2 * cos;
            var y = n2 * sin;
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}