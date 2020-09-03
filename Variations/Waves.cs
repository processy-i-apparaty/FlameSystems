using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Waves : VariationModel
    {
        private double _getXyCoeff10;
        private double _getXyCoeff11;
        private double _getXyCoeff20;
        private double _getXyCoeff21;
        public override int Id { get; } = 15;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = true;

        public override Point Fun(Point p)
        {
            //            return new Point(
            //                p.X + B * Math.Sin(p.Y / (C*C)),
            //                p.Y + E * Math.Sin(p.X / (F*F)));
            //            var minE = 1.0E-100;
            //
            //            var coef10 = B;
            //            var coef11 = D;//E;//D;
            //            var coef20 = E;//C;//E;
            //            var coef21 = F;
            //
            //            var xx = p.Y / (coef20 * coef20 + minE);
            //            var yy = p.X / (coef21 * coef21 + minE);
            //            if (double.IsInfinity(xx))
            //                xx = 0;
            //            if (double.IsInfinity(yy))
            //                yy = 0;
            //
            //            var xSin = Math.Sin(xx);
            //            var ySin = Math.Sin(yy);
            //
            //            var n1 = coef10 * xSin;
            //            var n2 = coef11 * ySin;
            //            var x = W * (p.X + n1);
            //            var y = W * (p.Y + n2);
            //            return new Point(
            //                x, y
            //            );

            //            xyzPoint2.x += n * (xyzPoint.x + xForm.getXYCoeff10() * MathLib.sin(xyzPoint.y / (xForm.getXYCoeff20() * xForm.getXYCoeff20() + 1.0E-300)));
            //            xyzPoint2.y += n * (xyzPoint.y + xForm.getXYCoeff11() * MathLib.sin(xyzPoint.x / (xForm.getXYCoeff21() * xForm.getXYCoeff21() + 1.0E-300)));


            var x = W * (p.X + _getXyCoeff10 *
                Math.Sin(p.Y / (_getXyCoeff20 * _getXyCoeff20 + VariationHelper.SmallDouble)));
            var y = W * (p.Y + _getXyCoeff11 *
                Math.Sin(p.X / (_getXyCoeff21 * _getXyCoeff21 + VariationHelper.SmallDouble)));
            return new Point(x, y);
        }

        public override void Init()
        {
            _getXyCoeff10 = B;
            _getXyCoeff20 = E;
            _getXyCoeff11 = D;
            _getXyCoeff21 = F;
        }
    }
}