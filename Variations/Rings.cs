using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Rings : VariationModel
    {
        private double _n2;
        public override int Id { get; } = 21;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = true;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            var c2 = C * C;
            //            var q = (r + c2) % (2.0 * c2) - c2 + r * (1.0 - c2);
            //            var dx = q * Math.Cos(theta);
            //            var dy = q * Math.Sin(theta);
            //            return new Point(dx, dy);

            // var coeff20 = E;
            // var n2 = coeff20 * coeff20 + 1.0E-300;
            var preSqrt = VariationHelper.PreSqrt(p);
            var n3 = preSqrt + _n2 - (int) ((preSqrt + _n2) / (2.0 * _n2)) * 2 * _n2 - _n2 +
                     preSqrt * (1.0 - _n2);
            return new Point(
                n3 * VariationHelper.PreCosA(p, preSqrt),
                n3 * VariationHelper.PreSinA(p, preSqrt)
            );
        }

        public override void Init()
        {
            _n2 = E * E + VariationHelper.SmallDouble;
        }
    }
}