using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Spherical : VariationModel
    {
        public override int Id { get; } = 2;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var r = VariationHelper.R(p);
            var q = 1.0 / (r * r) * W;
            return new Point(p.X * q, p.Y * q);
        }

        public override void Init()
        {
        }
    }
}