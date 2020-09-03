using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class AnamorphCyl : VariationModel
    {
        public AnamorphCyl()
        {
            SetParameters(new[] {1.0, 1.3, 3.0}, new[] {"a", "b", "k"});
        }

        public override int Id { get; } = 52;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = P1 * (p.Y + P2) * Math.Cos(P3 * p.X);
            var y = P1 * (p.Y + P2) * Math.Sin(P3 * p.X);
            return new Point(x * W, y * W);
        }

        public override void Init()
        {
        }
    }
}