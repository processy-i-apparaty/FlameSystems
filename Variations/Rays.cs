using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Rays : VariationModel
    {
        public override int Id { get; } = 44;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = W * Math.Tan(W * VariationHelper.Omega) * (W / (VariationHelper.PreSumSq(p) + 1.0E-300));
            return new Point(n2 * Math.Cos(p.X), n2 * Math.Sin(p.Y));
        }
    }
}