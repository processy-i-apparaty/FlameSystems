using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Fan : VariationModel
    {
        public override int Id { get; } = 22;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = true;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            var t = Math.PI * (C * C);
            //            var td2 = t / 2.0;
            //            double dx;
            //            double dy;
            //            if ((theta + F) % t > td2)
            //            {
            //                dx = r * Math.Cos(theta - td2);
            //                dy = r * Math.Sin(theta - td2);
            //            }
            //            else
            //            {
            //                dx = r * Math.Cos(theta + td2);
            //                dy = r * Math.Sin(theta + td2);
            //            }
            //            return new Point(W*dx, W*dy);

            var getXyCoeff20 = E;
            var getXyCoeff21 = F;

            var n2 = Math.PI * getXyCoeff20 * getXyCoeff20 + 1.0E-300;
            var n3 = n2 * 0.5;

            var preAtan = VariationHelper.PreAtan(p);
            double n4;
            if (preAtan + getXyCoeff21 - (int) ((preAtan + getXyCoeff21) / n2) * n2 > n3)
                n4 = preAtan - n3;
            else
                n4 = preAtan + n3;
            var sin = Math.Sin(n4);
            var cos = Math.Cos(n4);
            var n5 = W * VariationHelper.R(p);
            return new Point(n5 * cos, n5 * sin);
        }
    }
}