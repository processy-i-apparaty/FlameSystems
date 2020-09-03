using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class CannabisCurve : VariationModel
    {
        private int _filled;

        public CannabisCurve()
        {
            SetParameters(new[] {1.0}, new[] {"filled"});
        }

        public override int Id { get; } = 73;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var precalcAtan = VariationHelper.PreAtan(p);
            var n2 = (1.0 + 0.9 * Math.Cos(8.0 * precalcAtan)) * (1.0 + 0.1 * Math.Cos(24.0 * precalcAtan)) *
                     (0.9 + 0.1 * Math.Cos(200.0 * precalcAtan)) * (1.0 + Math.Sin(precalcAtan));
            var n3 = precalcAtan + VariationHelper.HalfPi;
            if (_filled == 1) n2 *= VariationHelper.Psi;

            var n4 = Math.Sin(n3) * n2;
            var n5 = Math.Cos(n3) * n2;
            var x = W * n4;
            var y = W * n5;
            return new Point(x, y);
        }

        public override void Init()
        {
            _filled = (int) Math.Floor(P1);
        }
    }
}