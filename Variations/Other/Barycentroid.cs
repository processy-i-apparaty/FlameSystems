using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class Barycentroid : VariationModel
    {
        public Barycentroid()
        {
            SetParameters(new[] { 1.0, 0.0, 0.0, 1.0 }, new[] { "a", "b", "c", "d" });
        }

        public override int Id { get; } = 62;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var a = P1;
            var b = P2;
            var c = P3;
            var d = P4;

            var point = new Point();
            var n2 = a * a + b * b;
            var n3 = a * c + b * d;
            var n4 = a * p.X + b * p.Y;
            var n5 = c * c + d * d;
            var n6 = c * p.X + d * p.Y;
            var n7 = 1.0 / (n2 * n5 - n3 * n3);
            var n8 = (n5 * n4 - n3 * n6) * n7;
            var n9 = (n2 * n6 - n3 * n4) * n7;
            var n10 = Math.Sqrt(VariationHelper.Sqr(n8) + VariationHelper.Sqr(p.X)) * Sgn(n8);
            var n11 = Math.Sqrt(VariationHelper.Sqr(n9) + VariationHelper.Sqr(p.Y)) * Sgn(n9);
            point.X += W * n10;
            point.Y += W * n11;

            return point;
        }

        private static double Sgn(double n)
        {
            return n < 0.0 ? -1.0 : n > 0.0 ? 1.0 : 0.0;
        }
    }
}