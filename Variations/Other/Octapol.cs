using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Octapol : VariationModel
    {
/*
        private const double DenomSqrt2 = 0.707106781;
*/
        private double _a;
        private double _ax;
        private double _ay;
        private double _bx;
        private double _by;
        private double _cx;
        private double _cy;

        private Double2 _double2A;
        private Double2 _double2B;
        private Double2 _double2C;
        private Double2 _double2D;
        private Double2 _double2E;
        private Double2 _double2F;
        private Double2 _double2G;
        private Double2 _double2H;
        private Double2 _double2I;
        private Double2 _double2J;
        private Double2 _double2K;
        private Double2 _double2L;
        private double _dx;
        private double _dy;
        private double _ex;
        private double _ey;
        private double _fx;
        private double _fy;
        private double _gx;
        private double _gy;
        private double _hx;
        private double _hy;
        private double _ix;
        private double _iy;
        private double _jx;
        private double _jy;
        private double _kx;
        private double _ky;
        private double _lx;
        private double _ly;

        private double _polarWeight;
        private MutableDouble _r;
        private double _rad;
        private double _radius;
        private double _s;
        private double _t;
        private MutableDouble _u;
        private MutableDouble _v;
        private Double2 _v0;
        private Double2 _v1;
        private Double2 _v2;

        private Double2 _xy;

        public Octapol()
        {
            SetParameters(new[] {0.0, 1.0, 0.5, .5}, new[] {"polar weight", "radius", "s", "t"});
        }

        public override int Id { get; } = 159;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();

            var n2 = p.X * 0.15;
            var n3 = p.Y * 0.15;
            // double z = xyzPoint.z;
            var r = _r;
            var u = _u;
            var v = _v;
            const double value = 0.0;
            v.Value = value;
            u.Value = value;
            r.Value = value;
            _xy.SetXy(n2, n3);
            if (_rad > 0.0 && HitsCircleAroundOrigin(_rad, _xy, _r))
            {
                var log = Math.Log(VariationHelper.Sqr(_r.Value / _rad));
                point.X += W * Lerp(n2, Math.Atan2(n3, n2), log * _polarWeight);
                point.Y += W * Lerp(n3, _r.Value, log * _polarWeight);
            }
            else if (HitsSquareAroundOrigin(_a, _xy))
            {
                if (HitsRect(_double2H, _double2K, _xy) || HitsRect(_double2J, _double2D, _xy) ||
                    HitsRect(_double2A, _double2J, _xy) ||
                    HitsRect(_double2K, _double2E, _xy) || HitsTriangle(_double2I, _double2A, _double2H, _xy, _u, _v) ||
                    HitsTriangle(_double2J, _double2B, _double2C, _xy, _u, _v) ||
                    HitsTriangle(_double2L, _double2D, _double2E, _xy, _u, _v) ||
                    HitsTriangle(_double2K, _double2F, _double2G, _xy, _u, _v))
                {
                    point.X += W * n2;
                    point.Y += W * n3;
                }
                else
                {
                    const double n4 = 0.0;
                    point.Y = n4;
                    point.X = n4;
                }
            }
            else
            {
                const double n5 = 0.0;
                point.Y = n5;
                point.X = n5;
            }

            point.X += W * n2;
            point.Y += W * n3;

            return point;
        }

        public override void Init()
        {
            _xy = new Double2();
            _r = new MutableDouble();
            _u = new MutableDouble();
            _v = new MutableDouble();
            _double2A = new Double2();
            _double2B = new Double2();
            _double2C = new Double2();
            _double2D = new Double2();
            _double2E = new Double2();
            _double2F = new Double2();
            _double2G = new Double2();
            _double2H = new Double2();
            _double2I = new Double2();
            _double2J = new Double2();
            _double2K = new Double2();
            _double2L = new Double2();
            _v0 = new Double2();
            _v1 = new Double2();
            _v2 = new Double2();

            _polarWeight = P1;
            _radius = P2;
            _s = P3;
            _t = P4;


            _a = _s * 0.5 + _t;
            _rad = 0.707106781 * _s * Math.Abs(_radius);
            _ax = -0.5 * _s;
            _ay = 0.5 * _s + _t;
            _bx = 0.5 * _s;
            _by = 0.5 * _s + _t;
            _cx = _t;
            _cy = 0.5 * _s;
            _dx = _t;
            _dy = -0.5 * _s;
            _ex = 0.5 * _s;
            _ey = -0.5 * _s - _t;
            _fx = -0.5 * _s;
            _fy = -0.5 * _s - _t;
            _gx = -_t;
            _gy = -0.5 * _s;
            _hx = -_t;
            _hy = 0.5 * _s;
            _ix = -0.5 * _s;
            _iy = 0.5 * _s;
            _jx = 0.5 * _s;
            _jy = 0.5 * _s;
            _kx = -0.5 * _s;
            _ky = -0.5 * _s;
            _lx = 0.5 * _s;
            _ly = -0.5 * _s;
            _double2A.SetXy(_ax, _ay);
            _double2B.SetXy(_bx, _by);
            _double2C.SetXy(_cx, _cy);
            _double2D.SetXy(_dx, _dy);
            _double2E.SetXy(_ex, _ey);
            _double2F.SetXy(_fx, _fy);
            _double2G.SetXy(_gx, _gy);
            _double2H.SetXy(_hx, _hy);
            _double2I.SetXy(_ix, _iy);
            _double2J.SetXy(_jx, _jy);
            _double2K.SetXy(_kx, _ky);
            _double2L.SetXy(_lx, _ly);
        }

        private static double Dot(Double2 double2, Double2 double3)
        {
            return double2.X * double3.X + double2.Y * double3.Y;
        }

        private static double Lerp(double n, double n2, double n3)
        {
            return n + n3 * (n2 - n);
        }

        private static bool HitsRect(Double2 double2, Double2 double3, Double2 double4)
        {
            return double4.X >= double2.X && double4.Y >= double2.Y && double4.X <= double3.X && double4.Y <= double3.Y;
        }

        private bool HitsTriangle(Double2 double2, Double2 double3, Double2 double4, Double2 double5,
            MutableDouble mutableDouble, MutableDouble mutableDouble2)
        {
            _v0.SetXy(double4.X - double2.X, double4.Y - double2.Y);
            _v1.SetXy(double3.X - double2.X, double3.Y - double2.Y);
            _v2.SetXy(double5.X - double2.X, double5.Y - double2.Y);
            var dot = Dot(_v0, _v0);
            var dot2 = Dot(_v0, _v1);
            var dot3 = Dot(_v0, _v2);
            var dot4 = Dot(_v1, _v1);
            var dot5 = Dot(_v1, _v2);
            var n = dot * dot4 - dot2 * dot2;
            if (n != 0.0)
            {
                mutableDouble.Value = (dot4 * dot3 - dot2 * dot5) / n;
                mutableDouble2.Value = (dot * dot5 - dot2 * dot3) / n;
            }
            else
            {
                var n2 = 0.0;
                mutableDouble2.Value = n2;
                mutableDouble.Value = n2;
            }

            return mutableDouble.Value + mutableDouble2.Value < 1.0 && mutableDouble.Value > 0.0 &&
                   mutableDouble2.Value > 0.0;
        }

        private static bool HitsSquareAroundOrigin(double n, Double2 double2)
        {
            return Math.Abs(double2.X) <= n && Math.Abs(double2.Y) <= n;
        }

        private bool HitsCircleAroundOrigin(double n, Double2 double2, MutableDouble mutableDouble)
        {
            if (Math.Abs(n) <= VariationHelper.SmallDouble) return true;

            mutableDouble.Value = Math.Sqrt(VariationHelper.Sqr(double2.X) + VariationHelper.Sqr(double2.Y));
            return mutableDouble.Value <= n;
        }
    }

    internal class MutableDouble
    {
        public double Value { get; set; }
    }

    internal class Double2
    {
        public Double2()
        {
        }

        public Double2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public void SetXy(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}