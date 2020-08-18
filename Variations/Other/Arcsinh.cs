using System.Windows; using FlameBase.Models;
using FlameBase.FlameMath;

namespace Variations.Other
{
    public class Arcsinh : VariationModel
    {
        public override int Id { get; } = 56;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var complex = new Complex(p);
            complex.AsinH();
            complex.Scale(W * VariationHelper.OneRadOfQuadrant);
            return new Point(complex.Re, complex.Im);
        }
    }
}