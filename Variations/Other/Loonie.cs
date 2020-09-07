using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Loonie : VariationModel
    {
        public override int Id { get; } = 151;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var preSumSq = VariationHelper.PreSumSq(p);
            var n2 = W * W;
            var point = new Point();
            if (preSumSq < n2 && Math.Abs(preSumSq) >= VariationHelper.SmallDouble)
            {
                var n3 = W * Math.Sqrt(n2 / preSumSq - 1.0);
                point.X += n3 * p.X;
                point.Y += n3 * p.Y;
            }
            else
            {
                point.X += W * p.X;
                point.Y += W * p.Y;
            }

            return point;
        }

        public override void Init()
        {
        }
    }
}