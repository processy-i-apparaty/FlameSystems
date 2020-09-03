using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Checks : VariationModel
    {
        private double _cs;
        private double _ncx;
        private double _ncy;

        public Checks()
        {
            SetParameters(new[] {5.0, 5.0, 5.0, 0.0}, new[] {"x", "y", "size", "rnd"});
        }

        public override int Id { get; } = 76;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = (int) VariationHelper.Rint(p.X * _cs) + (int) VariationHelper.Rint(p.Y * _cs);
            var n3 = P4 * VariationHelper.Psi;
            var n4 = P4 * VariationHelper.Psi;
            double xx;
            double ncy;
            if (n2 % 2 == 0)
            {
                xx = _ncx + n3;
                ncy = _ncy;
            }
            else
            {
                xx = P1;
                ncy = P2 + n4;
            }

            var x = W * (p.X + xx);
            var y = W * (p.Y + ncy);


            return new Point(x, y);
        }

        public override void Init()
        {
            _cs = 1.0 / (P3 + VariationHelper.SmallDouble2);
            _ncx = P1 * -1.0;
            _ncy = P2 * -1.0;
        }
    }
}