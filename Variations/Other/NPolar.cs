using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class NPolar : VariationModel
    {
        private int _absN;
        private double _cn;
        private int _isoDd;
        private int _n;
        private int _nnz;

        private int _parity;
        private double _vVar;
        private double _vVar2;

        public NPolar()
        {
            SetParameters(new[] {0.0, 1.0}, new[] {"parity", "n"});
        }

        public override int Id { get; } = 158;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = _isoDd != 0 ? p.X : _vVar * Math.Atan2(p.X, p.Y);
            var n3 = _isoDd != 0 ? p.Y : _vVar2 * Math.Log(p.X * p.X + p.Y * p.Y);
            var n4 = (Math.Atan2(n3, n2) +
                      VariationHelper.TwoPi * (VariationHelper.RandomInt(int.MinValue, int.MaxValue) % _absN)) / _nnz;
            var n5 = W * Math.Pow(VariationHelper.Sqr(n2) + VariationHelper.Sqr(n3), _cn) *
                     (_isoDd == 0 ? 1.0 : _parity);
            var sin = Math.Sin(n4);
            var n6 = Math.Cos(n4) * n5;
            var n7 = sin * n5;
            var n8 = _isoDd != 0 ? n6 : _vVar2 * Math.Log(n6 * n6 + n7 * n7);
            var n9 = _isoDd != 0 ? n7 : _vVar * Math.Atan2(n6, n7);

            return new Point(n8, n9);
        }

        public override void Init()
        {
            _n = (int) P2;
            _parity = (int) P1;
            _nnz = _n == 0 ? 1 : _n;
            _vVar = W / Math.PI;
            _vVar2 = _vVar * 0.5;
            _absN = Math.Abs(_nnz);
            _cn = 1.0 / _nnz / 2.0;
            _isoDd = (int) Math.Abs(P1) % 2;
        }
    }
}