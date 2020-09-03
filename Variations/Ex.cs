using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Ex : VariationModel
    {
        public override int Id { get; } = 12;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            var pp0 = Math.Sin(theta + r);
            //            var pp1 = Math.Cos(theta - r);
            //            var p0Cube = pp0 * pp0 * pp0;
            //            var p1Cube = pp1 * pp1 * pp1;
            //
            //            return new Point(
            //                r * (p0Cube + p1Cube),
            //                r * (p0Cube - p1Cube));

            var sqrt = VariationHelper.R(p);
            var preAtan = VariationHelper.PreAtan(p);
            var sin = Math.Sin(preAtan + sqrt);
            var cos = Math.Cos(preAtan - sqrt);
            var n2 = sin * sin * sin;
            var n3 = cos * cos * cos;
            var n4 = sqrt * W;

            return new Point(
                n4 * (n2 + n3),
                n4 * (n2 - n3)
            );
        }

        public override void Init()
        {
        }
    }
}