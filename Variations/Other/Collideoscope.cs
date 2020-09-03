using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Collideoscope : VariationModel
    {
        private double _ka;
        private double _kaKn;
        private double _knPi;
        private double _piKn;

        public Collideoscope()
        {
            SetParameters(new[] {0.2, 1.0}, new[] {"a", "num"});
        }

        public override int Id { get; } = 85;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var atan2 = Math.Atan2(p.Y, p.X);
            var n2 = W * Math.Sqrt(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y));
            double n4;
            if (atan2 >= 0.0)
            {
                var n3 = (int) (atan2 * _knPi);
                if (n3 % 2 == 0)
                    n4 = n3 * _piKn + VariationHelper.Fmod(_kaKn + atan2, _piKn);
                else
                    n4 = n3 * _piKn + VariationHelper.Fmod(-_kaKn + atan2, _piKn);
            }
            else
            {
                var n5 = (int) (-atan2 * _knPi);
                if (n5 % 2 != 0)
                    n4 = -(n5 * _piKn + VariationHelper.Fmod(-_kaKn - atan2, _piKn));
                else
                    n4 = -(n5 * _piKn + VariationHelper.Fmod(_kaKn - atan2, _piKn));
            }

            var sin = Math.Sin(n4);
            var x = n2 * Math.Cos(n4);
            var y = n2 * sin;

            return new Point(x, y);
        }

        public override void Init()
        {
            _knPi = P2 * 0.3183098861837907;
            _piKn = Math.PI / P2;
            _ka = Math.PI * P1;
            _kaKn = _ka / P2;
        }
    }
}