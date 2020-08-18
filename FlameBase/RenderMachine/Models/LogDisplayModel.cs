using System;
using System.Collections.Generic;
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
    public class LogDisplayModel
    {
        private readonly uint[,,] _display;

        public LogDisplayModel(int width, int height, int colorCount)
        {
            _display = new uint[width, height, colorCount];
            Width = width;
            Height = height;
            ColorCount = colorCount;
        }

        // public LogDisplayModel(uint[,,] array)
        // {
        //     Width = array.GetLength(0);
        //     Height = array.GetLength(1);
        //     ColorCount = array.GetLength(2);
        //     _display = CopyArray(array);
        //     GetMaxShots();
        // }

        public uint Max { get; private set; }
        public int Height { get; }
        public int Width { get; }
        public int ColorCount { get; }
        public double Contrast { get; set; } = 80.0;

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


        private Hsb GetColorGm(uint x, uint y, IReadOnlyList<double> gradientValues, GradientModel gradModel)
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

            var mix = new Hsb();
            if (total == 0) return mix;
            var colorValue = sum / total;
            var color = gradModel.GetFromPosition(colorValue);
            var rgb = new Rgb {R = color.R, G = color.G, B = color.B};
            mix = rgb.To<Hsb>();
            return mix;
        }

        private Lab GetColorCm(uint i, uint j, IReadOnlyList<Lab> labs)
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
            var tmp = new LogDisplayModel(Width, Height, ColorCount) {Max = Max, Contrast = Contrast};
            CopyDisplay(tmp);
            return tmp;
        }

        #endregion

        #region gbfr

        public BitmapSource GetBitmapForRender(Color[] funColors, bool fast = false)
        {
            var rect = new Int32Rect(0, 0, Width, Height);
            var img = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgr32, null);

            Algebra.DividePlane(Width, Height, Environment.ProcessorCount, out var dic);
            var length = dic.Count;
            var labsCm = ColorsToLabsCm(funColors);

            var stride = Width * 4;
            var a = new byte[stride * Height];

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var iLabCm = labsCm.ToArray();
                var f = fast;
                var contrast = Contrast;
                var str = stride;
                for (var y = (uint)d[1]; y < d[1] + d[3]; y++)
                    for (var x = (uint)d[0]; x < d[0] + d[2]; x++)
                    {
                        var shots = CountShots(x, y);
                        if (shots == 0) continue;
                        var colorCm = GetColorCm(x, y, iLabCm);
                        var colorHsb = colorCm.To<Hsb>();
                        if (!f)
                            // var log = Math.Log(1.0 + shots, Max) * contrast;
                            // colorCm.L = log;
                            colorHsb.B = Math.Log(1.0 + shots, Max);

                        // var cCmRgb = colorCm.ToRgb();
                        var cCmRgb = colorHsb.ToRgb();

                        var r = (byte)Math.Round(cCmRgb.R);
                        var g = (byte)Math.Round(cCmRgb.G);
                        var b = (byte)Math.Round(cCmRgb.B);

                        var n = y * str + x * 4;
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

        public BitmapSource GetBitmapForRender(double[] gradientValues, GradientModel gradModel, bool fast = false)
        {
            var rect = new Int32Rect(0, 0, Width, Height);
            var img = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgr32, null);

            Algebra.DividePlane(Width, Height, Environment.ProcessorCount, out var dic);
            var length = dic.Count;
            // var labsCm = ColorsToLabsCm(funColors);

            var stride = Width * 4;
            var a = new byte[stride * Height];

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                // var iLabCm = labsCm.ToArray();
                var f = fast;
                var contrast = Contrast;
                var str = stride;
                for (var y = (uint)d[1]; y < d[1] + d[3]; y++)
                    for (var x = (uint)d[0]; x < d[0] + d[2]; x++)
                    {
                        var shots = CountShots(x, y);
                        if (shots == 0) continue;
                        var colorHsb = GetColorGm(x, y, gradientValues, gradModel);
                        // var colorHsb = colorGm.To<Hsb>();
                        if (!f)
                        {
                            // var log = Math.Log(1.0 + shots, Max) * contrast;
                            // colorGm.L = log;
                            var colorB = colorHsb.B;
                            colorB = Algebra.Map(Math.Log(1.0 + shots, Max), 0.0, 1.0, 0.0, colorB);
                            // colorHsb.B = Math.Log(1.0 + shots, Max);
                            colorHsb.B = colorB;
                        }

                        // var cCmRgb = colorGm.ToRgb();
                        var cCmRgb = colorHsb.ToRgb();
                        var r = (byte)Math.Round(cCmRgb.R);
                        var g = (byte)Math.Round(cCmRgb.G);
                        var b = (byte)Math.Round(cCmRgb.B);

                        var n = y * str + x * 4;
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
    }
}