using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class BlurPixelize : VariationModel
    {
        private double _invSize;
        private double _v;

        public BlurPixelize()
        {
            SetParameters(new[] {0.1, 1.0}, new[] {"size", "scale"});
        }

        public override int Id { get; } = 67;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            

            var floor = Math.Floor(p.X * _invSize);
            var floor2 = Math.Floor(p.Y * _invSize);
            var x = p.X + _v * (floor + P2 * (VariationHelper.Psi - 0.5) + 0.5);
            var y = _v * (floor2 + P2 * (VariationHelper.Psi - 0.5) + 0.5);
            return new Point(x, y);
        }

        public override void Init()
        {
            _invSize = 1.0 / P1;
            _v = W * P1;
        }
    }
}