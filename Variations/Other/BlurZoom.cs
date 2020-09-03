using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class BlurZoom : VariationModel
    {
        public BlurZoom()
        {
            SetParameters(new[] {0.0, 0.0, 0.0}, new[] {"length", "x", "y"});
        }

        public override int Id { get; } = 68;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 1.0 + P1 * VariationHelper.Psi;
            var x = W * ((p.X - P2) * n2 + P2);
            var y = W * ((p.Y - P3) * n2 - P3);
            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}