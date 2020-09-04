using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class HyperShift : VariationModel
    {
        public HyperShift()
        {
            SetParameters(new[] {2.0, 1.0}, new[] {"shift", "stretch"});
        }

        public override int Id { get; } = 136;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 1.0 - VariationHelper.Sqr(P1);
            var n3 = 1.0 / (p.X * p.X + p.Y * p.Y);
            var n4 = n3 * p.X + P1;
            var n5 = n3 * p.Y;
            var n6 = W * n2 / (n4 * n4 + n5 * n5);
            var x = n6 * n4 + P1;
            var y = n6 * n5 * P2;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}