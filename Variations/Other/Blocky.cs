using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class BlockY : VariationModel
    {
        private double _v;

        public BlockY()
        {
            SetParameters(new[] { 1.0, 1.0, 4.0 }, new[] { "x", "y", "mp" });
        }

        public override int Id { get; } = 64;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var x = P1;
            var y = P2;
            var mp = P3;
            var n2 = W / ((Math.Cos(p.X) + Math.Cos(p.Y)) / mp + 1.0);
            var n3 = VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(p.X) + 1.0;
            var n4 = 2.0 * p.X;
            var n5 = 2.0 * p.Y;
            var n6 = 0.5 * (Math.Sqrt(n3 + n4) + Math.Sqrt(n3 - n4));
            var n7 = 0.5 * (Math.Sqrt(n3 + n5) + Math.Sqrt(n3 - n5));
            var n8 = p.X / n6;
            point.X += _v * Math.Atan2(n8, SqrtSafe(1.0 - VariationHelper.Sqr(n8))) * n2 * x;
            var n9 = p.Y / n7;
            point.Y += _v * Math.Atan2(n9, SqrtSafe(1.0 - VariationHelper.Sqr(n9))) * n2 * y;

            return point;
        }

        public override void Init()
        {
            _v = W / VariationHelper.HalfPi;
        }

        private static double SqrtSafe(double n)
        {
            return n < 0.0 ? 0.0 : Math.Sqrt(n);
        }
    }
}