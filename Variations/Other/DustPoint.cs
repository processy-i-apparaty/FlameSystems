using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class DustPoint : VariationModel
    {
        public override int Id { get; } = 116;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = VariationHelper.Psi < 0.5 ? 1.0 : -1.0;
            var sqrt = VariationHelper.R(p);
            var random = VariationHelper.Psi;
            double n3;
            double n4;
            if (random < 0.5)
            {
                n3 = p.X / sqrt - 1.0;
                n4 = n2 * p.Y / sqrt;
            }
            else if (random < 0.75)
            {
                n3 = p.X / 3.0;
                n4 = p.Y / 3.0;
            }
            else
            {
                n3 = p.X / 3.0 + 0.6666666666666666;
                n4 = p.Y / 3.0;
            }

            var x = n3 * W;
            var y = n4 * W;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}