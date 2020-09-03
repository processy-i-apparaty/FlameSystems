using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Boarders : VariationModel
    {
        public override int Id { get; } = 69;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var rint = VariationHelper.Rint(p.X);
            var rint2 = VariationHelper.Rint(p.Y);
            var n2 = p.X - rint;
            var n3 = p.Y - rint2;

            var point = new Point();
            if (VariationHelper.Psi >= 0.75)
            {
                point.X += W * (n2 * 0.5 + rint);
                point.Y += W * (n3 * 0.5 + rint2);
            }
            else if (Math.Abs(n2) >= Math.Abs(n3))
            {
                if (n2 >= 0.0)
                {
                    point.X += W * (n2 * 0.5 + rint + 0.25);
                    point.Y += W * (n3 * 0.5 + rint2 + 0.25 * n3 / n2);
                }
                else
                {
                    point.X += W * (n2 * 0.5 + rint - 0.25);
                    point.Y += W * (n3 * 0.5 + rint2 - 0.25 * n3 / n2);
                }
            }
            else if (n3 >= 0.0)
            {
                point.Y += W * (n3 * 0.5 + rint2 + 0.25);
                point.X += W * (n2 * 0.5 + rint + n2 / n3 * 0.25);
            }
            else
            {
                point.Y += W * (n3 * 0.5 + rint2 - 0.25);
                point.X += W * (n2 * 0.5 + rint - n2 / n3 * 0.25);
            }

            return point;
        }

        public override void Init()
        {
        }
    }
}