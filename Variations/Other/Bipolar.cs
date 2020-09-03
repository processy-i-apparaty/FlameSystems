using System;
using System.Windows; using FlameBase.Models;

namespace Variations.Other
{
    public class Bipolar : VariationModel
    {
        private double _wRadiant;
        private double _wRadiant25;

        public Bipolar()
        {
            SetParameters(new[] { 0.0 }, new[] { "shift" });
        }

        public override int Id { get; } = 63;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();

            var n2 = p.X * p.X + p.Y * p.Y;
            var n3 = n2 + 1.0;
            var n4 = 2.0 * p.X;
            var n5 = 0.5 * Math.Atan2(2.0 * p.Y, n2 - 1.0) + -VariationHelper.HalfPi * P1;
            if (n5 > VariationHelper.HalfPi)
                n5 = -VariationHelper.HalfPi + VariationHelper.Fmod(n5 + VariationHelper.HalfPi, Math.PI);
            else if (n5 < -VariationHelper.HalfPi)
                n5 = VariationHelper.HalfPi - VariationHelper.Fmod(VariationHelper.HalfPi - n5, Math.PI);
            var n6 = n3 + n4;
            var n7 = n3 - n4;
            if (Math.Abs(n7) < 1E-300 || n6 / n7 <= 0.0) return p;
            point.X += _wRadiant25 * Math.Log((n3 + n4) / (n3 - n4));
            point.Y += _wRadiant * n5;
            return point;
        }

        public override void Init()
        {
            _wRadiant = W * VariationHelper.OneRadOfQuadrant;
            _wRadiant25 = _wRadiant * .25;
        }
    }
}