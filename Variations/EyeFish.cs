using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class EyeFish : VariationModel
    {
        public override int Id { get; } = 27;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;
        public override Point Fun(Point p)
        {
            var q = 2.0 / (VariationHelper.R(p) + 1.0)*W;
            return new Point(q * p.X, q * p.Y);
        }
    }
}