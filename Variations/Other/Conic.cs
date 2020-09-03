using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Conic : VariationModel
    {
        public Conic()
        {
            SetParameters(new[] {1.0, 0.0}, new[] {"eccentricity", "holes", ""});
        }

        public override int Id { get; } = 86;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = W * (VariationHelper.Psi - P2) * P1 / (1.0 + P1 * (p.X / VariationHelper.PreSqrt(p))) /
                     VariationHelper.PreSqrt(p);
            var x = n2 * p.X;
            var y = n2 * p.Y;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}