using System.Windows;
using FlameBase.FlameMath;

using FlameBase.Models;

namespace Variations.Other
{
    public class Acosech : VariationModel
    {
        public override int Id { get; } = 49;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var complex = new Complex(p);
            complex.AcosecH();
            complex.Flip();
            complex.Scale(W * VariationHelper.OneRadOfQuadrant);
            return VariationHelper.Psi < 0.5 ? new Point(complex.Re, complex.Im) : new Point(-complex.Re, -complex.Im);
        }
    }
}