using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class AnamorphCyl : VariationModel
    {
        public AnamorphCyl()
        {
            SetParameters(new[] { 1.0, 1.3, 3.0 }, new[] { "a", "b", "k" });
        }

        public override int Id { get; } = 52;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var a = P1;
            var b = P2;
            var k = P3;
            var evalX = a * (p.Y + b) * Math.Cos(k * p.X);
            var evalY = a * (p.Y + b) * Math.Sin(k * p.X);
            return new Point(evalX * W, evalY * W);
        }
    }
}