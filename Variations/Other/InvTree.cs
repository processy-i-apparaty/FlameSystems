using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class InvTree : VariationModel
    {
        public override int Id { get; } = 140;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double n2;
            double n3;
            var psi = VariationHelper.Psi;
            if (psi < 0.333)
            {
                n2 = p.X / 2.0;
                n3 = p.Y / 2.0;
            }
            else if (psi < 0.666)
            {
                n2 = 1.0 / (p.X + 1.0);
                n3 = p.Y / (p.Y + 1.0);
            }
            else
            {
                n2 = p.X / (p.X + 1.0);
                n3 = 1.0 / (p.Y + 1.0);
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