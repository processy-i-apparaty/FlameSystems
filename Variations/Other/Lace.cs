using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Lace : VariationModel
    {
        public override int Id { get; } = 144;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            const double n2 = 2.0;
            var sqrt = VariationHelper.R(p);
            var random = VariationHelper.Psi;
            double n3;
            double n4;
            if (random > 0.75)
            {
                var atan2 = Math.Atan2(p.Y, p.X - 1.0);
                n3 = -sqrt * Math.Cos(atan2) / n2 + 1.0;
                n4 = -sqrt * Math.Sin(atan2) / n2;
            }
            else if (random > 0.5)
            {
                var atan3 = Math.Atan2(p.Y - Math.Sqrt(3.0) / 2.0, p.X + 0.5);
                n3 = -sqrt * Math.Cos(atan3) / n2 - 0.5;
                n4 = -sqrt * Math.Sin(atan3) / n2 + Math.Sqrt(3.0) / 2.0;
            }
            else if (random > 0.25)
            {
                var atan4 = Math.Atan2(p.Y + Math.Sqrt(3.0) / 2.0, p.X + 0.5);
                n3 = -sqrt * Math.Cos(atan4) / n2 - 0.5;
                n4 = -sqrt * Math.Sin(atan4) / n2 - Math.Sqrt(3.0) / 2.0;
            }
            else
            {
                var atan5 = Math.Atan2(p.Y, p.X);
                n3 = -sqrt * Math.Cos(atan5) / n2;
                n4 = -sqrt * Math.Sin(atan5) / n2;
            }

            var x = n4 * W;
            var y = n3 * W;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}