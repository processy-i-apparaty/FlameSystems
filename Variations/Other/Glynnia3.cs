using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Glynnia3 : VariationModel
    {
        private double _dScale;
        private double _rScale;
        private double _rThresh;
        private double _var2;
        private double _yThresh;

        public Glynnia3()
        {
            SetParameters(new[] {1.0, 1.0, 0.0, 0.0}, new[] {"rscale", "dscale", "rthresh", "ythresh"});
        }

        public override int Id { get; } = 129;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();
            var n2 = _rScale * Math.Sqrt(VariationHelper.Sqr(p.X) + VariationHelper.Sqr(p.Y));
            if (n2 > _rThresh && p.Y > _yThresh)
            {
                if (VariationHelper.Psi > 0.5)
                {
                    var n3 = _dScale * Math.Sqrt(n2 + p.X);
                    if (Math.Abs(n3) <= VariationHelper.SmallDouble) return p;

                    point.X += _var2 * n3;
                    point.Y -= _var2 / n3 * p.Y;
                }
                else
                {
                    var n4 = _dScale * (n2 + p.X);
                    var sqrt = Math.Sqrt(n2 * (VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(n4)));
                    if (Math.Abs(sqrt) <= VariationHelper.SmallDouble) return p;

                    var n5 = W / sqrt;
                    point.X += n5 * n4;
                    point.Y += n5 * p.Y;
                }
            }
            else if (VariationHelper.Psi > 0.5)
            {
                var n6 = _dScale * Math.Sqrt(n2 + p.X);
                if (Math.Abs(n6) <= VariationHelper.SmallDouble) return p;

                point.X -= _var2 * n6;
                point.Y -= _var2 / n6 * p.Y;
            }
            else
            {
                var n7 = _dScale * (n2 + p.X);
                var sqrt2 = Math.Sqrt(n2 * (VariationHelper.Sqr(p.Y) + VariationHelper.Sqr(n7)));
                if (Math.Abs(sqrt2) <= VariationHelper.SmallDouble) return p;

                var n8 = W / sqrt2;
                point.X -= n8 * n7;
                point.Y += n8 * p.Y;
            }

            return point;
        }

        public override void Init()
        {
            _rScale = P1;
            _dScale = P2;
            _rThresh = P3;
            _yThresh = P4;
            _var2 = W * Math.Sqrt(2.0) / 2.0;
        }
    }
}