using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Loonie2 : VariationModel
    {
        private double _cosA;
        private double _cosC;
        private double _cosS;
        private double _sinA;
        private double _sinC;
        private double _sinS;
        private double _sqrVVar;

        public Loonie2()
        {
            SetParameters(new[] {4.0, 0.0, 0.0}, new[] {"sides", "star", "circle"});
        }

        public override int Id { get; } = 152;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = p.X;
            var y = p.Y;
            var max = x * _cosS + Math.Abs(y) * _sinS;
            var sqrt = Math.Sqrt(VariationHelper.Sqr(x) + VariationHelper.Sqr(y));
            int i;
            for (i = 0; i < P1 - 1; ++i)
            {
                var n2 = x * _cosA - y * _sinA;
                y = x * _sinA + y * _cosA;
                x = n2;
                max = Math.Max(max, x * _cosS + Math.Abs(y) * _sinS);
            }

            var n3 = max * _cosC + sqrt * _sinC;
            double sqr;
            var point = new Point();

            if (i > 1)
                sqr = VariationHelper.Sqr(n3);
            else
                sqr = Math.Abs(n3) * n3;
            if (sqr > 0.0 && sqr < _sqrVVar)
            {
                var n4 = W * Math.Sqrt(Math.Abs(_sqrVVar / sqr - 1.0));
                point.X += n4 * p.X;
                point.Y += n4 * p.Y;
            }
            else if (sqr < 0.0)
            {
                var n5 = W / Math.Sqrt(Math.Abs(_sqrVVar / sqr) - 1.0);
                point.X += n5 * p.X;
                point.Y += n5 * p.Y;
            }
            else
            {
                point.X += W * p.X;
                point.Y += W * p.Y;
            }


            return point;
        }

        public override void Init()
        {
            _sqrVVar = W * W;
            var n2 = VariationHelper.TwoPi / P1;
            _sinA = Math.Sin(n2);
            _cosA = Math.Cos(n2);
            var n3 = -VariationHelper.HalfPi * P2;
            _sinS = Math.Sin(n3);
            _cosS = Math.Cos(n3);
            var n4 = VariationHelper.HalfPi * P3;
            _sinC = Math.Sin(n4);
            _cosC = Math.Cos(n4);
        }
    }
}