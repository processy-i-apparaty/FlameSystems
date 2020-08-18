using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Blob : VariationModel
    {
        public Blob()
        {
            SetParameters(new[] { 2.0, 1.0, 2.0 }, new[] { "low", "high", "waves" });
        }

        public override int Id { get; } = 23;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var preAtan = VariationHelper.PreAtan(p);
            var n2 = VariationHelper.R(p) * (P1 + (P2 - P1) * (0.5 + 0.5 * Math.Sin(P3 * preAtan)));
            var n3 = Math.Sin(preAtan) * n2;
            var n4 = Math.Cos(preAtan) * n2;
            return new Point(W * n3, W * n4);
        }
    }
}