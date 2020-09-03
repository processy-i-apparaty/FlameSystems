using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Pie : VariationModel
    {
        private double _p1;

        public Pie()
        {
            SetParameters(new[] {6.0, 0.0, 0.5}, new[] {"slices", "rotation", "thickness"});
        }

        public override int Id { get; } = 37;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var t1 = Math.Floor(VariationHelper.Psi * P1 + 0.5);
            var t2 = P2 + 2.0 * _p1 * (t1 + VariationHelper.Psi * P3);
            var psi = VariationHelper.Psi;
            return new Point(psi * Math.Cos(t2) * W, psi * Math.Sin(t2) * W);
        }

        public override void Init()
        {
            _p1 = Math.PI / P1;
        }
    }
}