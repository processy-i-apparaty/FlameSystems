using System;
using System.Windows; using FlameBase.Models;

namespace Variations
{
    public class Rectangles : VariationModel
    {
        public Rectangles()
        {
            SetParameters(new[] { 2.0, 2.0 }, new[] { "x", "y" });
        }

        public override int Id { get; } = 40;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = P1;
            var y = P2;
            var point = new Point();

            if (Math.Abs(x) < 1.0E-8)
                point.X += W * p.X;
            else
                point.X += W * ((2.0 * Math.Floor(p.X / x) + 1.0) * x - p.X);

            if (Math.Abs(y) < 1.0E-8)
                point.Y += W * p.Y;
            else
                point.Y += W * ((2.0 * Math.Floor(p.Y / y) + 1.0) * y - p.Y);

            return point;
        }
    }
}