using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Diamond : VariationModel
    {
        public override int Id { get; } = 11;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            return new Point(
            //                W * Math.Sin(theta) * Math.Cos(r),
            //                W * Math.Cos(theta) * Math.Sin(r));
            var preSqrt = VariationHelper.PreSqrt(p);
            var preSinA = VariationHelper.PreSinA(p, preSqrt); //xyzPoint.getPrecalcSinA();
            var preCosA = VariationHelper.PreCosA(p, preSqrt); //xyzPoint.getPrecalcCosA();
            var sin = Math.Sin(preSqrt);
            return new Point(W * preSinA * Math.Cos(preSqrt), W * preCosA * sin);
        }
    }
}