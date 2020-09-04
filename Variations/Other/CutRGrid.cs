using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutRGrid : VariationModel
    {
        private double _angle;
        private double _invert;

        private double _mode;
        private double _zoom;

        public CutRGrid()
        {
            SetParameters(new[] {1.0, 7.0, 0.0, 10.0}, new[] {"mode", "zoom", "invert", "angle"});
        }

        public override int Id { get; } = 105;
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

            var acos = Math.Acos(-1.0);
            var minus = new vec2(x * _zoom, y * _zoom);
            var mod = G.mod(_angle * acos / 180.0, acos * 2.0);
            var n2 = (0.5 + G.abs(Math.Sin(mod * 2.0)) * (Math.Sqrt(2.0) / 2.0 - 0.5)) * 2.0;
            var b = false;
            if (G.Fract(mod / acos + 0.25) > 0.5)
            {
                minus = minus.minus(0.5);
                b = true;
            }

            var multiply = minus.multiply(n2);
            var floor = G.Floor(multiply.division(n2));
            var times = G.mod(multiply, n2).minus(n2 / 2.0).multiply(G.mod(floor, 2.0).multiply(2.0).minus(1.0))
                .times(new mat2(Math.Cos(mod), Math.Sin(mod), -Math.Sin(mod), Math.Cos(mod)));
            var n3 = _zoom / 2000.0 * 1.5;
            var smoothStep = G.smoothstep(-n3, n3, G.max(Math.Abs(times.x), Math.Abs(times.y)) - 0.5);
            if (b) smoothStep = 1.0 - smoothStep;

            if (b && smoothStep < 0.5 &&
                (Math.Abs(times.x) - Math.Abs(times.y)) * G.sign(G.Fract(mod / acos) - 0.5) > 0.0)
                smoothStep = 0.4;

            if (!b && smoothStep < 0.5 && G.mod(floor.x + floor.y, 2.0) - 0.5 > 0.0) smoothStep = 0.4;


            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (Math.Abs(smoothStep) <= VariationHelper.SmallDouble)
                {
                    x = 0.0;
                    y = 0.0;
                }
            }
            else if (smoothStep > 0.0)
            {
                x = 0.0;
                y = 0.0;
            }

            var dx = W * x;
            var dy = W * y;

            return new Point(dx, dy);
        }

        public override void Init()
        {
            _angle = VariationHelper.Map(P4, -10, 10, 0, 180);
            _invert = P3;
            _zoom = P2;
            _mode = P1;
        }
    }
}