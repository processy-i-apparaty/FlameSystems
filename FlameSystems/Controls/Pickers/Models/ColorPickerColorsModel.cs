using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using ColorMine.ColorSpaces;
using FlameBase.Enums;
using FlameSystems.Controls.Pickers.Enums;

namespace FlameSystems.Controls.Pickers.Models
{
    public static class ColorPickerColorsModel
    {
        public static Color GetColor(Point cubeXy, double columnY, ColorPickerMode colorMode)
        {
            Hsb hsb;
            Rgb rgb;
            switch (colorMode)
            {
                case ColorPickerMode.H:
                    hsb = new Hsb
                    {
                        H = 360.0 - columnY * 360.0,
                        S = cubeXy.X,
                        B = 1.0 - cubeXy.Y
                    };
                    return Rgb2Color(hsb.To<Rgb>());
                case ColorPickerMode.S:
                    hsb = new Hsb
                    {
                        H = cubeXy.X * 360.0,
                        S = 1.0 - columnY,
                        B = 1.0 - cubeXy.Y
                    };
                    return Rgb2Color(hsb.To<Rgb>());
                case ColorPickerMode.V:
                    hsb = new Hsb
                    {
                        H = 360.0 * cubeXy.X,
                        S = 1.0 - cubeXy.Y,
                        B = 1.0 - columnY
                    };
                    return Rgb2Color(hsb.To<Rgb>());
                case ColorPickerMode.R:
                    rgb = new Rgb
                    {
                        R = 255.0 - columnY * 255.0,
                        G = 255.0 - cubeXy.Y * 255.0,
                        B = 255.0 * cubeXy.X
                    };
                    return Rgb2Color(rgb);
                case ColorPickerMode.G:
                    rgb = new Rgb
                    {
                        R = 255.0 - cubeXy.Y * 255.0,
                        G = 255.0 - columnY * 255.0,
                        B = 255.0 * cubeXy.X
                    };
                    return Rgb2Color(rgb);
                case ColorPickerMode.B:
                    rgb = new Rgb
                    {
                        R = 255.0 * cubeXy.X,
                        G = 255.0 - cubeXy.Y * 255.0,
                        B = 255.0 - columnY * 255.0
                    };
                    return Rgb2Color(rgb);
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
            }
        }

        private static Color Rgb2Color(IRgb rgb)
        {
            return Color.FromRgb((byte) Math.Round(rgb.R), (byte) Math.Round(rgb.G), (byte) Math.Round(rgb.B));
        }


        public static bool TryParseHexColor(string hexColor, out Color color)
        {
            color = new Color();
            if (hexColor == null) return false;
            if (hexColor.Length > 6) return false;
            try
            {
                var hex = int.Parse(hexColor, NumberStyles.HexNumber);
                var r = (byte) ((hex & 0xff0000) >> 0x10);
                var g = (byte) ((hex & 0xff00) >> 8);
                var b = (byte) (hex & 0xff);
                color.R = r;
                color.G = g;
                color.B = b;
                color.A = 255;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}