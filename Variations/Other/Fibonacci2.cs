using System;
using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class Fibonacci2 : VariationModel
    {
        private double _fFive;
        private double _fNatLog;

        public Fibonacci2()
        {
            SetParameters(new[] {1.0, 1.0}, new[] {"sc", "sc2"});
        }

        public override int Id { get; } = 125;
        public override int HasParameters { get; } = 2;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            var n2 = p.Y * _fNatLog;
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = (p.X * Math.PI + p.Y * _fNatLog) * -1.0;
            var sin2 = Math.Sin(n3);
            var cos2 = Math.Cos(n3);
            var n4 = P1 * Math.Exp(P2 * (p.X * _fNatLog));
            var n5 = P1 * Math.Exp(P2 * ((p.X * _fNatLog - p.Y * Math.PI) * -1.0));
            var x = W * (n4 * cos - n5 * cos2) * _fFive;
            var y = W * (n4 * sin - n5 * sin2) * _fFive;


            return new Point(x, y);
        }

        public override void Init()
        {
            _fFive = 0.4472135954999579;
            _fNatLog = Math.Log(1.618033988749895);
        }
    }
}