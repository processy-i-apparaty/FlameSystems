using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class Asteria : VariationModel
    {
        public Asteria()
        {
            SetParameters(new[] { 0.0 }, new[] { "alpha" });
        }

        public override int Id { get; } = 58;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = W * p.X;
            var n3 = W * p.Y;
            var n4 = n2;
            var n5 = n3;
            var n6 = VariationHelper.Sqr(n4) + VariationHelper.Sqr(n5);
            var sqrt = Math.Sqrt(VariationHelper.Sqr(Math.Abs(n5) - 1.0) + VariationHelper.Sqr(Math.Abs(n4) - 1.0));
            var b = n6 < 1.0;
            var b2 = sqrt < 1.0;
            bool b3;
            var sin = Math.Sin(Math.PI * P1);
            var cos = Math.Cos(Math.PI * P1);

            var point = new Point();
            if (b && b2)
                b3 = VariationHelper.Psi > 0.35;
            else
                b3 = !b;

            if (b3)
            {
                point.X += n2;
                point.Y += n3;
            }
            else
            {
                var n7 = n2 * cos - n3 * sin;
                var n8 = n2 * sin + n3 * cos;
                var n9 = n7 / Math.Sqrt(1.0 - n8 * n8) *
                         (1.0 - Math.Sqrt(1.0 - VariationHelper.Sqr(-Math.Abs(n8) + 1.0)));
                var n10 = n9 * cos + n8 * sin;
                var n11 = -n9 * sin + n8 * cos;
                point.X += n10;
                point.Y += n11;
            }

            return point;
        }
    }
}