using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Butterfly : VariationModel
    {
        private double _n2;
        public override int Id { get; } = 72;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n3 = p.Y * 2.0;
            var n4 = _n2 * Math.Sqrt(Math.Abs(p.Y * p.X) / (VariationHelper.SmallDouble + p.X * p.X + n3 * n3));
            var x = n4 * p.X;
            var y = n4 * n3;
            return new Point(x, y);
        }

        public override void Init()
        {
            _n2 = W * 1.3029400317411197;
        }
    }
}