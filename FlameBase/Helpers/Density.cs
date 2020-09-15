using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FlameBase.FlameMath;
using FlameBase.RenderMachine.Models;

namespace FlameBase.Helpers
{
    public static class Density
    {
        public static void Estimation(LogDisplayModel display, double minKernelRadius, double maxKernelRadius,
            double alpha,
            out LogDisplayModel newDisplay)
        {
            Debug.WriteLine($"Estimation START");
            var maxShots = display.Max;
            var avgShots = display.GetAvgShots();
            var array = display.GetArrayCopy();
            var newArray = new uint[array.GetLength(0), array.GetLength(1), display.ColorCount];
            var width = display.Width;
            var height = display.Height;

            Algebra.DividePlane(width, height, Environment.ProcessorCount, out var dic);
            var length = dic.Count;

            const double gamma = 1.0 / 1.1;

            Parallel.For(0, length, i =>
            {
                var d = dic[i];
                for (var y = d[1]; y < d[1] + d[3]; y++)
                for (var x = d[0]; x < d[0] + d[2]; x++)
                {
                    var countShots = CountShots(array, x, y);
                    // var kernelWidth = (int) maxKernelRadius;
                    var kernelWidth = 0;

                    if (countShots > 0)
                    {
                        // var density = 1.0 - Math.Pow(Math.Log(countShots + 1.0, maxShots), gamma);
                        //var kernelWidth = GetKernelWidth(density, maxKernelRadius, minKernelRadius, alpha);
                        //var density = maxShots / countShots;
                        //kernelWidth = (int) (maxKernelRadius / Math.Pow(countShots, alpha));


                        var density = 1.0 - Math.Pow(Math.Log(countShots + 1.0, avgShots), gamma);
                        kernelWidth = (int) Math.Round(density * maxKernelRadius);
                        kernelWidth = Math.Max(0, kernelWidth);
                    }

                    if (kernelWidth > 0)
                    {
                        const double weight = 1.2;
                        var kernel = GetKernel(kernelWidth, weight);
                        var k = K(kernel, array, x, y);
                        Copy(x, y, k, newArray);
                    }
                    else
                    {
                        Copy(x, y, array, newArray);
                    }
                }
            });

            newDisplay = new LogDisplayModel(newArray, display.BackColor) {RenderColorMode = display.RenderColorMode};
            Debug.WriteLine($"Estimation END");
        }

        private static void Copy(int x, int y, IReadOnlyList<uint> from, uint[,,] to)
        {
            for (var i = 0; i < from.Count; i++) to[x, y, i] = from[i];
        }

        private static uint[] K(double[,] kernel, uint[,,] array, int x, int y)
        {
            var colorsCount = array.GetLength(2);
            var ret = new uint[colorsCount];
            var width = array.GetLength(0);
            var height = array.GetLength(1);
            var kernelWidth = kernel.GetLength(0);

            for (var ky = 0; ky < kernelWidth; ky++)
            {
                var dy = y - kernelWidth + ky;
                if (dy < 0 || dy >= height) continue;
                for (var kx = 0; kx < kernelWidth; kx++)
                {
                    var dx = x - kernelWidth + kx;
                    if (dx < 0 || dx >= width) continue;
                    var k = kernel[kx, ky];
                    for (var c = 0; c < colorsCount; c++)
                        ret[c] += (uint) (k * array[dx, dy, c]);
                }
            }

            return ret;
        }


        private static void Copy(int x, int y, uint[,,] from, uint[,,] to)
        {
            for (var i = 0; i < from.GetLength(2); i++) to[x, y, i] = from[x, y, i];
        }

        private static uint CountShots(uint[,,] array, int x, int y)
        {
            var colorCount = array.GetLength(2);
            uint count = 0;
            for (var i = 0; i < colorCount; i++) count += array[x, y, i];
            return count;
        }


        private static int GetKernelWidth(double density, double maxKernelRadius, double alpha)
        {
            var kernelWidth = (int) (maxKernelRadius / Math.Pow(density, alpha));
            return kernelWidth;
        }

        private static double GetKernelWidth2(double density, double maxRadius, double minRadius, double alpha,
            bool round = true)
        {
            //KernelWidth = MaxKernelRadius / (Density ^ Alpha) // less alpha, more kernel
            var width = maxRadius * Math.Pow(1.0 - density, alpha);
            if (width < minRadius || double.IsInfinity(width) || double.IsNaN(width)) width = minRadius;
            return round ? Math.Round(width, 0) : width;
        }

        private static double[,] GetKernel(int n, double weight)
        {
            var length = n * 2 + 1;
            var kernel = new double[length, length];
            double sumTotal = 0;

            var kernelRadius = length / 2;

            var calculatedEuler = 1.0 / (2.0 * Math.PI * Math.Pow(weight, 2));


            for (var filterY = -kernelRadius;
                filterY <= kernelRadius;
                filterY++)
            for (var filterX = -kernelRadius;
                filterX <= kernelRadius;
                filterX++)
            {
                var distance = (filterX * filterX +
                                filterY * filterY) /
                               (2 * (weight * weight));


                kernel[filterY + kernelRadius,
                        filterX + kernelRadius] =
                    calculatedEuler * Math.Exp(-distance);


                sumTotal += kernel[filterY + kernelRadius,
                    filterX + kernelRadius];
            }


            for (var y = 0; y < length; y++)
            for (var x = 0; x < length; x++)
                kernel[y, x] = kernel[y, x] *
                               (1.0 / sumTotal);


            return kernel;
        }
    }
}