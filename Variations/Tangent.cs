using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Tangent : VariationModel
    {
        public override int Id { get; } = 42;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;
        public override Point Fun(Point p)
        {
            return new Point(Math.Sin(p.X) / Math.Cos(p.Y)*W, Math.Tan(p.Y)*W);
        }
    }
}