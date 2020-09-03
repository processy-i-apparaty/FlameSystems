using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Polar : VariationModel
    {
        private double _w;
        public override int Id { get; } = 5;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            return new Point(Math.Atan2(p.X, p.Y) * _w, (VariationHelper.R(p) - 1.0) * W);
//            return new Point(
//                VariationHelper.Theta(p) / Math.PI,
//                VariationHelper.R(p) - 1.0);
        }

        public override void Init()
        {
            _w = 0.31830989 * W;
        }
    }
}