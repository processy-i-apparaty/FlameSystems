using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class JDisc : VariationModel
    {
        private double _v;

        public override int Id { get; } = 137;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = Math.PI / (Math.Sqrt(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y)) + 1.0);
            var n3 = Math.Atan2(p.Y, p.X) * _v;

            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var x = n3 * cos;
            var y = n3 * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
            _v = W * 0.3183098861837907;
        }
    }
}