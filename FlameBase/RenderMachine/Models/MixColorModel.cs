using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ColorMine.ColorSpaces;

namespace FlameBase.RenderMachine.Models
{
    public static class MixColorModel
    {
        public static Color BlendRgb(Color color, Color backColor, double amount)
        {
            var r = (byte) (color.R * amount + backColor.R * (1.0 - amount));
            var g = (byte) (color.G * amount + backColor.G * (1.0 - amount));
            var b = (byte) (color.B * amount + backColor.B * (1.0 - amount));
            return Color.FromRgb(r, g, b);
        }

        public static Color MixC(Color color1, Color color2)
        {
            var lab = MixL(color1, color2);
            var rgb = lab.ToRgb();
            return Color.FromRgb((byte) rgb.R, (byte) rgb.G, (byte) rgb.B);
        }

        public static Lab MixL(Color color1, Color color2)
        {
            var lab1 = GetLabFromColor(color1);
            var lab2 = GetLabFromColor(color2);
            var resultLab = new Lab
                {A = (lab1.A + lab2.A) * .5, B = (lab1.B + lab2.B) * .5, L = (lab1.L + lab2.L) * .5};
            return resultLab;
        }

        public static Color AdditiveMix(IReadOnlyCollection<Color> colors)
        {
            var red = 0;
            var green = 0;
            var blue = 0;
            for (var i = 0; i < colors.Count; i++)
            {
                var background = colors.ElementAt(i);
                red += background.R;
                green += background.G;
                blue += background.B;
            }

            return Color.FromRgb(
                (byte) Math.Min(255, red),
                (byte) Math.Min(255, green),
                (byte) Math.Min(255, blue));
        }


        public static Color SubtractiveMix(IReadOnlyCollection<Color> colors)
        {
            var red = 1;
            var green = 1;
            var blue = 1;
            for (var i = 0; i < colors.Count; i++)
            {
                var background = colors.ElementAt(i);
                red *= background.R;
                green *= background.G;
                blue *= background.B;
            }

            return Color.FromRgb(
                (byte) Math.Min(255, red / 255.0),
                (byte) Math.Min(255, green / 255.0),
                (byte) Math.Min(255, blue / 255.0));
        }

        public static Color DilutingSubtractiveMix(IReadOnlyCollection<Color> colors)
        {
            var red = 0.0;
            var green = 0.0;
            var blue = 0.0;
            var count = colors.Count;
            for (var i = 0; i < count; i++)
            {
                var background = colors.ElementAt(i);
                red += Math.Pow(255.0 - background.R, 2);
                green += Math.Pow(255.0 - background.G, 2);
                blue += Math.Pow(255.0 - background.B, 2);
            }

            return Color.FromRgb(
                (byte) Math.Min(255, Math.Sqrt(red / count)),
                (byte) Math.Min(255, Math.Sqrt(green / count)),
                (byte) Math.Min(255, Math.Sqrt(blue / count)));
        }

        public static Lab GetLabFromColor(Color c)
        {
            return new Rgb {R = c.R, G = c.G, B = c.B}.To<Lab>();
        }


        public static Color MixRgb(Color c1, Color c2)
        {
            double r1 = c1.R;
            double g1 = c1.G;
            double b1 = c1.B;
            double r2 = c2.R;
            double g2 = c2.G;
            double b2 = c2.B;

            var w1 = Math.Min(r1, Math.Min(g1, b1));
            var w2 = Math.Min(r2, Math.Min(g2, b2));

            // remove white before mixing
            r1 -= w1;
            g1 -= w1;
            b1 -= w1;
            r2 -= w2;
            g2 -= w2;
            b2 -= w2;

            var m1 = Math.Max(r1, Math.Max(g1, b1));
            var m2 = Math.Max(r2, Math.Max(g2, b2));

            var br = (m1 + m2) / (2 * 255.0);

            var r3 = (r1 + r2) * br;
            var g3 = (g1 + g2) * br;
            var b3 = (b1 + b2) * br;

            // average whiteness and add into final color
            var w3 = (w1 + w2) / 2;

            r3 += w3;
            g3 += w3;
            b3 += w3;
            return Color.FromRgb((byte) r3, (byte) g3, (byte) b3);
        }


