using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class CloverLeaf : VariationModel
    {
        private int _filled;

        public CloverLeaf()
        {
            SetParameters(new[] {1.0}, new[] {"filled"});
        }

        public override int Id { get; } = 84;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var precalcAtan = VariationHelper.PreAtan(p);
            var n2 = Math.Sin(2.0 * precalcAtan) + 0.25 * Math.Sin(6.0 * precalcAtan);
            if (_filled == 1) n2 *= VariationHelper.Psi;

            var n3 = Math.Sin(precalcAtan) * n2;
            var n4 = Math.Cos(precalcAtan) * n2;
            var x = W * n3;
            var y = W * n4;


            return new Point(x, y);
        }

        public override void Init()
        {
            _filled = (int) Math.Floor(P1);
        }
    }
}