using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Elliptic : VariationModel
    {
        private double _v;
        
        public override int Id { get; } = 120;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var n2 = p.Y * p.Y + p.X * p.X + 1.0;
            var n3 = 2.0 * p.X;
            var n4 = 0.5 * (Math.Sqrt(n2 + n3) + Math.Sqrt(n2 - n3));
            var n5 = p.X / n4;
            point.X += _v * Math.Atan2(n5, VariationHelper.SqrtSafe(1.0 - n5 * n5));
            if (VariationHelper.Psi < 0.5)
                point.Y += _v * Math.Log(n4 + VariationHelper.SqrtSafe(n4 - 1.0));
            else
                point.Y -= _v * Math.Log(n4 + VariationHelper.SqrtSafe(n4 - 1.0));

            return point;
        }

        public override void Init()
        {
            _v = W / 1.5707963267948966;
        }
    }
}