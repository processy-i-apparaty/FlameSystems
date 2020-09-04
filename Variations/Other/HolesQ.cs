using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class HolesQ : VariationModel
    {
        public override int Id { get; } = 133;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var n2 = W * p.X;
            var n3 = W * p.Y;
            var fabs = Math.Abs(n2);
            var fabs2 = Math.Abs(n3);
            if (fabs + fabs2 > 1.0)
            {
                point.X += n2;
                point.Y += n3;
            }
            else if (fabs > fabs2)
            {
                var n4 = (n2 - fabs2 + 1.0) * 0.5;
                if (n2 < 0.0) n4 = (n2 + fabs2 - 1.0) * 0.5;
                point.X += n4;
                point.Y += n3;
            }
            else
            {
                var n5 = (n3 - fabs + 1.0) * 0.5;
                if (n3 < 0.0) n5 = (n3 + fabs - 1.0) * 0.5;
                point.X += n2;
                point.Y += n5;
            }


            return point;
        }

        public override void Init()
        {
        }
    }
}