using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutHexDots : VariationModel
    {
        private double _invert;
        private double _mode;
        private double _size;
        private double _zoom;

        public CutHexDots()
        {
            SetParameters(new[] {1.0, 0.5, 4.0, 0.0}, new[] {"mode", "size", "zoom", "invert"});
        }

        public override int Id { get; } = 103;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double x;
            double y;
            double n2;
            double n3;
            if (Math.Abs(_mode) <= VariationHelper.SmallDouble)
            {
                x = p.X;
                y = p.Y;
                n2 = 0.0;
                n3 = 0.0;
            }
            else
            {
                x = VariationHelper.Psi - 0.5;
                y = VariationHelper.Psi - 0.5;
                n2 = 0.0;
                n3 = 0.0;
            }

            var vec2 = new Vec2(x * _zoom, y * _zoom);
            var vec3 = new Vec2(1.0, 1.732);
            var minus = G.Mod(vec2, vec3).Multiply(2.0).Minus(vec3);
            var minus2 = G.Mod(vec2.Plus(vec3.Multiply(0.5)), vec3).Multiply(2.0).Minus(vec3);
            var n4 = 0.8 * G.Min(G.Dot(minus, minus), G.Dot(minus2, minus2));
            if (Math.Abs(_invert) <=VariationHelper.SmallDouble)
            {
                if (n4 > _size)
                {
                    x = 0.0;
                    y = 0.0;
                    return p;
                }
            }
            else if (n4 <= _size)
            {
                x = 0.0;
                y = 0.0;
                return p;
            }

            var dx = W * (x - n2);
            var dy = W * (y - n3);


            return new Point(dx, dy);
        }

        public override void Init()
        {
            _mode = P1;
            _size = P2;
            _zoom = P3;
            _invert = P4;
        }
    }
}