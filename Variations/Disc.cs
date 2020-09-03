using System;
using System.Windows;
using FlameBase.Models;

namespace Variations
{
    public class Disc : VariationModel
    {
        public override int Id { get; } = 8;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public override Point Fun(Point p)
        {
            //            var r = VariationHelper.R(p);
            //            var theta = VariationHelper.Theta(p);
            //            var tpi = theta / Math.PI * W;
            //            var pir = Math.PI * r;
            //            return new Point(
            //                tpi * Math.Sin(pir),
            //                tpi * Math.Cos(pir));

            var n2 = Math.PI * VariationHelper.R(p);
            var sin = Math.Sin(n2);
            var cos = Math.Cos(n2);
            var n3 = W * VariationHelper.PreAtan(p) / Math.PI;
            return new Point(sin * n3, cos * n3);
        }

        public override void Init()
        {
        }
    }
}