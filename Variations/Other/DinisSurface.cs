using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class DinisSurface : VariationModel
    {
        public DinisSurface()
        {
            SetParameters(new[] {0.8}, new[] {"a"});
        }

        public override int Id { get; } = 114;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = p.X;
            var y = p.Y;
            var sin = Math.Sin(y);
            var dx = W * P1 * Math.Cos(x) * sin;
            var dy = W * P1 * Math.Sin(x) * sin;

            return new Point(dx, dy);
        }

        public override void Init()
        {
        }
    }
}