using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class FishEye : VariationModel
    {
        public override int Id { get; } = 16;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var r = VariationHelper.R(p);
            var q = 2.0 / (r + 1.0) * W;
            return new Point(
                q * p.Y,
                q * p.X);
        }

        public override void Init()
        {
        }
    }
}