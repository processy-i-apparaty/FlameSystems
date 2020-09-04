using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class EDisc : VariationModel
    {
        public override int Id { get; } = 119;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;


        public override Point Fun(Point p)
        {
            var n2 = VariationHelper.PreSumSq(p) + 1.0;
            var n3 = 2.0 * p.X;
            var n4 = (Math.Sqrt(n2 + n3) + Math.Sqrt(n2 - n3)) * 0.5;
            var log = Math.Log(n4 + Math.Sqrt(n4 - 1.0));
            var n5 = -Math.Acos(p.X / n4);
            var n6 = W / 11.57034632;
            var sin = Math.Sin(log);
            var cos = Math.Cos(log);
            var sinh = Math.Sinh(n5);
            var cosh = Math.Cosh(n5);
            if (p.Y > 0.0) sin = -sin;
            var x = n6 * cosh * cos;
            var y = n6 * sinh * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}