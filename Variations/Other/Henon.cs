using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Henon : VariationModel
    {
        public Henon()
        {
            SetParameters(new[] {.5, 1.0, 1.0}, new[] {"a", "b", "c"});
        }

        public override int Id { get; } = 132;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = (P3 - P1 * VariationHelper.Sqr(p.X) + p.Y) * W;
            var y = P2 * p.X * W;
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}