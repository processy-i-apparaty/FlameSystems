using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Curliecue2 : VariationModel
    {
        private double _phi;
        private Random _randomize;
        private double _s;
        private double _theta;
        private double _x0;
        private double _x1;
        private double _y0;
        private double _y1;

        public Curliecue2()
        {
            SetParameters(new[] {1.0}, new[] {"seed"});
        }

        public override int Id { get; } = 99;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            _x1 = _x0 + 0.001 * Math.Cos(_phi);
            _y1 = _y0 + 0.001 * Math.Sin(_phi);
            _x0 = _x1;
            _y0 = _y1;
            _phi = (_theta + _phi) % VariationHelper.TwoPi;
            _theta = (_theta + VariationHelper.TwoPi * _s) % VariationHelper.TwoPi;
            var x = W * _x0;
            var y = W * _y0;

            return new Point(x, y);
        }

        public override void Init()
        {
            var seed = (int) VariationHelper.Map(P1, -10, 10, int.MinValue, int.MaxValue);
            _randomize = new Random(seed);
            _x0 = 0.0;
            _y0 = 0.0;
            _theta = 0.0;
            _phi = 0.0;
            _s = _randomize.NextDouble();
        }
    }
}