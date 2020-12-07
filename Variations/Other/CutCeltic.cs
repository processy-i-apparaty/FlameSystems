using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class CutCeltic : VariationModel
    {
        private double _invert;

        private double _mode;
        private double _zoom;

        public CutCeltic()
        {
            SetParameters(new[] {1.0, 5.0, 0.0}, new[] {"mode", "zoom", "invert"});
        }

        public override int Id { get; } = 102;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            double n2;
            double n3;
            double n4;
            double n5;
            if (Math.Abs(_mode) <= VariationHelper.SmallDouble)
            {
                n2 = p.X;
                n3 = p.Y;
                n4 = 0.0;
                n5 = 0.0;
            }
            else
            {
                n2 = VariationHelper.Psi;
                n3 = VariationHelper.Psi;
                n4 = 0.5;
                n5 = 0.5;
            }

            var multiply = new Vec2(n2 * 1.0, n3 * 1.155).Multiply(_zoom);
            var mod = G.Mod(multiply.Y, 2.0);
            var vec2 = multiply;
            vec2.X += G.Step(1.0, mod) * 0.5;
            Vec2 fract;
            var vec3 = fract = G.Fract(multiply);
            fract.Y /= 1.155;
            var vec4 = vec3;
            vec4.X -= 0.5775;
            var vec5 = vec3;
            vec5.Y -= 0.5;
            Vec2 multiply2;
            var vec6 = multiply2 = new Vec2(n2 * 1.0, n3 * 1.155).Multiply(_zoom);
            multiply2.X -= 0.5;
            var vec7 = vec6;
            vec7.Y -= 0.288675;
            var mod2 = G.Mod(vec6.Y, 2.0);
            var vec8 = vec6;
            vec8.X += G.Step(1.0, mod2) * 0.5;
            Vec2 fract2;
            var vec9 = fract2 = G.Fract(vec6);
            fract2.Y /= 1.155;
            var vec10 = vec9;
            vec10.X -= 0.5775;
            var vec11 = vec9;
            vec11.Y -= 0.5;
            Vec2 multiply3;
            var vec12 = multiply3 = new Vec2(n2 * 1.0, n3 * 1.155).Multiply(_zoom);
            ++multiply3.X;
            var vec13 = vec12;
            vec13.Y -= 0.65;
            var mod3 = G.Mod(vec12.Y, 2.0);
            var vec14 = vec12;
            vec14.X += G.Step(1.0, mod3) * 0.5;
            Vec2 fract3;
            var vec15 = fract3 = G.Fract(vec12);
            fract3.Y /= 1.155;
            var vec16 = vec15;
            vec16.X -= 0.5775;
            var vec17 = vec15;
            vec17.Y -= 0.5;
            var n6 = CelticShit(vec3) + CelticShit(vec9) + CelticShit(vec15);
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (n6 > 0.0)
                {
                    n2 = 0.0;
                    n3 = 0.0;
                    return p;
                }
            }
            else if (n6 <= 0.0)
            {
                n2 = 0.0;
                n3 = 0.0;
                return p;
            }

            var x = W * (n2 - n4);
            var y = W * (n3 - n5);

            return new Point(x, y);
        }

        public override void Init()
        {
            _mode = P1;
            _zoom = P2;
            _invert = P3;
        }

        private static double CelticShit(Vec2 vec2)
        {
            const double n2 = 0.38;
            const double n3 = 0.45;
            return G.Mix(0.0,
                Circ(vec2, n3) - Circ(vec2.Plus(new Vec2(0.5, -0.288675)), n3) -
                Circ(vec2.Plus(new Vec2(-0.5, -0.288675)), n3) - Circ(vec2.Plus(new Vec2(0.0, 0.57735)), n3),
                Circ(vec2, n2));
        }

        private static double Circ(Vec2 vec2, double n)
        {
            var length = G.Length(vec2);
            return G.Smoothstep(length, length + 0.02, n);
        }
    }
}