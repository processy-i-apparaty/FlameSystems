using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Bent : VariationModel
    {
        public override int Id { get; } = 14;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = p.X;
            var y = p.Y;
            if (x < 0.0) x += x;
            if (y < 0.0) y *= 0.5;
            return new Point(W * x, W * y);
        }

        public override void Init()
        {
        }
    }
}