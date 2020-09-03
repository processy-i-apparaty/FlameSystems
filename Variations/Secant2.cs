using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Secant2 : VariationModel
    {
        public override int Id { get; } = 46;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var sin = Math.Sin(p.X);
            //            var cos = Math.Cos(p.X);
            //            var sinh = Math.Sinh(p.Y);
            //            var cosh = Math.Cosh(p.Y);
            //            var n2 = Math.Cos(2.0 * p.X) + Math.Cosh(2.0 * p.Y);
            //            if (n2 == 0.0) return p;
            //            var n3 = 2.0 / n2;
            //            return new Point(W * n3 * cos * cosh, W * n3 * sin * sinh);


            var cos = Math.Cos(W * VariationHelper.PreSqrt(p));
            if (Math.Abs(cos) <= VariationHelper.SmallDouble) return p;
            var point = new Point();
            var n2 = 1.0 / cos;
            point.X = W * p.X;
            if (cos < 0.0)
                point.Y = W * (n2 + 1.0);
            else
                point.Y = W * (n2 - 1.0);
            return point;
        }

        public override void Init()
        {
        }
    }
}