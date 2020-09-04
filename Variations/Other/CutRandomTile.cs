using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutRandomTile : VariationModel
    {
        private double _invert;
        private double _mode;
        private Random _randomize;

        private int _seed;
        private vec3 _vec3;

        private double _x0;
        private double _y0;
        private double _zoom;

        public CutRandomTile()
        {
            SetParameters(new[] {0.0, 1.0, 5.0, 0.0}, new[] {"seed", "mode", "zoom", "invert"});
        }

        public override int Id { get; } = 104;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double n2;
            double n3;
            double n4;
            double n5;
            if (Math.Abs(_mode) <= VariationHelper.SmallDouble)
            {
                n2 = p.X;
                n3 = p.Y;
                n4 = 0.0;
                n5 = 0.0;
            }
            else
            {
                n2 = VariationHelper.Psi;
                n3 = VariationHelper.Psi;
                n4 = 0.5;
                n5 = 0.5;
            }

            // var vec2 = new vec2(n2, n3);
            var plus = new vec2(n2 * _zoom, n3 * _zoom).plus(new vec2(_x0, _y0));
            var vec3 = new vec2(4.0, 6.0);
            var tilePattern = TilePattern(plus);
            // var tilePattern2 = TilePattern(plus.plus(vec3));
            TilePattern(plus.plus(vec3.multiply(3.0)));
            // var smoothStep = G.smoothstep(0.0, 0.03333333333333333, tilePattern - 0.015);
            // var smoothStep2 = G.smoothstep(0.0, 0.03333333333333333, tilePattern2 - 0.015);
            // var n6 = Math.Max(smoothStep2 - smoothStep, 0.0) / G.Length(vec3);
            // var n7 = Math.Max(smoothStep - smoothStep2, 0.0) / G.Length(vec3);

            const double n8 = 0.005;
            var smoothStep3 = G.smoothstep(0.0, 0.5, GrungeTex(plus.division(4.0).plus(0.5)));
            smoothStep3.multiply(new vec3(0.85, 0.68, 0.51));
            var mix = G.Mix(new vec3(0.0), new vec3(0.0), SStep(n8, tilePattern));
            new vec3(2.5, 0.75, 0.25).multiply(smoothStep3);

            var x = G.sqrt(G.max(G.Mix(mix, _vec3, SStep(n8, tilePattern + 0.025)), 0.0)).x;

            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (x > 0.0)
                {
                    n2 = 0.0;
                    n3 = 0.0;
                }
            }
            else if (x <= 0.0)
            {
                n2 = 0.0;
                n3 = 0.0;
            }

            var dx = W * (n2 - n4);
            var dy = W * (n3 - n5);

            return new Point(dx, dy);
        }

        public override void Init()
        {
            _seed = (int) VariationHelper.Map(P1, -10, 10, int.MinValue, int.MaxValue);
            _mode = P2;
            _zoom = VariationHelper.Map(P3, -10, 10, -20, 20);
            _invert = P4;
            _vec3 = new vec3(1.0);
            _randomize = new Random(_seed);
            _x0 = _seed * _randomize.NextDouble();
            _y0 = _seed * _randomize.NextDouble();
        }


        private double SStep(double n, double n2)
        {
            return 1.0 - G.smoothstep(0.0, n, n2);
        }

        private mat2 Rot2(double n)
        {
            var cos = Math.Cos(n);
            var sin = Math.Sin(n);
            return new mat2(cos, -sin, sin, cos);
        }

        private double Hash21(vec2 vec2)
        {
            return G.Fract(Math.Sin(G.dot(vec2, new vec2(127.183, 157.927))) * 43758.5453);
        }

        private double N2D(vec2 vec2)
        {
            var floor = G.Floor(vec2);
            vec2 = vec2.minus(floor);
            vec2 = new vec2(3.0).minus(vec2.multiply(2.0)).multiply(vec2).multiply(vec2);
            return G.dot(
                new vec2(1.0 - vec2.y, vec2.y).times(new mat2(G.Fract(G.sin(new vec4(0.0, 1.0, 113.0, 114.0)
                    .plus(G.dot(floor, new vec2(1.0, 113.0))).multiply(43758.5453))))), new vec2(1.0 - vec2.x, vec2.x));
        }

        private double Fbm(vec2 vec2)
        {
            return N2D(vec2) * 0.533 + N2D(vec2.multiply(2.0)) * 0.267 + N2D(vec2.multiply(4.0)) * 0.133 +
                   N2D(vec2.multiply(8.0)) * 0.067;
        }

        public double TilePattern(vec2 vec2)
        {
            var floor = G.Floor(vec2);
            vec2 = vec2.minus(floor.plus(0.5));
            var hash21 = Hash21(floor);
            var hash22 = Hash21(floor.plus(27.93));
            vec2 = vec2.times(Rot2(G.Floor(hash21 * 4.0) * 3.14159 / 2.0));
            var length = G.Length(vec2.minus(new vec2(-0.5, 0.5)));
            var abs = Math.Abs(length - 0.25);
            var abs2 = Math.Abs(length - 0.5);
            var b = Math.Abs(length - 0.75);
            if (hash22 > 0.33) b = Math.Abs(G.Length(vec2.minus(new vec2(0.125, 0.5))) - 0.125);
            var min = Math.Min(Math.Min(Math.Min(abs, abs2), b),
                Math.Min(100000.0,
                    Math.Min(Math.Abs(G.Length(vec2.minus(new vec2(0.5, 0.125))) - 0.125),
                        Math.Abs(G.Length(vec2.minus(new vec2(0.5, -0.5))) - 0.25))));
            var length2 = G.Length(vec2.plus(0.5));
            var max = Math.Max(min, -(length2 - 0.75));
            var abs3 = Math.Abs(length2 - 0.5);
            return Math.Min(max, Math.Min(abs3, Math.Abs(abs3 - 0.25))) - 0.0625;
        }

        public double SFract(double fract, double n)
        {
            fract = G.Fract(fract);
            return Math.Min(fract, (1.0 - fract) * fract * n);
        }

        public vec3 GrungeTex(vec2 vec2)
        {
            var n = N2D(vec2.multiply(3.0)) * 0.57 + N2D(vec2.multiply(7.0)) * 0.28 + N2D(vec2.multiply(15.0)) * 0.15;
            var multiply = G.Mix(new vec3(0.25, 0.1, 0.02), new vec3(0.35, 0.5, 0.65), n)
                .multiply(N2D(vec2.multiply(new vec2(150.0, 350.0))) * 0.5 + 0.5);
            var mix = G.Mix(multiply, multiply.multiply(new vec3(0.75, 0.95, 1.2)), SFract(n * 4.0, 12.0));
            var mix2 = G.Mix(mix, mix.multiply(new vec3(1.2, 1.0, 0.8).multiply(0.8)),
                SFract(n * 5.0 + 0.35, 12.0) * 0.5);
            var n2 = N2D(vec2.multiply(8.0).plus(0.5)) * 0.7 + N2D(vec2.multiply(18.0).plus(0.5)) * 0.3;
            return G.Clamp(G.Mix(mix2.multiply(0.6), mix2.multiply(1.4), n2 * 0.7 + SFract(n2 * 5.0, 16.0) * 0.3), 0.0,
                1.0);
        }
    }
}