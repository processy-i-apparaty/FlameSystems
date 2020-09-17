using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorMine.ColorSpaces;
using ConsoleWpfApp.FlameMath;
using FlameBase.Enums;

namespace ConsoleWpfApp.Pickers.Models
{
    public static class ColorPickerColumnModel
    {
        private static int _cores;

        public static WriteableBitmap Get(Point cubeXy, ColorPickerMode colorMode, int columnWidth,
            int columnHeight)
        {
            _cores = Environment.ProcessorCount;
            switch (colorMode)
            {
                case ColorPickerMode.H:
                    return GetColumnH(columnWidth, columnHeight);
                case ColorPickerMode.S:
                    return GetColumnS(columnWidth, columnHeight, cubeXy);
                case ColorPickerMode.V:
                    return GetColumnV(columnWidth, columnHeight, cubeXy);
                case ColorPickerMode.R:
                    return GetColumnR(columnWidth, columnHeight, cubeXy);
                case ColorPickerMode.G:
                    return GetColumnG(columnWidth, columnHeight, cubeXy);
                case ColorPickerMode.B:
                    return GetColumnB(columnWidth, columnHeight, cubeXy);
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
            }
        }

        private static WriteableBitmap GetColumnB(int width, int height, Point cubePoint)
        {
            Algebra.DividePlane(width, height, _cores, out var dic);
            var length = dic.Count;
            var stride = width * 4;
            var a = new byte[stride * height];
            var step = 255.0 / height;

            var g = (byte)Math.Round(255.0 - cubePoint.Y * 255.0);
            var r = (byte)Math.Round(cubePoint.X * 255.0);

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                    for (var x = d[0]; x < d[0] + d[2]; x++)
                    {
                        var n = y * str + x * 4;
                        a[n] = (byte)Math.Round(255.0 - stp * y);
                        a[n + 1] = g;
                        a[n + 2] = r;
                        a[n + 3] = 255;
                    }
            });
            var rect = new Int32Rect(0, 0, width, height);
            var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        private static WriteableBitmap GetColumnG(int width, int height, Point cubePoint)
        {
            Algebra.DividePlane(width, height, _cores, out var dic);
            var length = dic.Count;
            var stride = width * 4;
            var a = new byte[stride * height];
            var step = 255.0 / height;

            var r = (byte)Math.Round(255.0 - cubePoint.Y * 255.0);
            var b = (byte)Math.Round(cubePoint.X * 255.0);

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
                        a[n + 1] = (byte)Math.Round(255.0 - stp * y);
                        a[n + 2] = r;
                        a[n + 3] = 255;
                    }
            });
            var rect = new Int32Rect(0, 0, width, height);
            var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        private static WriteableBitmap GetColumnR(int width, int height, Point cubePoint)
        {
            Algebra.DividePlane(width, height, _cores, out var dic);
            var length = dic.Count;
            var stride = width * 4;
            var a = new byte[stride * height];
            var step = 255.0 / height;

            var g = (byte)Math.Round(255.0 - cubePoint.Y * 255.0);
            var b = (byte)Math.Round(cubePoint.X * 255.0);

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
                        a[n + 1] = g;
                        a[n + 2] = (byte)Math.Round(255.0 - stp * y);
                        a[n + 3] = 255;
                    }
            });
            var rect = new Int32Rect(0, 0, width, height);
            var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        private static WriteableBitmap GetColumnV(int width, int height, Point cubePoint)
        {
            Algebra.DividePlane(width, height, _cores, out var dic);
            var length = dic.Count;
            var stride = width * 4;
            var a = new byte[stride * height];
            var step = 1.0 / height;

            var h = cubePoint.X * 360.0;
            var s = 1.0 - cubePoint.Y;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                    for (var x = d[0]; x < d[0] + d[2]; x++)
                    {
                        var n = y * str + x * 4;
                        var hsb = new Hsb
                        {
                            H = h,
                            S = s,
                            B = 1.0 - stp * y
                        };
                        var rgb = hsb.To<Rgb>();
                        a[n] = (byte)Math.Round(rgb.B);
                        a[n + 1] = (byte)Math.Round(rgb.G);
                        a[n + 2] = (byte)Math.Round(rgb.R);
                        a[n + 3] = 255;
                    }
            });
            var rect = new Int32Rect(0, 0, width, height);
            var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        private static WriteableBitmap GetColumnS(int width, int height, Point cubePoint)
        {
            Algebra.DividePlane(width, height, _cores, out var dic);
            var length = dic.Count;
            var stride = width * 4;
            var a = new byte[stride * height];
            var step = 1.0 / height;

            var h = cubePoint.X * 360.0;
            var b = 1.0 - cubePoint.Y;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                    for (var x = d[0]; x < d[0] + d[2]; x++)
                    {
                        var n = y * str + x * 4;
                        var hsb = new Hsb
                        {
                            H = h,
                            S = 1.0 - stp * y,
                            B = b
                        };
                        var rgb = hsb.To<Rgb>();
                        a[n] = (byte)Math.Round(rgb.B);
                        a[n + 1] = (byte)Math.Round(rgb.G);
                        a[n + 2] = (byte)Math.Round(rgb.R);
                        a[n + 3] = 255;
                    }
            });
            var rect = new Int32Rect(0, 0, width, height);
            var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        private static WriteableBitmap GetColumnH(int width, int height)
        {
            Algebra.DividePlane(width, height, _cores, out var dic);
            var length = dic.Count;
            var stride = width * 4;
            var a = new byte[stride * height];
            var step = 1.0 / height * 360.0;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                var str = stride;
                var stp = step;
                for (var y = d[1]; y < d[1] + d[3]; y++)
                    for (var x = d[0]; x < d[0] + d[2]; x++)
                    {
                        var n = y * str + x * 4;
                        var hsb = new Hsb
                        {
                            H = 360.0 - y * stp,
                            S = 1.0,
                            B = 1.0
                        };
                        var rgb = hsb.To<Rgb>();
                        a[n] = (byte)Math.Round(rgb.B);
                        a[n + 1] = (byte)Math.Round(rgb.G);
                        a[n + 2] = (byte)Math.Round(rgb.R);
                        a[n + 3] = 255;
                    }
            });
            var rect = new Int32Rect(0, 0, width, height);
            var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            img.Lock();
            img.WritePixels(rect, a, stride, 0);
            img.Unlock();
            img.Freeze();
            return img;
        }

        #region one thread

        // private static WriteableBitmap GetColumnB1(int width, int height, Point cubePoint)
        // {
        //     var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //
        //     var g = 255.0 - cubePoint.Y * 255.0;
        //     var r = cubePoint.X * 255.0;
        //     var b = 255.0;
        //     var step = 255.0 / height;
        //     for (var y = 0; y < height; y++)
        //     {
        //         for (var x = 0; x < width; x++)
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
        //         }
        //
        //         b -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, width, height));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap GetColumnG1(int width, int height, Point cubePoint)
        // {
        //     var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     var r = 255.0 - cubePoint.Y * 255.0;
        //     var b = cubePoint.X * 255.0;
        //     var g = 255.0;
        //     var step = 255.0 / height;
        //     for (var y = 0; y < height; y++)
        //     {
        //         for (var x = 0; x < width; x++)
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
        //         }
        //
        //         g -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, width, height));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap GetColumnR1(int width, int height, Point cubePoint)
        // {
        //     var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     var g = 255.0 - cubePoint.Y * 255.0;
        //     var b = cubePoint.X * 255.0;
        //     var r = 255.0;
        //     var step = 255.0 / height;
        //     for (var y = 0; y < height; y++)
        //     {
        //         for (var x = 0; x < width; x++)
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
        //         }
        //
        //         r -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, width, height));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        //
        // private static WriteableBitmap GetColumnV1(int width, int height, Point cubePoint)
        // {
        //     var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     var h = cubePoint.X * 360.0;
        //     var s = 1.0 - cubePoint.Y;
        //     var b = 1.0;
        //     var step = 1.0 / height;
        //     for (var y = 0; y < height; y++)
        //     {
        //         var hsb = new Hsb
        //         {
        //             H = h,
        //             S = s,
        //             B = b
        //         };
        //         var rgb = hsb.To<Rgb>();
        //         for (var x = 0; x < width; x++)
        //         {
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
        //         }
        //
        //         b -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, width, height));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        //
        // private static WriteableBitmap GetColumnS1(int width, int height, Point cubePoint)
        // {
        //     var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     var h = cubePoint.X * 360.0;
        //     var b = 1.0 - cubePoint.Y;
        //     var s = 1.0;
        //     var step = 1.0 / height;
        //     for (var y = 0; y < height; y++)
        //     {
        //         var hsb = new Hsb
        //         {
        //             H = h,
        //             S = s,
        //             B = b
        //         };
        //         var rgb = hsb.To<Rgb>();
        //         for (var x = 0; x < width; x++)
        //         {
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
        //         }
        //
        //         s -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, width, height));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }
        //
        // private static WriteableBitmap GetColumnH1(int width, int height)
        // {
        //     var img = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        //     var stride = img.BackBufferStride;
        //     img.Lock();
        //     var d = 360.0;
        //     var step = 1.0 / height * 360.0;
        //     for (var y = 0; y < height; y++)
        //     {
        //         var hsb = new Hsb
        //         {
        //             H = d,
        //             S = 1.0,
        //             B = 1.0
        //         };
        //         var rgb = hsb.To<Rgb>();
        //         for (var x = 0; x < width; x++)
        //         {
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
        //         }
        //
        //         d -= step;
        //     }
        //
        //     img.AddDirtyRect(new Int32Rect(0, 0, width, height));
        //     img.Unlock();
        //     img.Freeze();
        //     return img;
        // }

        #endregion
    }
}