using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Cosine : VariationModel

    {
        public override int Id { get; } = 20;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var piX = Math.PI * p.X;
            //            return new Point(
            //                W * Math.Cos(piX) * Math.Cos(p.Y),
            //                W * -Math.Sin(piX) * Math.Sin(p.Y));

            var n2 = p.X * Math.PI;
            var sin = Math.Sin(n2);
            return new Point(W * Math.Cos(n2) * Math.Cosh(p.Y), -W * sin * Math.Sinh(p.Y));
        }
    }
}