using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Gaussian : VariationModel
    {
        public override int Id { get; } = 35;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var q = 0.0;
            //            for (var i = 0; i < 4; i++) q += VariationHelper.Psi - 2.0;
            //            var w = 2.0 * Math.PI * VariationHelper.Psi;
            //            return new Point(q * Math.Cos(w), q * Math.Sin(w));

            var n2 = VariationHelper.Psi * 2.0 * Math.PI;
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * (VariationHelper.Psi + VariationHelper.Psi +
                VariationHelper.Psi + VariationHelper.Psi - 2.0);
            return new Point(n3 * cos, n3 * sin);
        }

        public override void Init()
        {
        }
    }
}