using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Murl : VariationModel
    {
        private double _a;
        private double _c;
        private double _cosA;
        private double _im;
        private double _p2;
        private double _r;
        private double _re;
        private double _rl;
        private double _sinA;
        private double _vp;

        public Murl()
        {
            SetParameters(new[] {0.1, 1.0}, new[] {"c", "power"});
        }


        public override int Id { get; } = 157;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            _a = Math.Atan2(p.Y, p.X) * P2;
            _sinA = Math.Sin(_a);
            _cosA = Math.Cos(_a);
            _r = _c * Math.Pow(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y), _p2);
            _re = _r * _cosA + 1.0;
            _im = _r * _sinA;
            _rl = _vp / (VariationHelper.Sqr(_re) + VariationHelper.Sqr(_im));
            var x = _rl * (p.X * _re + p.Y * _im);
            var y = _rl * (p.Y * _re - p.X * _im);


            return new Point(x, y);
        }

        public override void Init()
        {
            _c = P1;
            if (Math.Abs(P2 - 1) >= VariationHelper.SmallDouble) _c /= P2 - 1.0;
            _p2 = P2 / 2.0;
            _vp = W * (_c + 1.0);
        }
    }
}