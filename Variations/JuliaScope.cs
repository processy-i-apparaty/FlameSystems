using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class JuliaScope : VariationModel
    {
        private double _power;
        private int _absPower;
        private double _cPower;
        private const double TwoPi = Math.PI * 2.0;

        public JuliaScope()
        {
            SetParameters(new[] { 2.0, 2.0 }, new[] { "power", "dist" });
        }

        public override int Id { get; } = 33;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var pp3 = VariationHelper.Trunc( Math.Abs(P1) * VariationHelper.Psi);
            //            var t = (VariationHelper.Lambda * VariationHelper.Phi(p) + 2.0 * Math.PI * pp3) / P1;
            //            var r2 = Math.Pow(r, P2 / P1);
            //            var dx = r2 * Math.Cos(t);
            //            var dy = r2 * Math.Sin(t);
            //            return new Point(dx, dy);

            var random = VariationHelper.RandomNext(_absPower);
            double n2;
            if ((random & 0x1) == 0x0)
                n2 = (TwoPi * random + Math.Atan2(p.Y, p.X)) / _power;
            else
                n2 = (TwoPi * random - Math.Atan2(p.Y, p.X)) / _power;
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * Math.Pow(p.X * p.X + p.Y * p.Y, _cPower);
            return new Point(n3 * cos, n3 * sin);
        }

        public override void Init()
        {
            _power = Math.Floor(P1);
            _absPower = Math.Abs((int)_power);
            _cPower = P2 / _power * 0.5;
        }
    }
}