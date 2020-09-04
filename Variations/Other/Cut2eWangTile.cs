using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cut2EWangTile : VariationModel
    {
        private double _hashScale1;
        private double _invert;
        private double _k;
        private double _k2;
        private double _k3;
        private double _mode;
        private Random _randomize;

        private int _seed;
        private double _x0;
        private double _y0;
        private double _zoom;


        public Cut2EWangTile()
        {
            SetParameters(new[] {0.0, 1.0, -4.0, 0.0}, new[] {"seed", "mode", "zoom", "invert"});
        }

        public override int Id { get; } = 100;
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

            var plus = new vec2(n2 * _zoom, n3 * _zoom).plus(new vec2(_x0, _y0));
            G.normalize(new vec3(plus, 1.66));
            var vec2 = new vec2(0.01, 0.0);
            var map = Map(plus);
            Height(plus);
            Height(plus.plus(new vec2(vec2.x, vec2.y)));
            Height(plus.plus(new vec2(vec2.y, vec2.x)));
            var sqrt = G.sqrt(G.Mix(1.0, 0.0,
                G.smoothstep(0.0, 0.001, Math.Min(map, Math.Abs(map - 0.1) - 0.1) / _zoom)));
            // xyzPoint2.doHide = false;
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (sqrt > 0.3)
                {
                    n2 = 0.0;
                    n3 = 0.0;
                    // xyzPoint2.doHide = true;
                }
            }
            else if (sqrt <= 0.3)
            {
                n2 = 0.0;
                n3 = 0.0;
                // xyzPoint2.doHide = true;
            }

            var x = W * (n2 - n4);
            var y = W * (n3 - n5);


            return new Point(x, y);
        }

        public override void Init()
        {
            _seed = (int) VariationHelper.Map(P1, -10, 10, int.MinValue, int.MaxValue);
            _mode = P2;
            _zoom = VariationHelper.Map(P3, -10, 10, -5, 100);

            _invert = P4;
            _randomize = new Random(_seed);
            _x0 = 0.0;
            _y0 = 0.0;
            _k = 0.1;
            _k2 = (1.0 - _k) / 2.0;
            _k3 = Math.Sqrt(2.0) * 0.5 - _k2;
            _hashScale1 = 0.1031;

            _x0 = _seed * _randomize.NextDouble();
            _y0 = _seed * _randomize.NextDouble();
        }

        private double Tile0(vec2 vec2)
        {
            return G.Mix(G.Length(vec2) - _k3, _k2 - G.Length(new vec2(Math.Abs(vec2.x) - 0.5, vec2.y - 0.5)),
                G.step(Math.Abs(vec2.x), vec2.y));
        }

        private double Tile1(vec2 vec2)
        {
            return Math.Abs(G.Length(vec2.minus(0.5)) - 0.5) - _k * 0.5;
        }

        private double Tile2(vec2 vec2)
        {
            return Math.Abs(vec2.x) - _k * 0.5;
        }

        private double Tile3(vec2 vec2)
        {
            return Math.Max(-vec2.x - _k * 0.5, _k2 - G.Length(new vec2(vec2.x - 0.5, Math.Abs(vec2.y) - 0.5)));
        }

        private double Tile4(vec2 vec2)
        {
            return _k2 - G.Length(new vec2(Math.Abs(vec2.x) - 0.5, Math.Abs(vec2.y) - 0.5));
        }

        private double Tile(vec2 vec2, int n)
        {
            switch (n)
            {
                case 0:
                {
                    return 1.414;
                }
                case 1:
                {
                    return Math.Max(Tile0(vec2), 0.15 - G.Length(vec2));
                }
                case 2:
                {
                    return Tile0(new vec2(vec2.y, vec2.x));
                }
                case 3:
                {
                    return Tile1(vec2);
                }
                case 4:
                {
                    return Tile0(new vec2(vec2.x, -vec2.y));
                }
                case 5:
                {
                    return Tile2(vec2);
                }
                case 6:
                {
                    return Tile1(new vec2(vec2.x, -vec2.y));
                }
                case 7:
                {
                    return Tile3(vec2);
                }
                case 8:
                {
                    return Tile0(new vec2(vec2.y, -vec2.x));
                }
                case 9:
                {
                    return Tile1(new vec2(-vec2.x, vec2.y));
                }
                case 10:
                {
                    return Tile2(new vec2(vec2.y, vec2.x));
                }
                case 11:
                {
                    return Tile3(new vec2(vec2.y, vec2.x));
                }
                case 12:
                {
                    return Tile1(new vec2(-vec2.x, -vec2.y));
                }
                case 13:
                {
                    return Tile3(new vec2(-vec2.x, vec2.y));
                }
                case 14:
                {
                    return Tile3(new vec2(-vec2.y, vec2.x));
                }
                case 15:
                {
                    return Tile4(vec2);
                }
                default:
                {
                    return 1.414;
                }
            }
        }

        private double Hash(vec2 vec2)
        {
            var fract = G.Fract(new vec3(vec2.x, vec2.y, vec2.x).multiply(_hashScale1));
            var plus = fract.plus(G.dot(fract, new vec3(fract.y, fract.z, fract.x).plus(19.19)));
            return G.Fract((plus.x + plus.y) * plus.z);
        }

        private double Map(vec2 plus)
        {
            var n = 0;
            plus = plus.plus(0.5);
            var floor = G.Floor(plus);
            if (Hash(floor) >= 0.5) ++n;
            if (Hash(floor.multiply(-1.0)) >= 0.5) n += 8;
            if (Hash(floor.minus(new vec2(0.0, 1.0))) >= 0.5) n += 4;
            if (Hash(floor.plus(new vec2(1.0, 0.0)).multiply(-1.0)) >= 0.5) n += 2;
            return Tile(G.Fract(plus).minus(0.5), n);
        }

        private vec2 Rotate(vec2 vec2, double n)
        {
            var cos = Math.Cos(n);
            var sin = Math.Sin(n);
            return vec2.times(new mat2(cos, sin, -sin, cos));
        }

        private double Height(vec2 vec2)
        {
            var n = Map(vec2) - 0.1;
            return Math.Sqrt(0.01 - Math.Min(n * n, 0.01));
        }

        public bool UnitSquare(double n)
        {
            return n >= 0.0 && n <= 1.0;
        }
    }
}