using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cardioid : VariationModel
    {
        public Cardioid()
        {
            SetParameters(new[] {1.0}, new[] {"a"});
        }

        public override int Id { get; } = 74;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var atan2 = Math.Atan2(p.Y, p.X);
            var n2 = W * Math.Sqrt(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y) + Math.Sin(atan2 * P1) + 1.0);
            var cos = Math.Cos(atan2);
            var sin = Math.Sin(atan2);

            var x = n2 * cos;
            var y = n2 * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}