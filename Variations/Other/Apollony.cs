using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class Apollony : VariationModel
    {
        private readonly double _sqrt3 = Math.Sqrt(3.0);
        public override int Id { get; } = 54;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var q1 = 1.0 + _sqrt3 - p.X;
            var q2 = q1 * q1;
            var q3 = p.Y * p.Y + q2;

            var n2 = 3.0 * q1 / q3 - (1.0 + _sqrt3) / (2.0 + _sqrt3);
            var n3 = 3.0 * p.Y / q3;
            var q4 = n2 * n2 + n3 * n3;
            var n4 = n2 / q4;
            var n5 = -n3 / q4;
            var n6 = (int) (4.0 * VariationHelper.Psi);
            double n7;
            double n8;
            switch (n6 % 3)
            {
                case 0:
                    n7 = n2;
                    n8 = n3;
                    break;
                case 1:
                    n7 = -n4 / 2.0 - n5 * _sqrt3 / 2.0;
                    n8 = n4 * _sqrt3 / 2.0 - n5 / 2.0;
                    break;
                default:
                    n7 = -n4 / 2.0 + n5 * _sqrt3 / 2.0;
                    n8 = -n4 * _sqrt3 / 2.0 - n5 / 2.0;
                    break;
            }

            return new Point(n7 * W, n8 * W);
        }

        public override void Init()
        {
            
        }
    }
}