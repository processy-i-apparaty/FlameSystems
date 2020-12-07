using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutApollonian : VariationModel
    {
        public CutApollonian()
        {
            SetParameters(new[] {1.0, 4.0, 2.0, 0.0}, new[] {"mode", "levels", "zoom", "invert"});
        }

        public override int Id { get; } = 101;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double x;
            double y;
            if (Math.Abs(P1) <= VariationHelper.SmallDouble)
            {
                x = p.X;
                y = p.Y;
            }
            else
            {
                x = VariationHelper.Psi - 0.5;
                y = VariationHelper.Psi - 0.5;
            }

            // var vec2 = new vec2(x, y);
            var n2 = 0.0 + Apollonian(new Vec2(x * P3, y * P3));

            if (Math.Abs(P4) <= VariationHelper.SmallDouble)
            {
                if (n2 > 0.0)
                {
                    x = 0.0;
                    y = 0.0;
                    return p;
                }
            }
            else if (n2 <= 0.0)
            {
                x = 0.0;
                y = 0.0;
                return p;
            }

            var dx = W * x;
            var dy = W * y;


            return new Point(dx, dy);
        }

        public override void Init()
        {
        }

        private double Apollonian(Vec2 vec2)
        {
            var n = 1.0;
            var min = 1.0E20;
            var min2 = 1.0E20;
            for (var i = 0; i < P2; ++i)
            {
                vec2 = G.Fract(vec2.Multiply(0.5).Plus(0.5)).Multiply(2.0).Minus(1.0);
                var n2 = 1.34 / G.Dot(vec2, vec2);
                vec2 = vec2.Multiply(n2);
                min = G.Min(min, G.Dot(vec2, vec2));
                min2 = G.Min(min2, G.Max(G.Abs(vec2.X), G.Abs(vec2.Y)));
                n *= n2;
            }

            return G.Smoothstep(0.001, 0.002, 0.25 * G.Abs(vec2.Y) / n);
        }
    }
}