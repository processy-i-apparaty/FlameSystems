using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Hyperbolic : VariationModel
    {
        public override int Id { get; } = 10;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            return new Point(
            //                Math.Sin(theta) / r,
            //                r * Math.Cos(theta));

            var preSqrt = VariationHelper.PreSqrt(p);
            return new Point(W * VariationHelper.PreSinA(p, preSqrt) / preSqrt,
                W * VariationHelper.PreCosA(p, preSqrt) * preSqrt);
        }

        public override void Init()
        {
        }
    }
}