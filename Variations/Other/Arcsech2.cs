using System.Windows; using FlameBase.Models;
using FlameBase.FlameMath;

namespace Variations.Other
{
    public class Arcsech2 : VariationModel
    {
        public override int Id { get; } = 55;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var complex = new Complex(p);
            complex.Recip();
            var complex2 = new Complex(complex);
            complex2.Dec();
            complex2.Sqrt();
            var complex3 = new Complex(complex);
            complex3.Inc();
            complex3.Sqrt();
            complex3.Mul(complex2);
            complex.Add(complex3);
            complex.Log();
            complex.Scale(W * VariationHelper.OneRadOfQuadrant); //2.0 * 0.3183098861837907);
            var point = new Point();

            point.Y += complex.Im;
            if (complex.Im < 0.0)
            {
                point.X += complex.Re;
                ++point.Y;
            }
            else
            {
                point.X -= complex.Re;
                --point.Y;
            }

            return point;
        }
    }
}