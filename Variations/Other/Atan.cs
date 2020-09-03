using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Atan : VariationModel
    {
        private double _n2;
        private double _round;

        public Atan()
        {
            SetParameters(new[] {0.0, 1.0}, new[] {"mode", "stretch"});
        }

        public override int Id { get; } = 59;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var point = new Point();

            switch ((int) Math.Round(_round))
            {
                case 0:
                {
                    point.X += W * p.X;
                    point.Y += _n2 * Math.Atan(P2 * p.Y);
                    break;
                }

                case 1:
                {
                    point.X += _n2 * Math.Atan(P2 * p.X);
                    point.Y += W * p.Y;
                    break;
                }

                case 2:
                {
                    point.X += _n2 * Math.Atan(P2 * p.X);
                    point.Y += _n2 * Math.Atan(P2 * p.Y);
                    break;
                }
            }

            return point;
        }

        public override void Init()
        {
            _round = VariationHelper.Map(Math.Abs(P1), 0.0, 10.0, 0.0, 2.0);
            _n2 = VariationHelper.OneRadOfQuadrant * W;
        }
    }
}