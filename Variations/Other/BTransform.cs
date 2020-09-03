using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class BTransform : VariationModel
    {
        public BTransform()
        {
            SetParameters(new[] {0.0, 1.0, 0.0, 0.0}, new[] {"rotate", "power", "move", "split"});
        }

        public override int Id { get; } = 61;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var rotate = P1;
            var power = P2;
            var move = P3;
            var split = P4;


            var point = new Point();
            var n2 = 0.5 * (Math.Log(VariationHelper.Sqr(p.X + 1.0) + VariationHelper.Sqr(p.Y)) -
                            Math.Log(VariationHelper.Sqr(p.X - 1.0) + VariationHelper.Sqr(p.Y))) / power + move;
            var n3 = (Math.PI - Math.Atan2(p.Y, p.X + 1.0) - Math.Atan2(p.Y, 1.0 - p.X) + rotate) / power +
                     VariationHelper.TwoPi / power * Math.Floor(VariationHelper.Psi * power);
            double n4;
            if (p.X >= 0.0)
                n4 = n2 + split;
            else
                n4 = n2 - split;
            var sinh = Math.Sinh(n4);
            var cosh = Math.Cosh(n4);
            var sin = Math.Sin(n3);
            var n5 = cosh - Math.Cos(n3);
            point.X += W * sinh / n5;
            point.Y += W * sin / n5;

            return point;
        }

        public override void Init()
        {
        }
    }
}