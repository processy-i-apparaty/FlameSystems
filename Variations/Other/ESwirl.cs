using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class ESwirl : VariationModel
    {
        private double _in;
        private double _out;

        public ESwirl()
        {
            SetParameters(new[] {1.2, 0.2}, new[] {"in", "out"});
        }

        public override int Id { get; } = 118;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = p.Y * p.Y + p.X * p.X + 1.0;
            var n3 = 2.0 * p.X;
            var n4 = (VariationHelper.SqrtSafe(n2 + n3) + VariationHelper.SqrtSafe(n2 - n3)) * 0.5;
            if (n4 < 1.0) n4 = 1.0;

            var acosh = VariationHelper.Acosh(n4);
            var n5 = p.X / n4;
            if (n5 > 1.0)
                n5 = 1.0;
            else if (n5 < -1.0) n5 = -1.0;

            var acos = Math.Acos(n5);
            if (p.Y < 0.0) acos *= -1.0;

            var n6 = acos + acosh * _out + _in / acosh;
            var sinh = Math.Sinh(acosh);
            var cosh = Math.Cosh(acosh);
            var sin = Math.Sin(n6);
            var x = W * cosh * Math.Cos(n6);
            var y = W * sinh * sin;


            return new Point(x, y);
        }

        public override void Init()
        {
            _in = P1;
            _out = P2;
        }
    }
}