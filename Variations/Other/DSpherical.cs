using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class DSpherical : VariationModel
    {
        private double _dSpherWeight;

        public DSpherical()
        {
            SetParameters(new[] {0.0}, new[] {"dSpherWeight"});
        }

        public override int Id { get; } = 112;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            if (VariationHelper.Psi < _dSpherWeight)
            {
                var n2 = W / (VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y));
                point.X += p.X * n2;
                point.Y += p.Y * n2;
            }
            else
            {
                point.X += p.X * W;
                point.Y += p.Y * W;
            }

            return point;
        }

        public override void Init()
        {
            _dSpherWeight = VariationHelper.Map(P1, -10, 10, 0.0, 1.0);
        }
    }
}