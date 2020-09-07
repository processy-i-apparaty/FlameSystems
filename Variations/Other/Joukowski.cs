using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class Joukowski : VariationModel
    {
        private int _invert;
        private vec2 _z0;

        public Joukowski()
        {
            SetParameters(new[] {2.5, 0.0, 0.0}, new[] {"P1", "P2", "inverse"});
        }

        public override int Id { get; } = 141;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var vec2 = new vec2(p.X, p.Y);
            var vec3 = _invert == 0 ? Jouk(vec2) : InvJouk(vec2);

            var x = W * vec3.x;
            var y = W * vec3.y;

            return new Point(x, y);
        }

        public override void Init()
        {
            _invert = (int) P3;
            _z0 = new vec2(-P2, 0.2);
        }

        private static vec2 CExp(double n)
        {
            return new vec2(Math.Cos(n), Math.Sin(n));
        }

        private static vec2 CMul(vec2 vec2, vec2 vec3)
        {
            return new vec2(vec2.x * vec3.x - vec2.y * vec3.y, vec2.x * vec3.y + vec2.y * vec3.x);
        }

        private static vec2 CDiv(vec2 vec2, vec2 vec3)
        {
            return new vec2(G.dot(vec2, vec3), vec2.y * vec3.x - vec2.x * vec3.y).division(G.dot(vec3, vec3));
        }

        private static vec2 CSqrt(vec2 vec2)
        {
            var length = G.Length(vec2);
            return G.sqrt(new vec2(length + vec2.x, length - vec2.x).multiply(0.5))
                .multiply(new vec2(1.0, G.sign(vec2.y)));
        }

        private double CNorm(vec2 vec2)
        {
            return G.Length(vec2);
        }

        private vec2 Jouk(vec2 vec2)
        {
            return CMul(CExp(-0.0),
                vec2.plus(_z0).plus(CDiv(new vec2(P1 * P1, 0.0), vec2.plus(_z0))));
        }

        private vec2 InvJouk(vec2 cMul)
        {
            cMul = CMul(CExp(0.0), cMul);
            var minus = cMul.division(2.0)
                .plus(CSqrt(CMul(cMul, cMul).division(4.0).minus(new vec2(P1 * P1, 0.0))))
                .minus(_z0);
            var minus2 = cMul.division(2.0)
                .minus(CSqrt(CMul(cMul, cMul).division(4.0).minus(new vec2(P1 * P1, 0.0))))
                .minus(_z0);
            return CNorm(minus) > CNorm(minus2) ? minus : minus2;
        }
    }
}