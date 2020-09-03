using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Exponential : VariationModel
    {
        public override int Id { get; } = 18;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
//            var ex = Math.Exp(p.X - 1.0);
//            var piY = Math.PI * p.Y;
//            return new Point(
//                ex * Math.Cos(piY),
//                ex * Math.Sin(piY));

            var n2 = Math.PI * p.Y;
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * Math.Exp(p.X - 1.0);
            return new Point(cos * n3, sin * n3);
        }

        public override void Init()
        {
        }
    }
}