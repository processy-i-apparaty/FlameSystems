using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutSpots : VariationModel
    {
        private double _invert;
        private double _mode;
        private Random _randomize;
        private double _x0;
        private double _y0;
        private double _zoom;

        public CutSpots()
        {
            SetParameters(new[] {1.0, 0.0, 0.3, 0.0}, new[] {"mode", "seed", "zoom", "invert"});
        }

        public override int Id { get; } = 108;
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

            var plus = new vec2(x * _zoom * 43.0, y * _zoom * 43.0).plus(new vec2(_x0, _y0));
            const double n2 = 0.0;
            const double n3 = 1.0;
            var fbm = Fbm(plus);
            double mix;
            if (fbm < 0.4)
            {
                var n4 = Fbm(new vec2(plus.y, plus.x).plus(new vec2(1.0))) / 2.25;
                mix = G.Mix(n3, n3, Remap(0.25, 0.38, 0.0, 1.0, fbm - 0.06));
                if (fbm < n4) mix = n3;
            }
            else
            {
                mix = n2;
            }

            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (Math.Abs(mix) <= VariationHelper.SmallDouble)
                {
                    x = 0.0;
                    y = 0.0;
                }
            }
            else if (mix > 0.0)
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
            _mode = P1;
            _zoom = P3;
            _invert = P4;

            var seed = (int) VariationHelper.Map(P2, -10, 10, int.MinValue, int.MaxValue);
            _randomize = new Random(seed);
            _x0 = seed * _randomize.NextDouble();
            _y0 = seed * _randomize.NextDouble();
        }

        private static double Random(vec2 vec2)
        {
            return G.Fract(Math.Sin(G.dot(new vec2(vec2.x, vec2.y), new vec2(12.9898, 78.233))) * 43758.5453123);
        }

        private double noise(vec2 vec2)
        {
            var floor = G.Floor(vec2);
            var fract = G.Fract(vec2);
            var random = Random(floor);
            var random2 = Random(floor.plus(new vec2(1.0, 0.0)));
            var random3 = Random(floor.plus(new vec2(0.0, 1.0)));
            var random4 = Random(floor.plus(new vec2(1.0, 1.0)));
            var multiply = fract.multiply(fract).multiply(new vec2(3.0).minus(fract.multiply(2.0)));
            return G.Mix(random, random2, multiply.x) + (random3 - random) * multiply.y * (1.0 - multiply.x) +
                   (random4 - random2) * multiply.x * multiply.y;
        }

        private double Fbm(vec2 multiply)
        {
            var n = 0.0;
            var n2 = 0.5;
            for (var i = 0; i < 6; ++i)
            {
                n += n2 * noise(multiply);
                multiply = multiply.multiply(2.0);
                n2 *= 0.5;
            }

            return n;
        }

        private static double Remap(double n, double n2, double n3, double n4, double n5)
        {
            return n3 + (n5 - n) * (n4 - n3) / (n2 - n);
        }
    }
}