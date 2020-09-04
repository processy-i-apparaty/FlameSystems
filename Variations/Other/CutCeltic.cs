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

            var multiply = new vec2(n2 * 1.0, n3 * 1.155).multiply(_zoom);
            var mod = G.mod(multiply.y, 2.0);
            var vec2 = multiply;
            vec2.x += G.step(1.0, mod) * 0.5;
            vec2 fract;
            var vec3 = fract = G.Fract(multiply);
            fract.y /= 1.155;
            var vec4 = vec3;
            vec4.x -= 0.5775;
            var vec5 = vec3;
            vec5.y -= 0.5;
            vec2 multiply2;
            var vec6 = multiply2 = new vec2(n2 * 1.0, n3 * 1.155).multiply(_zoom);
            multiply2.x -= 0.5;
            var vec7 = vec6;
            vec7.y -= 0.288675;
            var mod2 = G.mod(vec6.y, 2.0);
            var vec8 = vec6;
            vec8.x += G.step(1.0, mod2) * 0.5;
            vec2 fract2;
            var vec9 = fract2 = G.Fract(vec6);
            fract2.y /= 1.155;
            var vec10 = vec9;
            vec10.x -= 0.5775;
            var vec11 = vec9;
            vec11.y -= 0.5;
            vec2 multiply3;
            var vec12 = multiply3 = new vec2(n2 * 1.0, n3 * 1.155).multiply(_zoom);
            ++multiply3.x;
            var vec13 = vec12;
            vec13.y -= 0.65;
            var mod3 = G.mod(vec12.y, 2.0);
            var vec14 = vec12;
            vec14.x += G.step(1.0, mod3) * 0.5;
            vec2 fract3;
            var vec15 = fract3 = G.Fract(vec12);
            fract3.y /= 1.155;
            var vec16 = vec15;
            vec16.x -= 0.5775;
            var vec17 = vec15;
            vec17.y -= 0.5;
            var n6 = CelticShit(vec3) + CelticShit(vec9) + CelticShit(vec15);
            if (Math.Abs(_invert) <= VariationHelper.SmallDouble)
            {
                if (n6 > 0.0)
                {
                    n2 = 0.0;
                    n3 = 0.0;
                }
            }
            else if (n6 <= 0.0)
            {
                n2 = 0.0;
                n3 = 0.0;
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

        private static double CelticShit(vec2 vec2)
        {
            const double n2 = 0.38;
            const double n3 = 0.45;
            return G.Mix(0.0,
                Circ(vec2, n3) - Circ(vec2.plus(new vec2(0.5, -0.288675)), n3) -
                Circ(vec2.plus(new vec2(-0.5, -0.288675)), n3) - Circ(vec2.plus(new vec2(0.0, 0.57735)), n3),
                Circ(vec2, n2));
        }

        private static double Circ(vec2 vec2, double n)
        {
            var length = G.Length(vec2);
            return G.smoothstep(length, length + 0.02, n);
        }
    }
}