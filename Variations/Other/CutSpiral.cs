using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutSpiral : VariationModel
    {
        private double _invert;
        private double _mode;
        private double _time;
        private double _zoom;

        public CutSpiral()
        {
            SetParameters(new[] {0.0, 1.0, 2.5, .0}, new[] {"time", "mode", "zoom", "invert"});
        }

        public override int Id { get; } = 106;
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

            var times = new vec2(x * _zoom, y * _zoom);
            var n2 = 0;
            var rot2 = Rot2(_time);
            for (var i = 0; i < 64; ++i)
            {
                times = rot2.times(times);
                if (times.y > 1.0) break;

                n2 ^= 0x1;
            }

            double n3 = n2;
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (n3 > 0.0)
                {
                    x = 0.0;
                    y = 0.0;
                }
            }
            else if (n3 <= 0.0)
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
            _time = P1;
            _mode = P2;
            _zoom = P3;
            _invert = P4;
        }

        public mat2 Rot2(double n)
        {
            return new mat2(1.1 * Math.Sin(n), 1.1 * Math.Cos(n), -1.1 * Math.Cos(n), 1.1 * Math.Sin(n));
        }
    }
}