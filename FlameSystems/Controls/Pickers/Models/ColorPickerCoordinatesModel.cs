using System;
using System.Windows;
using System.Windows.Media;
using ColorMine.ColorSpaces;
using FlameSystems.Controls.Pickers.Enums;

namespace FlameSystems.Controls.Pickers.Models
{
    public static class ColorPickerCoordinatesModel
    {
        public static void Get(Color color, ColorPickerMode colorMode, out Point cubeXy, out double columnY)
        {
            var r = color.R;
            var g = color.G;
            var b = color.B;

            double h = 0.0, s = 0.0, v = 0.0;
            if ((int) colorMode < 3)
            {
                var rgb = new Rgb
                {
                    R = r,
                    G = g,
                    B = b
                };
                var hsb = rgb.To<Hsb>();
                h = hsb.H;
                s = hsb.S;
                v = hsb.B;
            }

            Count(colorMode, h, s, v, r, g, b, out cubeXy, out columnY);
        }

        public static void Get(Hsb hsb, ColorPickerMode colorMode, out Point cubeXy, out double columnY)
        {
            var h = hsb.H;
            var s = hsb.S;
            var v = hsb.B;

            double r = 0.0, g = 0.0, b = 0.0;
            if ((int) colorMode > 2)
            {
                var rgb = hsb.To<Rgb>();
                r = rgb.R;
                g = rgb.G;
                b = rgb.B;
            }

            Count(colorMode, h, s, v, r, g, b, out cubeXy, out columnY);
        }

        private static void Count(ColorPickerMode colorMode, double h, double s, double v, double r, double g, double b,
            out Point cubeXy, out double columnY)
        {
            cubeXy = new Point();

            switch (colorMode)
            {
                case ColorPickerMode.H:
                    cubeXy.X = s;
                    cubeXy.Y = 1.0 - v;
                    columnY = 1.0 - h / 360.0;
                    return;
                case ColorPickerMode.S:
                    cubeXy.X = h / 360.0;
                    cubeXy.Y = 1.0 - v;
                    columnY = 1.0 - s;
                    return;
                case ColorPickerMode.V:
                    cubeXy.X = h / 360.0;
                    cubeXy.Y = 1.0 - s;
                    columnY = 1.0 - v;
                    return;
                case ColorPickerMode.R:
                    cubeXy.X = b / 255.0;
                    cubeXy.Y = 1.0 - g / 255.0;
                    columnY = 1.0 - r / 255.0;
                    return;
                case ColorPickerMode.G:
                    cubeXy.X = b / 255.0;
                    cubeXy.Y = 1.0 - r / 255.0;
                    columnY = 1.0 - g / 255.0;
                    return;
                case ColorPickerMode.B:
                    cubeXy.X = r / 255.0;
                    cubeXy.Y = 1.0 - g / 255.0;
                    columnY = 1.0 - b / 255.0;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
            }
        }
    }
}