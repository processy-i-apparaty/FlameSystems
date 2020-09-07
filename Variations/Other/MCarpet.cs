using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class MCarpet : VariationModel
    {
        public MCarpet()
        {
            SetParameters(new[] {1.0, 1.0, 0.0, 0.0}, new[] {"x", "y", "twist", "tilt"});
        }

        public override int Id { get; } = 156;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var n2 = W / ((VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y)) / 4.0 + 1.0);
            point.X += p.X * n2 * P1;
            point.Y += p.Y * n2 * P2;
            point.X += (1.0 - P3 * VariationHelper.Sqr(p.X) + p.Y) * W;
            point.Y += P4 * p.X * W;
            return point;
        }

        public override void Init()
        {
        }
    }
}