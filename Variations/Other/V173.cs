using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class V173 : VariationModel
    {
        public override int Id { get; } = 173;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public V173()
        {
            SetParameters(new[] { 0.0, 0.0, 0.0 }, new[] { "", "", "" });
        }

        public override Point Fun(Point p)
        {



            return new Point(x, y);
        }

        public override void Init()
        {

        }
    }
}