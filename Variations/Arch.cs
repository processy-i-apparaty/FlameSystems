using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Arch : VariationModel
    {
        public override int Id { get; } = 41;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n1 = VariationHelper.Random() * Math.PI * W;
            var n2 = Math.Sin(n1);
            var sinr = (1.0 - Math.Cos(2.0 * n1)) / 2.0;
            var n3 = sinr / Math.Cos(n1);

            return new Point(W * n2, W * n3);
        }
    }
}