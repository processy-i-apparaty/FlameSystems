using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Fan2 : VariationModel
    {
        public Fan2()
        {
            SetParameters(new[] { 2.0, 2.0 }, new[] { "x", "y" });
        }

        public override int Id { get; } = 25;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var p1 = Math.PI * (P1 * P1);
            //            var theta = VariationHelper.Theta(p);
            //            var t = theta + P2 - p1 * VariationHelper.Trunc(2.0 * theta * P2 / p1);
            //            var r = VariationHelper.R(p);
            //            var q = p1 / 2.0;
            //            return t > q
            //                ? new Point(r * Math.Sin(theta - q), r * Math.Cos(theta - q))
            //                : new Point(r * Math.Sin(theta + q), r * Math.Cos(theta + q));

            var sqrt = VariationHelper.R(p);
            double preAtan;
            if (p.X < -1.0E-300 || p.X > 1.0E-300 || p.Y < -1.0E-300 || p.Y > 1.0E-300)
                preAtan = VariationHelper.PreAtan(p);
            else
                preAtan = 0.0;

            var y = P2;
            var n2 = Math.PI * (P1*P1) + 1.0E-300;
            var n3 = n2 * 0.5;
            double n4;
            if (preAtan + y - (int) ((preAtan + y) / n2) * n2 > n3)
                n4 = preAtan - n3;
            else
                n4 = preAtan + n3;
            return new Point(W * sqrt * Math.Sin(n4), W * sqrt * Math.Cos(n4));
        }
    }
}