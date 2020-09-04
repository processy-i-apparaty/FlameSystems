using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Flower : VariationModel
    {
        private double _petals;
        private double _holes;

        public Flower()
        {
            SetParameters(new[] {0.2, 0.0}, new[] {"holes", "petals"});
        }

        public override int Id { get; } = 126;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var precalcAtanYX = VariationHelper.PreAtanYx(p);
            var precalcSqrt = VariationHelper.PreSqrt(p);
            if (Math.Abs(precalcSqrt) <= VariationHelper.SmallDouble) return p;
            var n2 = W * (VariationHelper.Psi - _holes) * Math.Cos(_petals * precalcAtanYX) / precalcSqrt;
            var x = n2 * p.X;
            var y = n2 * p.Y;


            return new Point(x, y);
        }

        public override void Init()
        {
            _petals = VariationHelper.Map(P2, -10, 10, 0, 30);
            _holes = VariationHelper.Map(P2, -10, 10, -5, 5);
        }
    }
}