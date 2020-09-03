using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Sinusoidal : VariationModel
    {
        public override int Id { get; } = 1;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return new Point(W * Math.Sin(p.X), W * Math.Sin(p.Y));
        }

        public override void Init()
        {
        }
    }
}