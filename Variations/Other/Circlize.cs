using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Circlize : VariationModel
    {
        public Circlize()
        {
            SetParameters(new[] {0.4}, new[] {"hole"});
        }

        public override int Id { get; } = 79;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = W / 0.7853981633974483;
            var fabs = Math.Abs(p.X);
            var fabs2 = Math.Abs(p.Y);
            double n3;
            double n4;
            if (fabs >= fabs2)
            {
                if (p.X >= fabs2)
                    n3 = fabs + p.Y;
                else
                    n3 = 5.0 * fabs - p.Y;
                n4 = fabs;
            }
            else
            {
                if (p.Y >= fabs)
                    n3 = 3.0 * fabs2 - p.X;
                else
                    n3 = 7.0 * fabs2 + p.X;
                n4 = fabs2;
            }

            var n5 = n2 * n4 + P1;
            var n6 = 0.7853981633974483 * n3 / n4 - 0.7853981633974483;
            var sin = Math.Sin(n6);
            var x = n5 * Math.Cos(n6);
            var y = n5 * sin;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}