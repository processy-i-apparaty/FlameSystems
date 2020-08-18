using System;
using System.Windows;

namespace FlameBase.FlameMath
{
    public static class Trigonometry
    {
        public const double ToDegrees = 180.0 / Math.PI;
        public const double ToRadians = Math.PI / 180.0;
        public const double TwoPi = Math.PI * 2.0;

        public static void TranslatePoint(double pX, double pY, double[,] m, double cX, double cY, out double tpX, out double tpY)
        {
            var dx = m[0, 0] * pX + m[0, 1] * pY + m[0, 2];
            var dy = m[1, 0] * pX + m[1, 1] * pY + m[1, 2];
            tpX = dx + cX;
            tpY = dy + cY;
        }

        public static void RotatePoint(ref double pX, ref double pY, double cos, double sin)
        {
            var dx = pX * cos - pY * sin;
            var dy = pY * cos + pX * sin;
            pX = dx;
            pY = dy;
        }

        public static bool InRange(double x, double y, Size size)
        {
            return x >= 0 && y >= 0 && x < size.Width && y < size.Height;
        }
    }
}