        public class KmColor
        {
            private double _aB; // BLUE channel absorbance

            private double _aG; // GREEN channel absorbance

            // Kubelka-Munk absorption coefficient to scattering coefficient ratios for each channel (also known as the Absorbance)
            private double _aR; // RED channel absorbance

            /**
             * Creates a new Color
             * @param color The color to create
             */
            public KmColor(Color color)
            {
                // normalize the RGB color values
                var red = color.R == 0 ? 0.00001 : color.R / 255.0;
                var green = color.G == 0 ? 0.00001 : color.G / 255.0;
                var blue = color.B == 0 ? 0.00001 : color.B / 255.0;

                // calculate an Absorbance measure for each channel of the color
                _aR = CalculateAbsorbance(red);
                _aG = CalculateAbsorbance(green);
                _aB = CalculateAbsorbance(blue);
            }

            /**
             * Returns a Reflectance measure.  Assumes the color is opaque.
             * @param absortionRatio Kubelka-Munk absorption coefficient to scattering coefficient ratio
             * @return
             */
            private static double CalculateReflectance(double absorbtionRatio)
            {
                return 1.0 + absorbtionRatio - Math.Sqrt(Math.Pow(absorbtionRatio, 2.0) + 2.0 * absorbtionRatio);
            }

            /**
             * Returns an absorbance (K/S) measure for a given RGB channel.  
             * @param RGBChannelValue (integer value between 0 and 255).
             * @return
             */
            private static double CalculateAbsorbance(double rgbChannelValue)
            {
                return Math.Pow(1.0 - rgbChannelValue, 2.0) / (2.0 * rgbChannelValue);
            }

            /**
             * Mixes a collection of colors into this color
             * Calculates a new K and S coefficient using a weighted average of all K and S coefficients
             * In this implementation we assume each color has an equal concentration so each concentration 
             * weight will be equal to 1/(1 + colors.size)
             * @param colors The Colors to mix into this Color
             */
            public void Mix(IReadOnlyCollection<Color> colors)
            {
                var length = colors.Count;
                // just a little error checking
                if (length == 0) return;

                // calculate a concentration weight for a equal concentration of all colors in mix
                var concentration = 1.0 / (1.0 + length);

                // calculate first iteration
                var aR = _aR * concentration;
                var aG = _aG * concentration;
                var aB = _aB * concentration;

                // sum the weighted average
                for (var i = 0; i < length; i++)
                {
                    var color = new KmColor(colors.ElementAt(i));
                    aR += color._aR * concentration;
                    aG += color._aG * concentration;
                    aB += color._aB * concentration;
                }

                // update with results
                _aR = aR;
                _aG = aG;
                _aB = aB;
            }

            /**
             * Mixes another color into this color
             * Calculates a new K and S coefficient using by averaging the K and S values
             * In this implementation we assume each color has an equal concentration
             * @param color The Color to mix into this Color
             */
            public void Mix(Color color)
            {
                // calculate new KS (Absorbance) for mix with one color of equal concentration
                var kmColor = new KmColor(color);
                _aR = (_aR + kmColor._aR) / 2.0;
                _aG = (_aG + kmColor._aG) / 2.0;
                _aB = (_aB + kmColor._aB) / 2.0;
            }

            /**
             * Returns a standard RGB color as a java.awt.Color object
             * @return
             */
            public Color GetColor()
            {
                var red = (byte) (CalculateReflectance(_aR) * 255.0);
                var green = (byte) (CalculateReflectance(_aG) * 255.0);
                var blue = (byte) (CalculateReflectance(_aB) * 255.0);
                return Color.FromRgb(red, green, blue);
            }
        }
    }
}