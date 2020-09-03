using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Cell : VariationModel
    {
        private double _n2;

        public Cell()
        {
            SetParameters(new[] {0.6}, new[] {"size"});
        }

        public override int Id { get; } = 75;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n3 = (int) Math.Floor(p.X * _n2);
            var n4 = (int) Math.Floor(p.Y * _n2);
            var n5 = p.X - n3 * P1;
            var n6 = p.Y - n4 * P1;
            int n7;
            int n8;
            if (n4 >= 0)
            {
                if (n3 >= 0)
                {
                    n7 = n4 * 2;
                    n8 = n3 * 2;
                }
                else
                {
                    n7 = n4 * 2;
                    n8 = -(2 * n3 + 1);
                }
            }
            else if (n3 >= 0)
            {
                n7 = -(2 * n4 + 1);
                n8 = n3 * 2;
            }
            else
            {
                n7 = -(2 * n4 + 1);
                n8 = -(2 * n3 + 1);
            }

            var x = W * (n5 + n8 * P1);
            var y = -W * (n6 + n7 * P1);

            return new Point(x, y);
        }

        public override void Init()
        {
            _n2 = 1.0 / P1;
        }
    }
}