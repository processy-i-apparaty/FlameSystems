using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Spiral : VariationModel
    {
        public override int Id { get; } = 9;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            var q = 1.0 / r;
            //            return new Point(
            //                q * (Math.Cos(theta) + Math.Sin(r)),
            //                q * (Math.Sin(theta) - Math.Cos(r))
            //            );

            var preSqrt = VariationHelper.PreSqrt(p);
            var preSinA = VariationHelper.PreSinA(p, preSqrt);
            var preCosA = VariationHelper.PreCosA(p, preSqrt);
            var sin = Math.Sin(preSqrt);
            var cos = Math.Cos(preSqrt);
            var n2 = W / preSqrt;
            return new Point(
                (preCosA + sin) * n2,
                (preSinA - cos) * n2
            );
        }

        public override void Init()
        {
        }
    }
}