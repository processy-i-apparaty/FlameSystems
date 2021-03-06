using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutX : VariationModel
    {
        private double _invert;
        private double _mode;
        private double _size;
        private double _zoom;

        public CutX()
        {
            SetParameters(new[] {1.0, 1.0, 0.0, 0.1}, new[] {"mode", "zoom", "invert", "size"});
        }

        public override int Id { get; } = 111;
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

            var abs = G.Abs(new Vec2(x, y).Multiply(_zoom));
            const double n2 = 0.0;
            var size = _size;
            Vec2 abs2;
            var vec2 = abs2 = G.Abs(Rot(0.7853981633975).Times(abs));
            abs2.Y -= size;
            var mix = G.Mix(n2, 1.0, G.Smoothstep(0.0, 0.009, vec2.Y));
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (mix < 0.1)
                {
                    // x = 0.0;
                    // y = 0.0;
                    return new Point(p.X,p.Y);
                }
            }
            else if (mix >= 0.1)
            {
                // x = 0.0;
                // y = 0.0;
                return new Point(p.X, p.Y);
            }

            var dx = W * x;
            var dy = W * y;

            return new Point(dx, dy);
        }

        public override void Init()
        {
            _mode = P1;
            _zoom = P2;
            _invert = P3;
            _size = P4;
        }

        public Mat2 Rot(double n)
        {
            return new Mat2(Math.Cos(n), -Math.Sin(n), Math.Sin(n), Math.Cos(n));
        }
    }
}