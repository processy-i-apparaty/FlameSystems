using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Rings2 : VariationModel
    {
        private double _dx;

        public Rings2()
        {
            SetParameters(new[] {2.0}, new[] {"val"});
        }

        public override int Id { get; } = 26;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var p1 = P1 * P1;
            //            var r = VariationHelper.R(p);
            //            var t = r - 2.0 * p1 * VariationHelper.Trunc((r + p1) / (2.0 * p1)) + r * (1.0 - p1);
            //            var theta = VariationHelper.Theta(p);
            //            return new Point(t * Math.Sin(theta), t * Math.Cos(theta));
            //            var y = p.Y;

            // var dx = P1 * P1 + 1.0E-8;
            var preSqrt = VariationHelper.PreSqrt(p);
            if (Math.Abs(_dx) <= VariationHelper.SmallDouble ||
                Math.Abs(preSqrt) <= VariationHelper.SmallDouble) return p;
            var n2 = W * (2.0 - _dx * ((int) ((preSqrt / _dx + 1.0) / 2.0) * 2.0 / preSqrt + 1.0));
            return new Point(n2 * p.X, n2 * p.Y);
        }

        public override void Init()
        {
            _dx = P1 * P1 + VariationHelper.SmallDouble;
        }
    }
}