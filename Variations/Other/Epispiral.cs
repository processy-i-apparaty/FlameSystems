using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Epispiral : VariationModel
    {
        public Epispiral()
        {
            SetParameters(new[] {6.0, 0.0, 1.0}, new[] {"n", "thickness", "holes"});
        }

        public override int Id { get; } = 122;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var atan2 = Math.Atan2(p.Y, p.X);
            double n2 = -P3;
            double n3;
            if (Math.Abs(P2) > 1.0E-8)
            {
                var cos = Math.Cos(P1 * atan2);
                if (Math.Abs(cos) <= VariationHelper.SmallDouble) return p;
                n3 = n2 + VariationHelper.Psi * P2 * (1.0 / cos);
            }
            else
            {
                var cos2 = Math.Cos(P1 * atan2);
                if (Math.Abs(cos2) <= VariationHelper.SmallDouble) return p;
                n3 = n2 + 1.0 / cos2;
            }

            var x = W * n3 * Math.Cos(atan2);
            var y = W * n3 * Math.Sin(atan2);


            return new Point(x, y);
        }

        public override void Init()
        {

        }
    }
}