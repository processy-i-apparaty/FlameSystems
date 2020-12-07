using System;
using System.Windows;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace Variations.Other
{
    public class Joukowski : VariationModel
    {
        private int _invert;
        private Vec2 _z0;

        public Joukowski()
        {
            SetParameters(new[] {2.5, 0.0, 0.0}, new[] {"P1", "P2", "inverse"});
        }

        public override int Id { get; } = 141;
        public override int HasParameters { get; } = 3;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var vec2 = new Vec2(p.X, p.Y);
            var vec3 = _invert == 0 ? Jouk(vec2) : InvJouk(vec2);

            var x = W * vec3.X;
            var y = W * vec3.Y;

            return new Point(x, y);
        }

        public override void Init()
        {
            _invert = (int) P3;
            _z0 = new Vec2(-P2, 0.2);
        }

        private static Vec2 CExp(double n)
        {
            return new Vec2(Math.Cos(n), Math.Sin(n));
        }

        private static Vec2 CMul(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(vec2.X * vec3.X - vec2.Y * vec3.Y, vec2.X * vec3.Y + vec2.Y * vec3.X);
        }

        private static Vec2 CDiv(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(G.Dot(vec2, vec3), vec2.Y * vec3.X - vec2.X * vec3.Y).Division(G.Dot(vec3, vec3));
        }

        private static Vec2 CSqrt(Vec2 vec2)
        {
            var length = G.Length(vec2);
            return G.Sqrt(new Vec2(length + vec2.X, length - vec2.X).Multiply(0.5))
                .Multiply(new Vec2(1.0, G.Sign(vec2.Y)));
        }

        private double CNorm(Vec2 vec2)
        {
            return G.Length(vec2);
        }

        private Vec2 Jouk(Vec2 vec2)
        {
            return CMul(CExp(-0.0),
                vec2.Plus(_z0).Plus(CDiv(new Vec2(P1 * P1, 0.0), vec2.Plus(_z0))));
        }

        private Vec2 InvJouk(Vec2 cMul)
        {
            cMul = CMul(CExp(0.0), cMul);
            var minus = cMul.Division(2.0)
                .Plus(CSqrt(CMul(cMul, cMul).Division(4.0).Minus(new Vec2(P1 * P1, 0.0))))
                .Minus(_z0);
            var minus2 = cMul.Division(2.0)
                .Minus(CSqrt(CMul(cMul, cMul).Division(4.0).Minus(new Vec2(P1 * P1, 0.0))))
                .Minus(_z0);
            return CNorm(minus) > CNorm(minus2) ? minus : minus2;
        }
    }
}