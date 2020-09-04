using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Glynnia : VariationModel
    {
        private double _var2;
        public override int Id { get; } = 128;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var sqrt = Math.Sqrt(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y));
            if (sqrt >= 1.0)
            {
                if (VariationHelper.Psi > 0.5)
                {
                    var sqrt2 = Math.Sqrt(sqrt + p.X);
                    if (Math.Abs(sqrt2) <= VariationHelper.SmallDouble) return p;
                    point.X += _var2 * sqrt2;
                    point.Y -= _var2 / sqrt2 * p.Y;
                }
                else
                {
                    var n2 = sqrt + p.X;
                    var sqrt3 = Math.Sqrt(sqrt * (VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(n2)));
                    if (Math.Abs(sqrt3) <= VariationHelper.SmallDouble) return p;
                    var n3 = W / sqrt3;
                    point.X += n3 * n2;
                    point.Y += n3 * p.Y;
                }
            }
            else if (VariationHelper.Psi > 0.5)
            {
                var sqrt4 = Math.Sqrt(sqrt + p.X);
                if (Math.Abs(sqrt4) <= VariationHelper.SmallDouble) return p;
                point.X -= _var2 * sqrt4;
                point.Y -= _var2 / sqrt4 * p.Y;
            }
            else
            {
                var n4 = sqrt + p.X;
                var sqrt5 = Math.Sqrt(sqrt * (VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(n4)));
                if (Math.Abs(sqrt5) <= VariationHelper.SmallDouble) return p;
                var n5 = W / sqrt5;
                point.X -= n5 * n4;
                point.Y += n5 * p.Y;
            }

            return point;
        }

        public override void Init()
        {
            _var2 = W * Math.Sqrt(2.0) / 2.0;
        }
    }
}