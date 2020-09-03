using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Clifford : VariationModel
    {
        public Clifford()
        {
            SetParameters(new[] {-1.4, 1.6, 1.0, 0.7}, new[] {"a", "b", "c", "d"});
        }

        public override int Id { get; } = 83;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = Math.Sin(P1 * p.Y) + P3 * Math.Cos(P1 * p.X);
            var n3 = Math.Sin(P2 * p.X) + P4 * Math.Cos(P2 * p.Y);
            var x = n2 * W;
            var y = n3 * W;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}