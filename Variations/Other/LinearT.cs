using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class LinearT : VariationModel
    {
        public LinearT()
        {
            SetParameters(new[] {1.2, 1.2}, new[] {"powX", "powY"});
        }

        public override int Id { get; } = 147;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = Sgn(p.X) * Math.Pow(Math.Abs(p.X), P1) * W;
            var y = Sgn(p.Y) * Math.Pow(Math.Abs(p.Y), P2) * W;


            return new Point(x, y);
        }

        public override void Init()
        {
        }

        private static double Sgn(double n)
        {
            if (n > 0.0) return 1.0;
            return -1.0;
        }
    }
}