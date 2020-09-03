using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Chrysanthemum : VariationModel
    {
        public override int Id { get; } = 77;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 65.97344572538566 * VariationHelper.Psi;
            var sin = Math.Sin(17.0 * n2 / 3.0);
            var sin2 = Math.Sin(2.0 * Math.Cos(3.0 * n2) - 28.0 * n2);
            var n3 = (5.0 * (1.0 + Math.Sin(11.0 * n2 / 5.0)) -
                      4.0 * sin * sin * sin * sin * sin2 * sin2 * sin2 * sin2 * sin2 * sin2 * sin2 * sin2) * W;
            var x = n3 * Math.Cos(n2);
            var y = n3 * Math.Sin(n2);

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}