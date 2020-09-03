using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Linear : VariationModel
    {
        public override int Id { get; } = 0;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return W == 1.0 ? p : new Point(W * p.X, W * p.Y);
        }

        public override void Init()
        {
        }
    }
}