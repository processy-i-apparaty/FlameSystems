using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Hypercrop : VariationModel
    {
        public Hypercrop()
        {
            SetParameters(new[] {4.0, 1.0, 0.0}, new[] {"n", "rad", "zero"});
        }

        public override int Id { get; } = 135;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = P1 * 0.5 / Math.PI;
            var x = p.X;
            var y = p.Y;
            var n3 = Math.PI / P1;
            var n4 = 1.0 / Math.Cos(n3);
            var n5 = P2 * Math.Sin(n3) * n4;
            var n6 = Math.Floor(Math.Atan2(p.Y, p.X) * n2) / n2 + Math.PI / P1;
            var n7 = Math.Cos(n6) * n4;
            var n8 = Math.Sin(n6) * n4;
            if (Math.Sqrt(VariationHelper.Sqr(p.X - n7) + VariationHelper.Sqr(p.Y - n8)) < n5)
            {
                if (P3 > 1.5)
                {
                    x = n7;
                    y = n8;
                }
                else if (P3 > 0.5)
                {
                    x = 0.0;
                    y = 0.0;
                }
                else
                {
                    var atan2 = Math.Atan2(p.Y - n8, p.X - n7);
                    x = n7 + Math.Cos(atan2) * n5;
                    y = n8 + Math.Sin(atan2) * n5;
                }
            }

            var dx = x * W;
            var dy = y * W;


            return new Point(dx, dy);
        }

        public override void Init()
        {
        }
    }
}