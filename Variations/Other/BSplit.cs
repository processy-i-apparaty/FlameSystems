using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class BSplit : VariationModel
    {
        public BSplit()
        {
            SetParameters(new[] {0.0, 0.0}, new[] {"x", "y"});
        }

        public override int Id { get; } = 71;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            // if (Math.Abs(p.X + P1) <= VariationHelper.SmallDouble ||
            //     Math.Abs(p.X + P2 - Math.PI) <= VariationHelper.SmallDouble)
            // {
            //     //xyzPoint2.doHide = true;
            // }
            // else
            // {
            //     //xyzPoint2.doHide = false;
            //     point.X += W / Math.Tan(p.X + P1) * Math.Cos(p.Y + P2);
            //     point.Y += W / Math.Sin(p.X + P1) * (-1.0 * p.Y + P2);
            // }

            var x = W / Math.Tan(p.X + P1) * Math.Cos(p.Y + P2);
            var y = W / Math.Sin(p.X + P1) * (-1.0 * p.Y + P2);
            return new Point(x, y);
        }

        public override void Init()
        {
           
        }
    }
}