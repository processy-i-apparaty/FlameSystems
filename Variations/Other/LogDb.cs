using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class LogDb : VariationModel
    {
        private double _denominator;
        private double _fixPe;

        public LogDb()
        {
            SetParameters(new[] {1.0, 1.0}, new[] {"base", "fix period"});
        }

        public override int Id { get; } = 149;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 0.0;
            for (var i = 0; i < 7; ++i)
            {
                var n3 = VariationHelper.RandomNext(10) - 5;
                if (Math.Abs(n3) >= 3) n3 = 0;
                n2 += n3;
            }

            var n4 = n2 * _fixPe;
            var x = _denominator * Math.Log(VariationHelper.PreSumSq(p));
            var y = W * (VariationHelper.PreAtanYx(p) + n4);

            return new Point(x, y);
        }

        public override void Init()
        {
            _denominator = 0.5;
            if (P1 > 1.0E-20) _denominator /= Math.Log(Math.E * P1);
            _denominator *= W;
            _fixPe = Math.PI;
            if (P2 > 1.0E-20) _fixPe *= P2;
        }
    }
}