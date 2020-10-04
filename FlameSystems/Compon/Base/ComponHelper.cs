using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace FlameSystems.Compon.Base
{
    public static class ComponHelper
    {
        public static ComponTransformModel ComponentTransform(List<string> names, List<Type> types)
        {
            return new ComponTransformModel(names, types, new ComponBaseModel());
        }


        public static ComponTransformModel ComponentStandartTransform()
        {
            var names = new List<string>
            {
                "ShiftX", "ShiftY", "Angle", "ScaleX", "ScaleY", "ShearX", "ShearY", "Probability", "Weight",
                "ColorBrush", "Coefficients", "VariationSelected", "Parameter1", "Parameter2",
                "Parameter3", "Parameter4"
            };
            var types = new List<Type>
            {
                typeof(double), typeof(double), typeof(double), typeof(double),
                typeof(double), typeof(double), typeof(double), typeof(double), typeof(double),
                typeof(SolidColorBrush), typeof(string), typeof(string),
                typeof(double), typeof(double), typeof(double), typeof(double)
            };
            return new ComponTransformModel(names, types, new ComponTransformView());
        }
    }
}