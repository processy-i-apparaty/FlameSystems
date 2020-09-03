using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Boarders2 : VariationModel
    {
        private double _c;
        private double _cl;
        private double _cr;

        public Boarders2()
        {
            SetParameters(new[] {0.4, 0.65, 0.35}, new[] {"c", "left", "right"});
        }

        public override int Id { get; } = 70;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var rint = VariationHelper.Rint(p.X);
            var rint2 = VariationHelper.Rint(p.Y);
            var n2 = p.X - rint;
            var n3 = p.Y - rint2;
            var point = new Point();

            if (VariationHelper.Psi >= _cr)
            {
                point.X += W * (n2 * _c + rint);
                point.Y += W * (n3 * _c + rint2);
            }
            else if (Math.Abs(n2) >= Math.Abs(n3))
            {
                if (n2 >= 0.0)
                {
                    point.X += W * (n2 * _c + rint + _cl);
                    point.Y += W * (n3 * _c + rint2 + _cl * n3 / n2);
                }
                else
                {
                    point.X += W * (n2 * _c + rint - _cl);
                    point.Y += W * (n3 * _c + rint2 - _cl * n3 / n2);
                }
            }
            else if (n3 >= 0.0)
            {
                point.Y += W * (n3 * _c + rint2 + _cl);
                point.X += W * (n2 * _c + rint + n2 / n3 * _cl);
            }
            else
            {
                point.Y += W * (n3 * _c + rint2 - _cl);
                point.X += W * (n2 * _c + rint - n2 / n3 * _cl);
            }

            return point;
        }

        public override void Init()
        {
            _c = Math.Abs(P1);
            _cl = Math.Abs(P2);
            _cr = Math.Abs(P3);
            _c = Math.Abs(_c) <= double.Epsilon ? VariationHelper.SmallDouble2 : _c;
            _cl = Math.Abs(_cl) <= double.Epsilon ? VariationHelper.SmallDouble2 : _cl;
            _cr = Math.Abs(_cr) <= double.Epsilon ? VariationHelper.SmallDouble2 : _cr;
            _cl *= _c;
            _cr = _c + _c * _cr;
        }
    }
}