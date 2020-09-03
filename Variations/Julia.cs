using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Julia : VariationModel
    {
        public override int Id { get; } = 13;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = VariationHelper.PreAtan(p) * 0.5 + Math.PI * VariationHelper.RandomNext(2);
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * Math.Sqrt(VariationHelper.R(p));
            return new Point(
                n3 * cos,
                n3 * sin
            );
        }

        public override void Init()
        {
        }
    }
}