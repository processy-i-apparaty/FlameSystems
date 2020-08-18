using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Handkerchief : VariationModel
    {
        public override int Id { get; } = 6;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            return new Point(
            //                r * Math.Sin(theta + r)*W,
            //                r * Math.Cos(theta - r)*W);

            var preAtan = VariationHelper.PreAtan(p);
            var sqrt = VariationHelper.R(p);
            return new Point(W * (Math.Sin(preAtan + sqrt) * sqrt),
                W * (Math.Cos(preAtan - sqrt) * sqrt));
        }
    }
}