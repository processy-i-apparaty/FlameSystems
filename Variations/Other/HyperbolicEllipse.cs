using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class HyperbolicEllipse : VariationModel
    {
        public HyperbolicEllipse()
        {
            SetParameters(new[] {1.0}, new[] {"a"});
        }

        public override int Id { get; } = 134;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var eX = Math.Exp(p.X);
            var eXMinus = Math.Exp(-p.X);
            var p1Y = P1 * p.Y;
            var x = (eX - eXMinus) / 2.0 * Math.Cos(p1Y);
            var y = (eX + eXMinus) / 2.0 * Math.Sin(p1Y);
            return new Point(x * W, y * W);
        }

        public override void Init()
        {
        }
    }
}