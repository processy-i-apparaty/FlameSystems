using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Bubble : VariationModel
    {
        public override int Id { get; } = 28;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var r = VariationHelper.R(p);
            var q = W * (4.0 / (r * r + 4.0));
            return new Point(q * p.X, q * p.Y);
        }

        public override void Init()
        {
        }
    }
}