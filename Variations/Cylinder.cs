using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Cylinder : VariationModel
    {
        public override int Id { get; } = 29;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return new Point(W * Math.Sin(p.X), W * p.Y);
        }

        public override void Init()
        {
        }
    }
}