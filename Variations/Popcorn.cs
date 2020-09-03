using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Popcorn : VariationModel
    {
        public override int Id { get; } = 17;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = true;

        public override Point Fun(Point p)
        {
            var tan = Math.Tan(3.0 * p.Y);
            if (double.IsNaN(tan) || double.IsInfinity(tan)) tan = 0.0;
            var tan2 = Math.Tan(3.0 * p.X);
            if (double.IsNaN(tan2) || double.IsInfinity(tan2)) tan2 = 0.0;

            var n2 = p.X + E * Math.Sin(tan);
            var n3 = p.Y + F * Math.Sin(tan2);
            return new Point(W * n2, W * n3);
        }

        public override void Init()
        {
        }
    }
}