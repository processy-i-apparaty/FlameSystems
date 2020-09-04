using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class Crown : VariationModel
    {
        public Crown()
        {
            SetParameters(new[] {5.0, 0.6309298}, new[] {"a", "b"});
        }

        public override int Id { get; } = 95;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = -Math.PI + VariationHelper.TwoPi * VariationHelper.Psi;
            var complex = new Complex(0.0, 0.0);
            for (var i = 1; i < 15; ++i)
            {
                var pow = Math.Pow(P1, P2 * i);
                var n3 = Math.Pow(P1, i) * Math.Pow(-1.0, i) * n2;
                complex.Add(new Complex(Math.Sin(n3) / pow, Math.Cos(n3) / pow));
            }

            var re = complex.Re;
            var im = complex.Im;
            var x = re * W;
            var y = im * W;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}