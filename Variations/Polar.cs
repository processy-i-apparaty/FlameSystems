using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Polar : VariationModel
    {
        public override int Id { get; } = 5;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return new Point(Math.Atan2(p.X, p.Y) * 0.31830989 * W, (VariationHelper.R(p) - 1.0) * W);
//            return new Point(
//                VariationHelper.Theta(p) / Math.PI,
//                VariationHelper.R(p) - 1.0);
        }
    }
}