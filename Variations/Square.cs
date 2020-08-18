using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Square : VariationModel
    {
        public override int Id { get; } = 43;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return new Point(W * VariationHelper.Random() - 0.5, W * VariationHelper.Random() - 0.5);
        }
    }
}