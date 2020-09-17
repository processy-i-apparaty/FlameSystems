using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorMine.ColorSpaces;
using FlameBase.FlameMath;
using FlameSystems.Controls.Pickers.Enums;

namespace FlameSystems.Controls.Pickers.Models
{
    public static class ColorPickerCubeModel
    {
        private static int _cores;

        public static WriteableBitmap Get(ColorPickerMode colorMode, double columnY, int cubeWidth)
        {
            _cores = Environment.ProcessorCount;

            switch (colorMode)
            {
                case ColorPickerMode.H:
                    return CubeH(cubeWidth, columnY);
                case ColorPickerMode.S:
                    return CubeS(cubeWidth, columnY);
                case ColorPickerMode.V:
                    return CubeV(cubeWidth, columnY);
                case ColorPickerMode.R:
                    return CubeR(cubeWidth, columnY);
                case ColorPickerMode.G:
                    return CubeG(cubeWidth, columnY);
                case ColorPickerMode.B:
                    return CubeB(cubeWidth, columnY);
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
            }
        }


        private static WriteableBitmap CubeB(int cubeWidth, double columnY)
        {
            Algebra.DividePlane(cubeWidth, cubeWidth, _cores, out var dic);
            var length = dic.Count;
            var stride = cubeWidth * 4;
            var a = new byte[stride * cubeWidth];
            var step = 255.0 / cubeWidth;


            var b = (byte) Math.Round(255.0 - columnY * 255.0);

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var n = y * str + x * 4;

                    a[n] = b;
                    a[n + 1] = (byte) Math.Round(255.0 - stp * y);
                    a[n + 2] = (byte) Math.Round(stp * x);
                    a[n + 3] = 255;
                }
            });
            var rect = new Int32Rect(0, 0, cubeWidth, cubeWidth);
            var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }


        private static WriteableBitmap CubeG(int cubeWidth, double columnY)
        {
            Algebra.DividePlane(cubeWidth, cubeWidth, _cores, out var dic);
            var length = dic.Count;
            var stride = cubeWidth * 4;
            var a = new byte[stride * cubeWidth];
            var step = 255.0 / cubeWidth;


            var g = (byte) Math.Round(255.0 - columnY * 255.0);

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var n = y * str + x * 4;

                    a[n] = (byte) Math.Round(stp * x);
                    a[n + 1] = g;
                    a[n + 2] = (byte) Math.Round(255.0 - stp * y);
                    a[n + 3] = 255;
                }
            });
            var rect = new Int32Rect(0, 0, cubeWidth, cubeWidth);
            var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }


        private static WriteableBitmap CubeR(int cubeWidth, double columnY)
        {
            Algebra.DividePlane(cubeWidth, cubeWidth, _cores, out var dic);
            var length = dic.Count;
            var stride = cubeWidth * 4;
            var a = new byte[stride * cubeWidth];

            var step = 255.0 / cubeWidth;
            var r = (byte) Math.Round(255.0 - columnY * 255.0);

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var n = y * str + x * 4;

                    a[n] = (byte) Math.Round(stp * x);
                    a[n + 1] = (byte) Math.Round(255.0 - stp * y);
                    a[n + 2] = r;
                    a[n + 3] = 255;
                }
            });
            var rect = new Int32Rect(0, 0, cubeWidth, cubeWidth);
            var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }


        private static WriteableBitmap CubeV(int cubeWidth, double columnY)
        {
            Algebra.DividePlane(cubeWidth, cubeWidth, _cores, out var dic);
            var length = dic.Count;
            var stride = cubeWidth * 4;
            var a = new byte[stride * cubeWidth];
            var v = 1.0 - columnY;
            var stepY = 1.0 / cubeWidth;
            var stepX = 360.0 / cubeWidth;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var n = y * str + x * 4;
                    var hsb = new Hsb
                    {
                        H = stepX * x,
                        S = 1.0 - stepY * y,
                        B = v
                    };
                    var rgb = hsb.To<Rgb>();
                    var r = (byte) Math.Round(rgb.R);
                    var g = (byte) Math.Round(rgb.G);
                    var b = (byte) Math.Round(rgb.B);
                    a[n] = b;
                    a[n + 1] = g;
                    a[n + 2] = r;
                    a[n + 3] = 255;
                }
            });
            var rect = new Int32Rect(0, 0, cubeWidth, cubeWidth);
            var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        private static WriteableBitmap CubeS(int cubeWidth, double columnY)
        {
            Algebra.DividePlane(cubeWidth, cubeWidth, _cores, out var dic);
            var length = dic.Count;
            var stride = cubeWidth * 4;
            var a = new byte[stride * cubeWidth];

            var stepY = 1.0 / cubeWidth;
            var stepX = 360.0 / cubeWidth;
            var s = 1.0 - columnY;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var n = y * str + x * 4;
                    var hsb = new Hsb
                    {
                        H = stepX * x,
                        S = s,
                        B = 1.0 - stepY * y
                    };
                    var rgb = hsb.To<Rgb>();
                    var r = (byte) Math.Round(rgb.R);
                    var g = (byte) Math.Round(rgb.G);
                    var b = (byte) Math.Round(rgb.B);
                    a[n] = b;
                    a[n + 1] = g;
                    a[n + 2] = r;
                    a[n + 3] = 255;
                }
            });
            var rect = new Int32Rect(0, 0, cubeWidth, cubeWidth);
            var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }


        private static WriteableBitmap CubeH(int cubeWidth, double columnY)
        {
            Algebra.DividePlane(cubeWidth, cubeWidth, _cores, out var dic);
            var length = dic.Count;
            var stride = cubeWidth * 4;
            var a = new byte[stride * cubeWidth];

            var step = 1.0 / cubeWidth;

            var h = 360.0 - columnY * 360.0;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var n = y * str + x * 4;
                    var hsb = new Hsb
                    {
                        H = h,
                        S = step * x,
                        B = 1.0 - step * y
                    };
                    var rgb = hsb.To<Rgb>();
                    var r = (byte) Math.Round(rgb.R);
                    var g = (byte) Math.Round(rgb.G);
                    var b = (byte) Math.Round(rgb.B);
                    a[n] = b;
                    a[n + 1] = g;
                    a[n + 2] = r;
                    a[n + 3] = 255;
                }
            });
            var rect = new Int32Rect(0, 0, cubeWidth, cubeWidth);
            var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        #region one thread

        // private static WriteableBitmap CubeB1(int cubeWidth, double columnY)
        // {
        //     var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
        //     var b = 255.0 - columnY * 255.0;
        //     var g = 255.0;
        //     var step = 255.0 / cubeWidth;
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     for (var y = 0; y < cubeWidth; y++)
        //     {
        //         var r = 0.0;
        //         for (var x = 0; x < cubeWidth; x++)
        //         {
        //             unsafe
        //             {
        //                 var pBackBuffer = (int) img.BackBuffer;
        //                 pBackBuffer += y * stride;
        //                 pBackBuffer += x * 4;
        //                 var colorData = (byte) r << 16;
        //                 colorData |= (byte) g << 8;
        //                 colorData |= (byte) b;
        //                 *(int*) pBackBuffer = colorData;
        //             }
        //
        //             r += step;
        //         }
        //
        //         g -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, cubeWidth, cubeWidth));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap CubeG1(int cubeWidth, double columnY)
        // {
        //     var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
        //     var g = 255.0 - columnY * 255.0;
        //     var r = 255.0;
        //     var step = 255.0 / cubeWidth;
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     for (var y = 0; y < cubeWidth; y++)
        //     {
        //         var b = 0.0;
        //         for (var x = 0; x < cubeWidth; x++)
        //         {
        //             unsafe
        //             {
        //                 var pBackBuffer = (int) img.BackBuffer;
        //                 pBackBuffer += y * stride;
        //                 pBackBuffer += x * 4;
        //                 var colorData = (byte) r << 16;
        //                 colorData |= (byte) g << 8;
        //                 colorData |= (byte) b;
        //                 *(int*) pBackBuffer = colorData;
        //             }
        //
        //             b += step;
        //         }
        //
        //         r -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, cubeWidth, cubeWidth));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap CubeR1(int cubeWidth, double columnY)
        // {
        //     var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
        //     var r = 255.0 - columnY * 255.0;
        //     var g = 255.0;
        //     var step = 255.0 / cubeWidth;
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     for (var y = 0; y < cubeWidth; y++)
        //     {
        //         var b = 0.0;
        //         for (var x = 0; x < cubeWidth; x++)
        //         {
        //             unsafe
        //             {
        //                 var pBackBuffer = (int) img.BackBuffer;
        //                 pBackBuffer += y * stride;
        //                 pBackBuffer += x * 4;
        //                 var colorData = (byte) r << 16;
        //                 colorData |= (byte) g << 8;
        //                 colorData |= (byte) b;
        //                 *(int*) pBackBuffer = colorData;
        //             }
        //
        //             b += step;
        //         }
        //
        //         g -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, cubeWidth, cubeWidth));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        //
        // private static WriteableBitmap CubeV1(int cubeWidth, double columnY)
        // {
        //     var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
        //     var v = 1.0 - columnY;
        //     var s = 1.0;
        //     var stepY = 1.0 / cubeWidth;
        //     var stepX = 360.0 / cubeWidth;
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     for (var y = 0; y < cubeWidth; y++)
        //     {
        //         var h = 0.0;
        //         for (var x = 0; x < cubeWidth; x++)
        //         {
        //             var hsb = new Hsb
        //             {
        //                 H = h,
        //                 S = s,
        //                 B = v
        //             };
        //             var rgb = hsb.To<Rgb>();
        //             unsafe
        //             {
        //                 var pBackBuffer = (int) img.BackBuffer;
        //                 pBackBuffer += y * stride;
        //                 pBackBuffer += x * 4;
        //                 var colorData = (byte) rgb.R << 16;
        //                 colorData |= (byte) rgb.G << 8;
        //                 colorData |= (byte) rgb.B;
        //                 *(int*) pBackBuffer = colorData;
        //             }
        //
        //             h += stepX;
        //         }
        //
        //         s -= stepY;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, cubeWidth, cubeWidth));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap CubeS1(int cubeWidth, double columnY)
        // {
        //     var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
        //     var s = 1.0 - columnY;
        //     var v = 1.0;
        //     var stepY = 1.0 / cubeWidth;
        //     var stepX = 360.0 / cubeWidth;
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     for (var y = 0; y < cubeWidth; y++)
        //     {
        //         var h = 0.0;
        //         for (var x = 0; x < cubeWidth; x++)
        //         {
        //             var hsb = new Hsb
        //             {
        //                 H = h,
        //                 S = s,
        //                 B = v
        //             };
        //             var rgb = hsb.To<Rgb>();
        //             unsafe
        //             {
        //                 var pBackBuffer = (int) img.BackBuffer;
        //                 pBackBuffer += y * stride;
        //                 pBackBuffer += x * 4;
        //                 var colorData = (byte) rgb.R << 16;
        //                 colorData |= (byte) rgb.G << 8;
        //                 colorData |= (byte) rgb.B;
        //                 *(int*) pBackBuffer = colorData;
        //             }
        //
        //             h += stepX;
        //         }
        //
        //         v -= stepY;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, cubeWidth, cubeWidth));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap CubeH1(int cubeWidth, double columnY)
        // {
        //     var img = new WriteableBitmap(cubeWidth, cubeWidth, 96, 96, PixelFormats.Bgr32, null);
        //     var h = 360.0 - columnY * 360.0;
        //     var v = 1.0;
        //     var step = 1.0 / cubeWidth;
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     for (var y = 0; y < cubeWidth; y++)
        //     {
        //         var s = 0.0;
        //         for (var x = 0; x < cubeWidth; x++)
        //         {
        //             var hsb = new Hsb
        //             {
        //                 H = h,
        //                 S = s,
        //                 B = v
        //             };
        //             var rgb = hsb.To<Rgb>();
        //             unsafe
        //             {
        //                 var pBackBuffer = (int) img.BackBuffer;
        //                 pBackBuffer += y * stride;
        //                 pBackBuffer += x * 4;
        //
        //                 var colorData = (byte) rgb.R << 16;
        //                 colorData |= (byte) rgb.G << 8;
        //                 colorData |= (byte) rgb.B;
        //
        //                 *(int*) pBackBuffer = colorData;
        //             }
        //
        //             s += step;
        //         }
        //
        //         v -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, cubeWidth, cubeWidth));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }

        #endregion
    }
}