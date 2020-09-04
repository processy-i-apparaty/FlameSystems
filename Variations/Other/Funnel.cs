using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Funnel : VariationModel
    {
        private double _effect;

        public Funnel()
        {
            SetParameters(new[] {4.0}, new[] {"effect"});
        }

        public override int Id { get; } = 127;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = W * Math.Tanh(p.X) * (1.0 / Math.Cos(p.X) + _effect * Math.PI);
            var y = W * Math.Tanh(p.Y) * (1.0 / Math.Cos(p.Y) + _effect * Math.PI);
            
            return new Point(x, y);
        }

        public override void Init()
        {
            _effect = VariationHelper.Map(P1, -10, 10, -20, 20);
        }
    }
}