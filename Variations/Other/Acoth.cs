using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class Acoth : VariationModel
    {
        public override int Id { get; } = 51;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var complex = new Complex(p);
            complex.AcotH();
            complex.Flip();
            complex.Scale(W * VariationHelper.OneRadOfQuadrant);
            return new Point(complex.Re, complex.Im);
        }

        public override void Init()
        {
        }
    }
}