using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class Auger : VariationModel
    {
        public Auger()
        {
            SetParameters(new[] { 1.0, 0.5, 0.1, 0.9 }, new[] { "freq", "weight", "sym", "scale" });
        }

        public override int Id { get; } = 60;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();

            var sin = Math.Sin(P1 * p.X);
            var sin2 = Math.Sin(P1 * p.Y);
            var n2 = p.Y + P2 * (P4 * sin * 0.5 + Math.Abs(p.Y) * sin);
            point.X += W * (p.X + P3 * (p.X + P2 * (P4 * sin2 * 0.5 + Math.Abs(p.X) * sin2) - p.X));
            point.Y += W * n2;
            return point;
        }
    }
}