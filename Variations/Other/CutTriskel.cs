using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutTriskel : VariationModel
    {
        private double _invert;
        private double _mode;
        private double _zoom;

        public CutTriskel()
        {
            SetParameters(new[] {1.0, 1.0, 0.0}, new[] {"mode", "zoom", "invert"});
        }

        public override int Id { get; } = 110;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            const double n2 = 0.0;
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

            vec2 vec2;
            var times = vec2 = new vec2(x * _zoom, y * _zoom);
            vec2.y += 0.1;
            var max = n2 - n2;
            for (var i = 0; i < 3; ++i)
            {
                times = new mat2(-0.5, 0.866, -0.866, -0.5).times(times);
                var plus = times.plus(new vec2(0.03, -0.577));
                var n3 = 3.0 * G.Length(plus);
                var atan2 = G.atan2(plus.y, plus.x);
                max = G.max(max,
                    n3 + G.Fract(atan2 / 7.0 + 0.3) < 2.0 ? 0.5 + 0.5 * Math.Sin(atan2 + 6.24 * n3) : 0.0);
            }

            var n4 = G.smoothstep(0.0, 0.1, Math.Abs(max - 0.5)) - G.smoothstep(0.8, 0.9, max);
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (n4 > 0.5)
                {
                    x = 0.0;
                    y = 0.0;
                    return p;
                }
            }
            else if (n4 <= 0.5)
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
            _mode = P1;
            _zoom = P2;
            _invert = P3;
        }
    }
}