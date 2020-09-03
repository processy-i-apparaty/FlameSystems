using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Cross : VariationModel
    {
        public override int Id { get; } = 48;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var q = W * Math.Sqrt(1.0 / Math.Pow(p.X * p.X - p.Y * p.Y, 2.0));
            return new Point(q * p.X, q * p.Y);
        }

        public override void Init()
        {
        }
    }
}