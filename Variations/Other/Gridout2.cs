using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Gridout2 : VariationModel
    {
        private double _a;
        private double _b;
        private double _c;
        private double _d;

        public Gridout2()
        {
            SetParameters(new[] {1.0, 1.0, 1.0, 1.0}, new[] {"a", "b", "c", "d"});
        }

        public override int Id { get; } = 130;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var n2 = VariationHelper.Rint(p.X) * _c;
            var n3 = VariationHelper.Rint(p.Y) * _d;
            if (n3 <= 0.0)
            {
                if (n2 > 0.0)
                {
                    if (-n3 >= n2)
                    {
                        point.X += W * (p.X + _a);
                        point.Y += W * p.Y;
                    }
                    else
                    {
                        point.X += W * p.X;
                        point.Y += W * (p.Y + _b);
                    }
                }
                else if (n3 <= n2)
                {
                    point.X += W * (p.X + _a);
                    point.Y += W * p.Y;
                }
                else
                {
                    point.X += W * p.X;
                    point.Y += W * (p.Y - _b);
                }
            }
            else if (n2 > 0.0)
            {
                if (n3 >= n2)
                {
                    point.X += W * (p.X - _a);
                    point.Y += W * p.Y;
                }
                else
                {
                    point.X += W * p.X;
                    point.Y += W * (p.Y + _b);
                }
            }
            else if (n3 > -n2)
            {
                point.X += W * (p.X - _a);
                point.Y += W * p.Y;
            }
            else
            {
                point.X += W * p.X;
                point.Y += W * (p.Y - _b);
            }

            return point;
        }

        public override void Init()
        {
            _a = P1;
            _b = P2;
            _c = P3;
            _d = P4;
        }
    }
}