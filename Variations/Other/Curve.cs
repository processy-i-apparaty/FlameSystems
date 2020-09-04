using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Curve : VariationModel
    {
        private double _pcXLen;
        private double _pcYLen;

        public Curve()
        {
            SetParameters(new[] {.25, .5, 1.0, 1.0}, new[] {"xAmp", "yAmp", "xLength", "yLength"});
        }

        public override int Id { get; } = 98;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = W * (p.X + P1 * Math.Exp(-p.Y * p.Y / _pcXLen));
            var y = W * (p.Y + P2 * Math.Exp(-p.X * p.X / _pcYLen));

            return new Point(x, y);
        }

        public override void Init()
        {
            _pcXLen = P3 * P3;
            _pcYLen = P4 * P4;
            if (_pcXLen < 1.0E-20) _pcXLen = 1.0E-20;
            if (_pcYLen < 1.0E-20) _pcYLen = 1.0E-20;
        }
    }
}