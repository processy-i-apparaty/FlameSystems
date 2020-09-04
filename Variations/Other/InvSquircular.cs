using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class InvSquircular : VariationModel
    {
        private double _sqrt2;


        public override int Id { get; } = 139;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            if (!(Math.Abs(W) >= VariationHelper.SmallDouble)) return p;
            var x = p.X;
            var y = p.Y;
            var n2 = x * x + y * y;
            var n3 = Math.Sqrt(n2 - Math.Sqrt(n2 * (W * W * n2 - 4.0 * x * x * y * y) / W)) / _sqrt2;
            var dx = n3 / x;
            var dy = n3 / y;
            return new Point(dx, dy);
        }

        public override void Init()
        {
            _sqrt2 = Math.Sqrt(2.0);
        }
    }
}