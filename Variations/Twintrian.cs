using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Twintrian : VariationModel
    {
        public override int Id { get; } = 47;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = VariationHelper.Random() * W * VariationHelper.PreSqrt(p);
            var sin = Math.Sin(n2);
            var n3 = Math.Log10(sin * sin) + Math.Cos(n2);
            if (Math.Abs(n3) < 1.0E-300) n3 = -30.0;
            return new Point(W * p.X * n3, W * p.X * (n3 - sin * Math.PI));
        }
    }
}