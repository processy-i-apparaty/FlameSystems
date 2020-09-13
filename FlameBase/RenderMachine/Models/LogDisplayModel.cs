﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorMine.ColorSpaces;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace FlameBase.RenderMachine.Models
{
    [Serializable]
    public class LogDisplayModel
    {
        private static readonly Random Rand = new Random();
        private readonly uint[,,] _display;

        public LogDisplayModel(int width, int height, int colorCount, Color backColor)
        {
            _display = new uint[width, height, colorCount];
            Width = width;
            Height = height;
            ColorCount = colorCount;
            BackColor = backColor;
        }

        public LogDisplayModel(uint[,,] array, Color backColor)
        {
            Width = array.GetLength(0);
            Height = array.GetLength(1);
            ColorCount = array.GetLength(2);
            _display = CopyArray(array);
            GetMaxShots();
        }

        public Color BackColor { get; set; }
        public uint Max { get; private set; }
        public int Height { get; }
        public int Width { get; }
        public int ColorCount { get; }
        public double Contrast { get; set; } = 80.0;

        public RenderColorModeModel.RenderColorMode RenderColorMode { get; set; } =
            RenderColorModeModel.RenderColorMode.Hsb;

        public uint[,,] GetArrayCopy()
        {
            return CopyArray(_display);
        }


        #region log display

        public bool Shot(uint x, uint y, uint c)
        {
            _display[x, y, c]++;
            var count = CountShots(x, y);
            if (count > Max) Max = count;
            return true;
        }

        private static Lab[] ColorsToLabsCm(IReadOnlyList<Color> colors)
        {
            var labs = new Lab[colors.Count];
            for (var i = 0; i < colors.Count; i++)
            {
                var c = colors[i];
                var rgb = new Rgb {R = c.R, G = c.G, B = c.B};
                labs[i] = rgb.To<Lab>();
            }

            return labs;
        }


        private Hsb GetColorModeGradient(uint x, uint y, IReadOnlyList<double> gradientValues, GradientModel gradModel)
        {
            var length = gradientValues.Count;
            var total = 0u;
            var sum = 0.0;
            for (var i = 0; i < length; i++)
            {
                var d = _display[x, y, i];
                sum += d * gradientValues[i];
                total += d;
            }

            if (total == 0) return new Hsb();
            ;
            var colorValue = sum / total;
            return gradModel.GetLabFromPosition(colorValue).To<Hsb>();
        }


        private Lab GetColorModeColor(uint i, uint j, IReadOnlyList<Lab> labs)
        {
            var total = 0u;
            var length = labs.Count;
            var colorHits = new uint[length];
            for (var k = 0; k < length; k++)
            {
                colorHits[k] = _display[i, j, k];
                total += colorHits[k];
            }

            var mix = new Lab();

            if (total == 0) return mix;

            var coefficient = new double[length];
            for (var k = 0; k < length; k++)
                coefficient[k] = Algebra.Map(colorHits[k], 0.0, total, 0.0, 1.0);
            for (var k = 0; k < length; k++)
            {
                mix.L += labs[k].L * coefficient[k];
                mix.A += labs[k].A * coefficient[k];
                mix.B += labs[k].B * coefficient[k];
            }

            return mix;
        }

        private uint CountShots(uint x, uint y)
        {
            uint count = 0;
            for (var i = 0; i < ColorCount; i++) count += _display[x, y, i];
            return count;
        }

        private static uint[,,] CopyArray(uint[,,] array)
        {
            var d1 = array.GetLength(0);
            var d2 = array.GetLength(1);
            var d3 = array.GetLength(2);
            var ret = new uint[d1, d2, d3];
            for (var i = 0; i < d1; i++)
            for (var j = 0; j < d2; j++)
            for (var k = 0; k < d3; k++)
                ret[i, j, k] = array[i, j, k];
            return ret;
        }


        private void CopyDisplay(LogDisplayModel toDisplay)
        {
            var d1 = _display.GetLength(0);
            var d2 = _display.GetLength(1);
            var d3 = _display.GetLength(2);
            for (var i = 0; i < d1; i++)
            for (var j = 0; j < d2; j++)
            for (var k = 0; k < d3; k++)
                toDisplay._display[i, j, k] = _display[i, j, k];
            toDisplay.RenderColorMode = RenderColorMode;
        }

        private void GetMaxShots()
        {
            uint maxSum = 0;
            for (var y = 0; y < _display.GetLength(1); y++)
            for (var x = 0; x < _display.GetLength(0); x++)
            {
                uint sum = 0;
                for (var c = 0; c < _display.GetLength(2); c++) sum += _display[x, y, c];
                if (maxSum < sum) maxSum = sum;
            }

            Max = maxSum;
        }


        public LogDisplayModel Copy()
        {
            var tmp = new LogDisplayModel(Width, Height, ColorCount, BackColor) {Max = Max, Contrast = Contrast};
            CopyDisplay(tmp);
            return tmp;
        }

        #endregion

        #region gbfr

        private static Color Blend(Color color, Color backColor, double amount)
        {
            var r = (byte) (color.R * amount + backColor.R * (1 - amount));
            var g = (byte) (color.G * amount + backColor.G * (1 - amount));
            var b = (byte) (color.B * amount + backColor.B * (1 - amount));
            return Color.FromRgb(r, g, b);
        }


        private GradientModel _gm;

        #endregion


        #region get bitmap for render

        public BitmapSource GetBitmapForRender(Color[] funColors, bool fast = false)
        {
            Debug.WriteLine("GetBitmapForRender");
            var rect = new Int32Rect(0, 0, Width, Height);
            var img = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgr32, null);

            Algebra.DividePlane(Width, Height, Environment.ProcessorCount, out var dic);
            var length = dic.Count;

            var labsCm = ColorsToLabsCm(funColors);

            var stride = Width * 4;
            var pixels = new byte[stride * Height];

            var backColorLab = MixColorModel.GetLabFromColor(BackColor);

            var logMax = Math.Log(Max);

            // Debug.WriteLine("FILL_START");
            // for (var i = 0; i < pixels.Length; i += 4)
            // {
            //     pixels[i] = BackColor.B;
            //     pixels[i + 1] = BackColor.G;
            //     pixels[i + 2] = BackColor.R;
            //     pixels[i + 3] = 255;
            // }
            //
            // Debug.WriteLine("FILL_END");


            // img.FillRectangle(0, 0, Width, Height, BackColor);
            // FillBitmap(ref img, BackColor);

            var strides = new int[length];
            for (var i = 0; i < length; i++) strides[i] = stride;

            var parallelOptions = new ParallelOptions
                {MaxDegreeOfParallelism = Environment.ProcessorCount};

            var gamma = 1.0 / 1.5;

            Parallel.For(0, length, parallelOptions, i =>
            {
                var iLabCm = labsCm.ToArray();
                var contrast = Contrast;
                var renderColorMode = RenderColorMode;
                var max = Max;

                var d = dic[i];
                var f = fast;

                for (var y = (uint) d[1]; y < d[1] + d[3]; y++)
                for (var x = (uint) d[0]; x < d[0] + d[2]; x++)
                {
                    var shots = CountShots(x, y);

                    long n;
                    if (shots == 0)
                    {
                        n = y * strides[i] + x * 4;
                        pixels[n] = BackColor.B;
                        pixels[n + 1] = BackColor.G;
                        pixels[n + 2] = BackColor.R;
                        pixels[n + 3] = 255;
                        continue;
                    }

                    //TODO: GetBitmapForRender mix colors
                    // var cCmRgb = GetColorCm(x, y, funColors);

                    var colorLab = GetColorModeColor(x, y, iLabCm);
                    var colorCm = colorLab.ToRgb();
                    var cCmRgb = Color.FromRgb((byte) colorCm.R, (byte) colorCm.G, (byte) colorCm.B);


                    var log = 1.0;


                    switch (renderColorMode)
                    {
                        case RenderColorModeModel.RenderColorMode.Hsb:
                            if (!f)
                                log = Math.Log(1.0 + shots, max);
                            cCmRgb = Blend(cCmRgb, BackColor, log);
                            break;
                        case RenderColorModeModel.RenderColorMode.Lab:
                            if (!f)
                            {
                                log = Math.Pow(Math.Log(1.0 + shots, max), gamma);
                            }

                            cCmRgb = BlendLab(colorLab, backColorLab, log, contrast);
                            break;
                        case RenderColorModeModel.RenderColorMode.LogGamma:
                            if (!f)
                            {
                                //alpha[x][y] := log(frequency_avg[x][y]) / log(frequency_max);
                                //frequency_max is the maximal number of iterations that hit a cell in the histogram.
                                //final_pixel_color[x][y] := color_avg[x][y] * alpha[x][y] ^ (1 / gamma); //gamma is a value greater than 1.

                                var alpha = Math.Log(shots) / logMax;
                                log = Math.Pow(alpha, gamma);
                                //log = 1.0 - Math.Pow(Math.Log(shots) / shots, gamma);
                            }

                            cCmRgb = Blend(cCmRgb, BackColor, log);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    // log = Math.Log(shots) / shots;
                    // log = Math.Pow(log, gamma);


                    var r = cCmRgb.R;
                    var g = cCmRgb.G;
                    var b = cCmRgb.B;

                    // var colorCm = GetColorCm(x, y, iLabCm);

                    // if (!f)
                    // {
                    //     var log = Math.Log(1.0 + shots, Max) * contrast;
                    //     colorCm.L = log;
                    // }
                    // var cCmRgb = colorCm.ToRgb();

                    // var colorHsb = colorCm.To<Hsb>();
                    // if (!f)
                    // {
                    // var log = = Math.Log(1.0 + shots, Max);
                    // colorHsb.B = Math.Log(1.0 + shots, Max);
                    // }
                    // var cCmRgb = colorHsb.ToRgb();

                    // var r = (byte) Math.Round(cCmRgb.R);
                    // var g = (byte) Math.Round(cCmRgb.G);
                    // var b = (byte) Math.Round(cCmRgb.B);

                    n = y * strides[i] + x * 4;
                    pixels[n] = b;
                    pixels[n + 1] = g;
                    pixels[n + 2] = r;
                    pixels[n + 3] = 255;
                }
            });

            Debug.WriteLine("\tPARALLEL END.");

            img.Lock();
            img.WritePixels(rect, pixels, stride, 0);
            img.Unlock();
            img.Freeze();
            Debug.WriteLine("GetBitmapForRender end");
            return img;
        }

        private static Color BlendLab(ILab color, ILab backColor, double log, double contrast)
        {
            var logInv = 1.0 - log;

            var b = new Lab
            {
                A = color.A * log + backColor.A * logInv,
                B = color.B * log + backColor.B * logInv,
                L = color.L * log + backColor.L * logInv
            }.ToRgb();

            return Color.FromRgb((byte) b.R, (byte) b.G, (byte) b.B);
        }

        public BitmapSource GetBitmapForRender(double[] gradientValues, GradientModel gradModel, bool fast = false)
        {
            var rect = new Int32Rect(0, 0, Width, Height);
            var img = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgr32, null);

            Algebra.DividePlane(Width, Height, Environment.ProcessorCount, out var dic);
            var length = dic.Count;
            // var labsCm = ColorsToLabsCm(funColors);

            var stride = Width * 4;
            var a = new byte[stride * Height];

            _gm = gradModel.Copy();

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                // var iLabCm = labsCm.ToArray();
                var f = fast;
                // var contrast = Contrast;
                var str = stride;
                for (var y = (uint) d[1]; y < d[1] + d[3]; y++)
                for (var x = (uint) d[0]; x < d[0] + d[2]; x++)
                {
                    var shots = CountShots(x, y);
                    long n;
                    if (shots == 0)
                    {
                        n = y * str + x * 4;
                        a[n] = BackColor.B;
                        a[n + 1] = BackColor.G;
                        a[n + 2] = BackColor.R;
                        a[n + 3] = 255;

                        continue;
                    }

                    var colorHsb = GetColorModeGradient(x, y, gradientValues, _gm);
                    var colorCm = colorHsb.ToRgb();
                    var cCmRgb = Color.FromRgb((byte) colorCm.R, (byte) colorCm.G, (byte) colorCm.B);
                    var log = 1.0;
                    if (!f)
                        log = Math.Log(1.0 + shots, Max);
                    // log = Math.Log(shots)/shots;//grad

                    cCmRgb = Blend(cCmRgb, BackColor, log);
                    var r = cCmRgb.R;
                    var g = cCmRgb.G;
                    var b = cCmRgb.B;

                    // // var colorHsb = colorGm.To<Hsb>();
                    // if (!f)
                    // {
                    //     // var log = Math.Log(1.0 + shots, Max) * contrast;
                    //     // colorGm.L = log;
                    //     var colorB = colorHsb.B;
                    //     colorB = Algebra.Map(Math.Log(1.0 + shots, Max), 0.0, 1.0, 0.0, colorB);
                    //     // colorHsb.B = Math.Log(1.0 + shots, Max);
                    //     colorHsb.B = colorB;
                    // }
                    //
                    // // var cCmRgb = colorGm.ToRgb();
                    // var cCmRgb = colorHsb.ToRgb();
                    // var r = (byte) Math.Round(cCmRgb.R);
                    // var g = (byte) Math.Round(cCmRgb.G);
                    // var b = (byte) Math.Round(cCmRgb.B);

                    n = y * str + x * 4;
                    a[n] = b;
                    a[n + 1] = g;
                    a[n + 2] = r;
                    a[n + 3] = 255;
                }
            });

            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        #endregion


        #region commented

        // private uint[] GetDisplayXy(uint x, uint y)
        // {
        //     var displayXy = new uint[ColorCount];
        //     for (var i = 0; i < ColorCount; i++) displayXy[i] = _display[x, y, i];
        //     return displayXy;
        // }


        // private Hsb GetColorModeGradient(uint x, uint y, IReadOnlyList<double> gradientValues, GradientModel gradModel)
        // {
        //
        //     var displayXy = GetDisplayXy(x, y);
        //     var colorValue = gradientValues[0];
        //
        //     for (int i = 0; i < ColorCount; i++)
        //     {
        //         
        //     }
        //
        //     var color = gradModel.GetFromPosition(colorValue);
        //     var rgb = new Rgb { R = color.R, G = color.G, B = color.B };
        //     mix = rgb.To<Hsb>();
        //     return mix;
        // }

        // private Hsb GetColorModeGradient2(uint x, uint y, IReadOnlyList<double> gradientValues, GradientModel gradModel)
        // {
        //     var displayXy = GetDisplayXy(x, y);
        //     var total = (uint) displayXy.Select(n => (long) n).Sum();
        //     var mix = new Hsb();
        //     if (total == 0) return mix;
        //
        //     var displayXyDouble = new double[ColorCount];
        //     for (var i = 0; i < ColorCount; i++) displayXyDouble[i] = (double) displayXy[i] / total;
        //
        //     var colorValue = 0.0;
        //     for (var i = 0; i < ColorCount; i++)
        //     {
        //         colorValue += displayXyDouble[i] * gradientValues[i];
        //     }
        //
        //     if (displayXy[0] > 0 && displayXy[1] > 0)
        //     {
        //     }
        //
        //     return gradModel.GetLabFromPosition(colorValue).To<Hsb>();
        // }

        // private Lab GetColorCm1(uint i, uint j, IReadOnlyList<Lab> labs)
        // {
        //     var total = 0u;
        //     var length = labs.Count;
        //     var colorHits = new uint[length];
        //     for (var k = 0; k < length; k++)
        //     {
        //         colorHits[k] = _display[i, j, k];
        //         total += colorHits[k];
        //     }
        //
        //     var backColorLab = new Rgb
        //     {
        //         R = BackColor.R,
        //         G = BackColor.G,
        //         B = BackColor.B
        //     }.To<Lab>();
        //
        //     var mix = new Lab();
        //
        //
        //     //TODO: GetColorCm mix back
        //     if (total == 0) return backColorLab;
        //
        //     var coefficient = new double[length];
        //     for (var k = 0; k < length; k++)
        //         coefficient[k] = Algebra.Map(colorHits[k], 0.0, total, 0.0, 1.0);
        //     for (var k = 0; k < length; k++)
        //     {
        //         mix.L += labs[k].L * coefficient[k];
        //         mix.A += labs[k].A * coefficient[k];
        //         mix.B += labs[k].B * coefficient[k];
        //     }
        //
        //     mix.L = (mix.L + backColorLab.L) * .5;
        //     mix.A = (mix.A + backColorLab.A) * .5;
        //     mix.B = (mix.B + backColorLab.B) * .5;
        //
        //     return mix;
        // }

        // private Color GetColorModeColor(uint i, uint j, IReadOnlyList<Color> colors)
        // {
        //     var length = colors.Count;
        //     var color = Colors.Black;
        //
        //     for (var k = 0; k < length; k++)
        //     {
        //         var c = _display[i, j, k];
        //         for (var x = 0; x < c; x++) color = MixColors(color, colors[k]);
        //     }
        //
        //     return color;
        // }

        // private static Color MixColors(Color c1, Color c2)
        // {
        //     var r = (byte) Math.Round((c1.R + c2.R) * .5);
        //     var g = (byte) Math.Round((c1.G + c2.G) * .5);
        //     var b = (byte) Math.Round((c1.B + c2.B) * .5);
        //     return Color.FromRgb(r, g, b);
        // }

        // private void FillBitmap(ref WriteableBitmap bmp, Color color)
        // {
        //     var r = color.R;
        //     var g = color.G;
        //     var b = color.B;
        //     byte a = 255;
        //     var width = bmp.PixelWidth;
        //     var stride = width * bmp.Format.BitsPerPixel / 8;
        //     for (var x = 0; x < width; x++)
        //     for (var y = 0; y < bmp.PixelHeight; y++)
        //     {
        //         byte[] colorData = {b, g, r, a};
        //         var rect = new Int32Rect(x, y, 1, 1);
        //         bmp.WritePixels(rect, colorData, stride, 0);
        //     }
        // }
        //
        // private static int ConvertColor(Color color)
        // {
        //     var num1 = 0;
        //     if (color.A == 0) return num1;
        //
        //     var num2 = color.A + 1;
        //     num1 = (color.A << 24) | ((byte) ((color.R * num2) >> 8) << 16) |
        //            ((byte) ((color.G * num2) >> 8) << 8) | (byte) ((color.B * num2) >> 8);
        //
        //     return num1;
        // }

        // private static Color Blend(Color c1, Color c2)
        // {
        //     var l1 = new Rgb {R = c1.R, G = c1.G, B = c1.B}.To<Lab>();
        //     var l2 = new Rgb {R = c2.R, G = c2.G, B = c2.B}.To<Lab>();
        //     var l3 = new Lab
        //     {
        //         L = (l1.L + l2.L) * .5,
        //         A = (l1.A + l2.A) * .5,
        //         B = (l1.B + l2.B) * .5
        //     }.To<Rgb>();
        //     return Color.FromRgb((byte) l3.R, (byte) l3.G, (byte) l3.B);
        // }

        //
        // private Color MixRgbColors(Color c1, Color c2)
        // {
        //     var mix = new Color
        //     {
        //         R = (byte) Math.Round(c1.R * c2.R / 255.0),
        //         G = (byte) Math.Round(c1.G * c2.G / 255.0),
        //         B = (byte) Math.Round(c1.B * c2.B / 255.0)
        //     };
        //     return mix;
        // }

        #endregion
    }
}