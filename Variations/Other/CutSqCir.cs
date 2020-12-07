using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutSqCir : VariationModel
    {
        private double _invert;
        private double _mode;
        private double _power;
        private double _zoom;

        public CutSqCir()
        {
            SetParameters(new[] {1.0, 2.0, 0.0, -6.0}, new[] {"mode", "zoom", "invert", "power"});
        }

        public override int Id { get; } = 109;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double x;
            double y;
            if (Math.Abs(_mode) <= VariationHelper.SmallDouble)
            {
                x = p.X;
                y = p.Y;
            }
            else
            {
                x = VariationHelper.Psi - 0.5;
                y = VariationHelper.Psi - 0.5;
            }

            var vec2 = new Vec2(x * _zoom, y * _zoom);
            var b = Math.Pow(Math.Abs(vec2.X - vec2.Y), _power) + Math.Pow(Math.Abs(vec2.Y + vec2.X), _power) <= 1.0;
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (!b)
                {
                    x = 0.0;
                    y = 0.0;
                    return p;
                }
            }
            else if (b)
            {
                return p;
            }

            var dx = W * x;
            var dy = W * y;
            return new Point(dx, dy);
        }

        public override void Init()
        {
            _zoom = P2;
            _mode = P1;
            _invert = P3;
            _power = VariationHelper.Map(P4, -10, 10, 0, 10);
        }
    }
}