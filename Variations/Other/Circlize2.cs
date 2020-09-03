using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Circlize2 : VariationModel
    {
        public Circlize2()
        {
            SetParameters(new[] {0.0}, new[] {"hole"});
        }

        public override int Id { get; } = 80;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var fabs = Math.Abs(p.X);
            var fabs2 = Math.Abs(p.Y);
            double n2;
            double n3;
            if (fabs >= fabs2)
            {
                if (p.X >= fabs2)
                    n2 = fabs + p.Y;
                else
                    n2 = 5.0 * fabs - p.Y;
                n3 = fabs;
            }
            else
            {
                if (p.Y >= fabs)
                    n2 = 3.0 * fabs2 - p.X;
                else
                    n2 = 7.0 * fabs2 + p.X;
                n3 = fabs2;
            }

            var n4 = W * (n3 + P1);
            var n5 = 0.7853981633974483 * n2 / n3 - 0.7853981633974483;
            var sin = Math.Sin(n5);
            var x = n4 * Math.Cos(n5);
            var y = n4 * sin;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}