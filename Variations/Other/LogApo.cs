using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class LogApo : VariationModel
    {
        private double _denom;

        public LogApo()
        {
            SetParameters(new[] {2.71828182845905}, new[] {"base"});
        }

        public override int Id { get; } = 148;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = W * Math.Log(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y)) * _denom;
            var y = W * Math.Atan2(p.Y, p.X);


            return new Point(x, y);
        }

        public override void Init()
        {
            _denom = 0.5 / Math.Log(P1);
        }
    }
}