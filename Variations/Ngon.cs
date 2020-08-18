using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class NGon : VariationModel
    {
        private const double TwoPi = Math.PI * 2.0;

        public NGon()
        {
            SetParameters(new[] { 2.0, 3.0, 5.0, 1.0 }, new[] { "power", "sides", "corners", "circle" });
        }

        public override int Id { get; } = 38;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var p2 = Math.PI * 2.0 / P2;
            //            var phi = VariationHelper.Phi(p);
            //            var t3 = phi - p2 * Math.Floor(phi / p2);
            //            var q = p2 / 2.0;
            //            var t4 = t3 > q ? t3 : t3 - p2;
            //            var r = VariationHelper.R(p);
            //            var k = (P3 * (1.0 / Math.Cos(t4) - 1.0) + P4) / Math.Pow(r, P1);
            //            return new Point(k * p.X, k * p.Y);
            //"power", "sides", "corners", "circle"
            var power = P1;
            var sides = P2;
            var corners = P3;
            var circle = P4;
            
            var pow = Math.Pow(VariationHelper.PreSumSq(p), power / 2.0);
            var preAtanYx = VariationHelper.PreAtanYx(p);
            var n2 = TwoPi / sides;
            var n3 = preAtanYx - n2 * Math.Floor(preAtanYx / n2);
            if (n3 > n2 / 2.0) n3 -= n2;
            var n4 = (corners * (1.0 / (Math.Cos(n3) + 1.0E-300) - 1.0) + circle) / (pow + 1.0E-300);
            return new Point(W * p.X * n4, W * p.Y * n4);
        }
    }
}