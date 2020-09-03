using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class V95 : VariationModel
    {
        private double ang;
        private double c;
        private double coeff;
        private double d;
        private double half_c;
        private double half_d;
        private double p_a;

        public V95()
        {
            SetParameters(new[] {1.0, 1.0, 1.0, 1.0}, new[] {"r", "d", "divisor", "spread"});
        }

        public override int Id { get; } = 95;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var precalcAtanYX = VariationHelper.PreAtanYx(p);
            if (precalcAtanYX < 0.0) precalcAtanYX += VariationHelper.TwoPi;

            if (Math.Cos(precalcAtanYX / 2.0) < VariationHelper.Psi * 2.0 - 1.0) precalcAtanYX -= VariationHelper.TwoPi;

            var n2 = precalcAtanYX + (VariationHelper.Psi < 0.5 ? VariationHelper.TwoPi : -VariationHelper.TwoPi) *
                Math.Round(Math.Log(VariationHelper.Psi) * coeff);
            var log = Math.Log(VariationHelper.PreSumSq(p));
            var n3 = W * Math.Exp(half_c * log - d * n2);
            var n4 = c * n2 + half_d * log + ang * Math.Floor(P3 * VariationHelper.Psi);
            var x = n3 * Math.Cos(n4);
            var y = n3 * Math.Sin(n4);


            return new Point(x, y);
        }

        public override void Init()
        {
            ang = 6.283185307179586 / P3;
            p_a = Math.Atan2((this.p_d < 0.0 ? -Math.Log(-this.p_d) : Math.Log(this.p_d)) * this.p_r,
                6.283185307179586);
            c = Math.Cos(p_a) * this.p_r * Math.Cos(p_a) / P3;
            d = Math.Cos(p_a) * this.p_r * Math.Sin(p_a) / P3;
            half_c = c / 2.0;
            half_d = d / 2.0;
            coeff = d == 0.0 ? 0.0 : -0.095 * P4 / d;
        }
    }
}