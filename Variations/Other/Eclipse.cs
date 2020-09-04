using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Eclipse : VariationModel
    {
        private double _shift;

        public Eclipse()
        {
            SetParameters(new[] {0.0}, new[] {"shift"});
        }

        public override int Id { get; } = 117;
        public override int HasParameters { get; } = 1;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            if (Math.Abs(p.Y) <= W)
            {
                var sqrt = Math.Sqrt(VariationHelper.Sqr(W) - VariationHelper.Sqr(p.Y));
                if (Math.Abs(p.X) <= sqrt)
                {
                    var n2 = p.X + _shift * W;
                    if (Math.Abs(n2) >= sqrt)
                        point.X -= W * p.X;
                    else
                        point.X += W * n2;
                }
                else
                {
                    point.X += W * p.X;
                }

                point.Y += W * p.Y;
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
            _shift = VariationHelper.Map(P1, -10, 10, -2.0, 2.0);
        }
    }
}