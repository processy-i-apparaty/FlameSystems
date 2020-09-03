using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Swirl : VariationModel
    {
        public override int Id { get; } = 3;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var rSquare = r * r;
            //            return new Point(
            //                p.X * Math.Sin(rSquare) - p.Y * Math.Cos(rSquare)*W,
            //                p.X * Math.Cos(rSquare) + p.Y * Math.Sin(rSquare)*W);

            var n2 = VariationHelper.PreSumSq(p);
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            return new Point(W * (sin * p.X - cos * p.Y), W * (cos * p.X + sin * p.Y));
        }

        public override void Init()
        {
        }
    }
}