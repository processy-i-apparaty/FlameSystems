using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class CPow : VariationModel
    {
        private double _n4;
        private double _n5;

        public CPow()
        {
            SetParameters(new[] {1.0, 0.1, 1.5}, new[] {"r", "i", "power"});
        }

        public override int Id { get; } = 94;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var precalcAtanYx = VariationHelper.PreAtanYx(p);
            var n2 = 0.5 * Math.Log(VariationHelper.PreSumSq(p));
            var n3 = VariationHelper.TwoPi / P3;
            // double n4 = r / power;
            // double n5 = i / power;
            var n6 = _n4 * precalcAtanYx + _n5 * n2 + n3 * Math.Floor(P3 * VariationHelper.Psi);
            var n7 = W * Math.Exp(_n4 * n2 - _n5 * precalcAtanYx);
            var sin = Math.Sin(n6);
            var x = n7 * Math.Cos(n6);
            var y = n7 * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
            _n4 = P1 / P3;
            _n5 = P2 / P3;
        }
    }
}