using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class Arctanh : VariationModel
    {
        private double _scale;
        public override int Id { get; } = 57;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var complex = new Complex(p);
            var complex2 = new Complex(complex);
            var complex3 = new Complex(complex);

            complex2.Scale(-1.0);
            complex2.Inc();

            complex3.Inc();
            complex3.Div(complex2);
            complex3.Log();
            complex3.Scale(_scale);
            return new Point(complex3.Re, complex3.Im);
        }

        public override void Init()
        {
            _scale = W * VariationHelper.OneRadOfQuadrant;
        }
    }
}