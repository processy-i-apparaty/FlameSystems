using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Apocarpet : VariationModel
    {
        private readonly double _n4 = 1.0 / (1.0 + Math.Sqrt(2.0));
        public override int Id { get; } = 53;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = 0.0;
            var n3 = 0.0;
            var n5 = p.X * p.X + p.Y * p.Y;
            switch ((int) (6.0 * VariationHelper.Psi))
            {
                case 0:
                {
                    n2 = 2.0 * p.X * p.Y / n5;
                    n3 = (p.X * p.X - p.Y * p.Y) / n5;
                    break;
                }

                case 1:
                {
                    n2 = p.X * _n4 - _n4;
                    n3 = p.Y * _n4 - _n4;
                    break;
                }

                case 2:
                {
                    n2 = p.X * _n4 + _n4;
                    n3 = p.Y * _n4 + _n4;
                    break;
                }

                case 3:
                {
                    n2 = p.X * _n4 + _n4;
                    n3 = p.Y * _n4 - _n4;
                    break;
                }

                case 4:
                {
                    n2 = p.X * _n4 - _n4;
                    n3 = p.Y * _n4 + _n4;
                    break;
                }

                case 5:
                {
                    n2 = (p.X * p.X - p.Y * p.Y) / n5;
                    n3 = 2.0 * p.X * p.Y / n5;
                    break;
                }
            }

            return new Point(n2 * W, n3 * W);
        }

        public override void Init()
        {
        }
    }
}