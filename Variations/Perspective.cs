using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Perspective : VariationModel
    {
        public Perspective()
        {
            SetParameters(new[] { 0.62, 2.2 }, new[] { "angle", "dist" });
        }

        public override int Id { get; } = 30;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var dist = P2;
            var q = P1 * Math.PI * .5;
            var sin = Math.Sin(q);
            var cos = dist * Math.Cos(q);

            var n2 = dist - p.Y * sin;
            if (n2 == 0.0) return p;
            var n3 = 1.0 / n2;
            return new Point(W * dist * p.X * n3, W * cos * p.Y * n3);
        }
    }
}