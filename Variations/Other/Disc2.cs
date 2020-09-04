using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Disc2 : VariationModel
    {
        private double _cosAdd;
        private double _sinAdd;
        private double _timeSpi;

        public Disc2()
        {
            SetParameters(new[] {2.0, 0.5}, new[] {"rot", "twist"});
        }

        public override int Id { get; } = 115;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = _timeSpi * (p.X + p.Y);
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * VariationHelper.PreAtan(p) / Math.PI;
            var x = (sin + _cosAdd) * n3;
            var y = (cos + _sinAdd) * n3;
            return new Point(x, y);
        }

        public override void Init()
        {
            var twist = P2;
            _timeSpi = P1 * Math.PI;
            _sinAdd = Math.Sin(twist);
            _cosAdd = Math.Cos(twist);
            --_cosAdd;
            if (twist > VariationHelper.TwoPi)
            {
                var n2 = 1.0 + twist - VariationHelper.TwoPi;
                _cosAdd *= n2;
                _sinAdd *= n2;
            }

            if (twist < -VariationHelper.TwoPi)
            {
                var n3 = 1.0 + twist + VariationHelper.TwoPi;
                _cosAdd *= n3;
                _sinAdd *= n3;
            }
        }
    }
}