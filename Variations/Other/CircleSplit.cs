using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class CircleSplit : VariationModel
    {
        private double _cs;

        public CircleSplit()
        {
            SetParameters(new[] {1.0, 0.5}, new[] {"radius", "split"});
        }

        public override int Id { get; } = 78;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var cx = p.X;
            var cy = p.Y;
            var sqrt = Math.Sqrt(VariationHelper.Sqr(cx) + VariationHelper.Sqr(cy));
            double n2;
            double n3;
            if (sqrt < _cs)
            {
                n2 = cx;
                n3 = cy;
            }
            else
            {
                var atan2 = Math.Atan2(cy, cx);
                var n4 = sqrt + P2;
                n2 = Math.Cos(atan2) * n4;
                n3 = Math.Sin(atan2) * n4;
            }

            var x = W * n2;
            var y = W * n3;


            return new Point(x, y);
        }

        public override void Init()
        {
            _cs = P1 - P2;
        }
    }
}