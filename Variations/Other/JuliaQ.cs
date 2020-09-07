using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class JuliaQ : VariationModel
    {
        private double _halfInvPower;

        private double _invPower;
        private double _invPower2Pi;

        public JuliaQ()
        {
            SetParameters(new[] {2.0, 2.0}, new[] {"power", "divisor"});
        }

        public override int Id { get; } = 143;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = Math.Atan2(p.Y, p.X) * _invPower +
                     VariationHelper.RandomInt(0, int.MaxValue) * _invPower2Pi;
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * Math.Pow(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y), _halfInvPower);
            var x = n3 * cos;
            var y = n3 * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
            _halfInvPower = 0.5 * P2 / P1;
            _invPower = P2 / P1;
            _invPower2Pi = VariationHelper.TwoPi / P1;
        }
    }
}