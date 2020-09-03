using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class BlurCircle : VariationModel
    {
        public override int Id { get; } = 65;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 2.0 * VariationHelper.Psi - 1.0;
            var n3 = 2.0 * VariationHelper.Psi - 1.0;
            var n4 = n2;
            if (n4 < 0.0) n4 *= -1.0;
            var n5 = n3;
            if (n5 < 0.0) n5 *= -1.0;
            double n6;
            double n7;
            if (n4 >= n5)
            {
                if (n2 >= n5)
                    n6 = n4 + n3;
                else
                    n6 = 5.0 * n4 - n3;
                n7 = n4;
            }
            else
            {
                if (n3 >= n4)
                    n6 = 3.0 * n5 - n2;
                else
                    n6 = 7.0 * n5 + n2;
                n7 = n5;
            }

            var n8 = W * n7;
            var a = 0.7853981633974483 * n6 / n7 - 0.7853981633974483;

            var x = n8 * Math.Cos(a);
            var y = n8 * Math.Sin(a);
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}