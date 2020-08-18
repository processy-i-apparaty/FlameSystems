using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Noise : VariationModel
    {
        public override int Id { get; } = 31;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var psi1 = VariationHelper.Psi;
            var psi2 = VariationHelper.Psi;
            return new Point(
                psi1 * (p.X * Math.Cos(2.0 * Math.PI * psi2)) * W,
                psi1 * (p.Y * Math.Sin(2.0 * Math.PI * psi2) * W)
            );
        }
    }
}