using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class MacMillan : VariationModel
    {
        private double _a;
        private double _b;
        private double _x;
        private double _xa;
        private double _y;

        public MacMillan()
        {
            SetParameters(new[] {1.6, 0.4, 0.1, 0.1}, new[] {"a", "b", "starX", "starY"});
        }

        public override int Id { get; } = 155;
        public override int HasParameters { get; } = 4;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            _x = _y;
            _y = -_xa + 2.0 * _a * (_y / (1.0 + _y * _y)) + _b * _y;
            _xa = _x;
            _x = _y;
            _y = -_xa + 2.0 * _a * (_y / (1.0 + _y * _y)) + _b * _y;
            var dx = W * _x;
            var dy = W * _y;
            _xa = _x;

            return new Point(dx, dy);
        }

        public override void Init()
        {
            _a = P1;
            _b = P2;
            _x = P3;
            _y = P4;
            _xa = P3;
        }
    }
}