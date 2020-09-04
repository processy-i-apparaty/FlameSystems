using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Hadamard : VariationModel
    {
        public override int Id { get; } = 131;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double n2;
            double n3;
            if (VariationHelper.Psi < 0.333)
            {
                n2 = p.X / 2.0;
                n3 = p.Y / 2.0;
            }
            else if (VariationHelper.Psi < 0.666)
            {
                n2 = p.Y / 2.0;
                n3 = -p.X / 2.0 - 0.5;
            }
            else
            {
                n2 = -p.Y / 2.0 - 0.5;
                n3 = p.X / 2.0;
            }

            var x = n2 * W;
            var y = n3 * W;

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}