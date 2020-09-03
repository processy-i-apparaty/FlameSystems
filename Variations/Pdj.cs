using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Pdj : VariationModel
    {
        public Pdj()
        {
            SetParameters(new[] {2.0, 2.0, 2.0, 2.0}, new[] {"a", "b", "c", "d"});
        }

        public override int Id { get; } = 24;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return new Point(
                (Math.Sin(P1 * p.Y) - Math.Cos(P2 * p.X)) * W,
                (Math.Sin(P3 * p.X) - Math.Cos(P4 * p.Y)) * W
            );
        }

        public override void Init()
        {
        }
    }
}