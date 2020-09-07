using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Loonie3 : VariationModel
    {
        private double _sqrVVar;

        public override int Id { get; } = 153;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var sqr = 2.0 * _sqrVVar;
            if (p.X > 1.0E-300) sqr = VariationHelper.Sqr((VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y)) / p.X);

            if (sqr < _sqrVVar)
            {
                var n2 = W * Math.Sqrt(_sqrVVar / sqr - 1.0);
                point.X += n2 * p.X;
                point.Y += n2 * p.Y;
            }
            else
            {
                point.X += W * p.X;
                point.Y += W * p.Y;
            }


            return point;
        }

        public override void Init()
        {
            _sqrVVar = W * W;
        }
    }
}