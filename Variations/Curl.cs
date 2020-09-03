using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Curl : VariationModel
    {
        public Curl()
        {
            SetParameters(new[] {2.0, 2.0}, new[] {"c1", "c2"});
        }

        public override int Id { get; } = 39;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var t1 = 1.0 + P1 * p.X + P2 * (p.X * p.X - p.Y * p.Y);
            var t2 = P1 * p.Y + 2.0 * P2 * p.X * p.Y;
            var q = 1.0 / (t1 * t1 + t2 * t2) * W;
            var dx = q * (p.X * t1 + p.Y * t2);
            var dy = q * (p.Y * t1 - p.X * t2);
            return new Point(dx, dy);
        }

        public override void Init()
        {
        }
    }
}