using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutSpiralCb : VariationModel
    {
        private double _invert;
        private double _mode;
        private double _time;
        private double _zoom;

        public CutSpiralCb()
        {
            SetParameters(new[] { 0.0, 1.0, 2.5, .0 }, new[] { "time", "mode", "zoom", "invert" });
        }

        public override int Id { get; } = 107;
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
                x = 2.0 * VariationHelper.Psi - 1.0;
                y = 2.0 * VariationHelper.Psi - 1.0;
            }

            var computeSpiral = ComputeSpiral(new Vec2(x * _zoom, y * _zoom), _time);
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (computeSpiral > 0.0)
                {
                    x = 0.0;
                    y = 0.0;
                    return p;
                }
            }
            else if (computeSpiral <= 0.0)
            {
                x = 0.0;
                y = 0.0;
                return p;
            }

            var dx = W * x;
            var dy = W * y;


            return new Point(dx, dy);
        }

        public override void Init()
        {
            _time = P1;
            _mode = P2;
            _zoom = P3;
            _invert = P4;
        }

        public Mat2 Rotate(double n)
        {
            return new Mat2(-Math.Sin(n), Math.Cos(n), Math.Cos(n), Math.Sin(n));
        }

        private static double Hill(double n, double n2, double n3)
        {
            return Math.Min(G.Step(n - n2 / 2.0, n3), 1.0 - G.Step(n + n2 / 2.0, n3));
        }

        private double ComputeSpiral(Vec2 vec2, double n)
        {
            var n2 = G.Length(vec2) * 50.0;
            var atan2 = G.Atan2(vec2.Y, vec2.X);
            vec2 = Rotate(_time * 7.0).Times(vec2);
            vec2 = vec2.Multiply(Math.Sin(atan2 * 15.0));
            vec2 = Rotate(n2).Times(vec2);
            return G.Mix(1.0, 0.0, Math.Min(G.Step(0.0, vec2.X), Hill(0.0, n2 / 25.0, vec2.Y)));
        }
    }
}