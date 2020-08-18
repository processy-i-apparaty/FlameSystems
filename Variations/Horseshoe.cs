using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Horseshoe : VariationModel
    {
        public override int Id { get; } = 4;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var x = p.X;
            //            var y = p.Y;
            //            var q = 1.0 / VariationHelper.R(p);
            //            return new Point(
            //                q * (x - y) * (x + y),
            //                q * (2.0 * x * y));

            var preSqrt = VariationHelper.PreSqrt(p);
            var preSinA = VariationHelper.PreSinA(p, preSqrt);
            var preCosA = VariationHelper.PreCosA(p, preSqrt);
            return new Point(W * (preSinA * p.X - preCosA * p.Y),
                W * (preCosA * p.X + preSinA * p.Y));
        }
    }
}