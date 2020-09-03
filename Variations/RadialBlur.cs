using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class RadialBlur : VariationModel
    {
        private double _n3;

        private double _n4;

        private double _q;
        // private const double HalfPi = Math.PI * .5;

        public RadialBlur()
        {
            SetParameters(new[] {1.5}, new[] {"angle"});
        }

        public override int Id { get; } = 36;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var p1 = P1 * (Math.PI / 2.0);
            //            var t1 = .0;
            //            for (var i = 0; i < 4; i++) t1 += VariationHelper.Psi - 2.0;
            //            var t2 = VariationHelper.Phi(p) + t1 * Math.Sin(p1);
            //            var t3 = t1 * Math.Cos(p1) - 1.0;
            //            var q = 1.0 / t1;
            //            var r = VariationHelper.R(p);
            //            return new Point(
            //                q * (r * Math.Cos(t2) + t3 * p.X),
            //                q * (r * Math.Sin(t2) + t3 * p.Y)
            //            );

            var n2 = VariationHelper.Psi + VariationHelper.Psi + VariationHelper.Psi +
                VariationHelper.Psi - 2.0;
//            var n2 = VariationHelper.Psi * 3.0 + VariationHelper.Psi - 2.0;

            // var q = P1 * VariationHelper.HalfPi;
            // _n3 = W * Math.Sin(q);
            // _n4 = W * Math.Cos(q);

            var sqrt = VariationHelper.R(p);
            var n5 = Math.Atan2(p.Y, p.X) + _n3 * n2;
            var sin = Math.Sin(n5);
            var cos = Math.Cos(n5);
            var n6 = _n4 * n2 - 1.0;
            return new Point(sqrt * cos + n6 * p.X, sqrt * sin + n6 * p.Y);
        }

        public override void Init()
        {
            var q = P1 * VariationHelper.HalfPi;
            _n3 = W * Math.Sin(q);
            _n4 = W * Math.Cos(q);
        }
    }
}