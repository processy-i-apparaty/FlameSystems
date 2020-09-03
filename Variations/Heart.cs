using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Heart : VariationModel
    {
//        private const double HalfPi = Math.PI *.5;
        public override int Id { get; } = 7;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var tr = VariationHelper.Theta(p) * r;
            //            var sin = _xMath.Sin(tr);
            //            var cos = _xMath.Sin(HalfPi - tr);
            //            return new Point(r * sin, r * -cos);

            //            return new Point(
            //                r * Math.Sin(tr),
            //                r * -Math.Cos(tr));

            var r = VariationHelper.R(p);
            var q = VariationHelper.R(p) * VariationHelper.PreAtan(p);
            var sin = Math.Sin(q);
            var cos = Math.Cos(q);
            var n2 = r * W;
            return new Point(n2 * sin, -(n2 * cos));
        }

        public override void Init()
        {
        }
    }
}