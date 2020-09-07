using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class LogTile2 : VariationModel
    {
        public LogTile2()
        {
            SetParameters(new[] {2.0, 2.0}, new[] {"spreadX", "spreadY"});
        }

        public override int Id { get; } = 150;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var x = -P1;
            if (VariationHelper.Psi < 0.5) x = P1;

            var y = -P2;
            if (VariationHelper.Psi < 0.5) y = P2;

            var dx = W * (p.X + x * Math.Round(Math.Log(VariationHelper.Psi)));
            var dy = W * (p.Y + y * Math.Round(Math.Log(VariationHelper.Psi)));
            
            return new Point(dx, dy);
        }

        public override void Init()
        {
        }
    }
}