using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Power : VariationModel
    {
        public override int Id { get; } = 19;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var theta = VariationHelper.Theta(p);
            //            var sin = Math.Sin(theta);
            //            var q = W * Math.Pow(VariationHelper.R(p), sin);
            //            return new Point(q * Math.Cos(theta), q * sin);

            var preSqrt = VariationHelper.PreSqrt(p);
            var preSinA = VariationHelper.PreSinA(p, preSqrt);
            var preCosA = VariationHelper.PreCosA(p, preSqrt);

            var n2 = W * Math.Pow(preSqrt, preSinA);
            return new Point(n2 * preCosA, n2 * preSinA);
        }
    }
}