using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Escher : VariationModel
    {
        public Escher()
        {
            SetParameters(new[] {0.3}, new[] {"beta"});
        }

        public override int Id { get; } = 123;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var precalcAtanYx = VariationHelper.PreAtanYx(p);
            var n2 = 0.5 * Math.Log(VariationHelper.PreSumSq(p));
            var sin = Math.Sin(P1);
            var n3 = 0.5 * (1.0 + Math.Cos(P1));
            var n4 = 0.5 * sin;
            var n5 = W * Math.Exp(n3 * n2 - n4 * precalcAtanYx);
            var n6 = n3 * precalcAtanYx + n4 * n2;
            var sin2 = Math.Sin(n6);
            var x = n5 * Math.Cos(n6);
            var y = n5 * sin2;


            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}