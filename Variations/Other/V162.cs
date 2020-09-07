using System.Windows;
using FlameBase.Models;

namespace Variations.Other
{
    public class V162 : VariationModel
    {
        public override int Id { get; } = 162;
        public override int HasParameters { get; } = 0;
        public override bool IsDependent { get; } = false;

        public V162()
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