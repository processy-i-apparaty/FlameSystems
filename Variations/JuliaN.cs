using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class JuliaN : VariationModel
    {
        private const double TwoPi = Math.PI * 2.0;

        public JuliaN()
        {
            SetParameters(new[] { 2.0, 2.0 }, new[] { "power", "dist" });
        }

        public override int Id { get; } = 32;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var pp3 = VariationHelper.Trunc(Math.Abs(P1) * VariationHelper.Psi);
            //            var t = (VariationHelper.Phi(p) + 2.0 * Math.PI * pp3) / P1;
            //            var r2 = Math.Pow(r, P2 / P1);
            //            var dx = r2 * Math.Cos(t);
            //            var dy = r2 * Math.Sin(t);
            //            return new Point(dx, dy);

            var power = Math.Floor(P1);
            var dist = P2;
            var absPower = Math.Abs((int) power);
            var cPower = dist / power * 0.5;

            var n2 = (Math.Atan2(p.Y, p.X) + TwoPi * VariationHelper.RandomNext(absPower)) / power;
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * Math.Pow(p.X * p.X + p.Y * p.Y, cPower);
            return new Point(n3 * cos, n3 * sin);
        }
    }
}