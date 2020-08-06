using System;
using System.Collections.Generic;

namespace FlameBase.FlameMath
{
    public static class Algebra
    {
        public static double Map(double value, double iStart, double iStop, double oStart, double oStop)
        {
            return oStart + (oStop - oStart) * ((value - iStart) / (iStop - iStart));
        }

        public static void DividePlane(int imgWidth, int imgHeight, int parts, out Dictionary<int, short[]> dic)
        {
            var w = 1;
            var h = 1;
            var min = double.MaxValue;
            for (var dw = 1; dw <= parts; dw++)
            {
                if (dw <= 1 || parts % dw != 0) continue;
                var dh = parts / dw;
                var arr = new[] {1.0 * imgWidth / dw, 1.0 * imgHeight / dh};
                Array.Sort(arr);
                var d = arr[1] / arr[0];
                if (d > min) continue;
                w = dw;
                h = dh;
                min = d;
            }

            var wr = imgWidth % w;
            var hr = imgHeight % h;

            dic = new Dictionary<int, short[]>();
            var i = 0;
            var width = imgWidth / w;
            var height = imgHeight / h;

            for (var dw = 0; dw < w; dw++)
            {
                var x = (short) (dw * width);
                var ww = width;
                if (dw == w - 1) ww += wr;
                for (var dh = 0; dh < h; dh++)
                {
                    var y = (short) (dh * height);
                    var hh = height;
                    if (dh == h - 1) hh += hr;
                    dic.Add(i, new[] {x, y, (short) ww, (short) hh});
                    i++;
                }
            }
        }
    }
}