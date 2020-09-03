using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class BlurLinear : VariationModel
    {
        private double _cos;
        private double _sin;

        public BlurLinear()
        {
            SetParameters(new[] {1.0, 0.5}, new[] {"length", "angle"});
        }

        public override int Id { get; } = 66;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = P1 * VariationHelper.Psi;
            var x =  W * (p.X + n2 * _cos);
            var y =  W * (p.Y + n2 * _sin);
            return new Point(x, y);
        }

        public override void Init()
        {
            _cos = Math.Cos(P2);
            _sin = Math.Sin(P2);
        }
    }
}