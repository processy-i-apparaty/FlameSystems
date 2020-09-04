using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class InvPolar : VariationModel
    {
        
        public override int Id { get; } = 138;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 1.0 + p.Y;
            var x = W * n2 * Math.Sin(p.X * Math.PI);
            var y = W * n2 * Math.Cos(p.X * Math.PI);

            return new Point(x, y);
        }

        public override void Init()
        {
        }
    }
}