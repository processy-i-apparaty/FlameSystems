using System;

namespace FlameBase.FlameMath
{
    public class G
    {
        public static vec2 Reflect(vec2 vec2, vec2 vec3)
        {
            return vec2.minus(vec3.multiply(2.0 * dot(vec2, vec3)));
        }

        public static vec3 Reflect(vec3 vec3, vec3 vec4)
        {
            return vec3.minus(vec4.multiply(2.0 * dot(vec3, vec4)));
        }

        public static vec2 refract(vec2 vec2, vec2 vec3, double n)
        {
            var vec4 = new vec2(0.0);
            var a = 1.0 - n * n * (1.0 - dot(vec3, vec2) * dot(vec3, vec2));
            vec2 minus;
            if (a < 0.0)
                minus = new vec2(0.0);
            else
                minus = vec2.multiply(n).minus(vec3.multiply(n * dot(vec3, vec2) + Math.Sqrt(a)));
            return minus;
        }

        public static vec3 refract(vec3 vec3, vec3 vec4, double n)
        {
            var vec5 = new vec3(0.0);
            var a = 1.0 - n * n * (1.0 - dot(vec4, vec3) * dot(vec4, vec3));
            vec3 minus;
            if (a < 0.0)
                minus = new vec3(0.0);
            else
                minus = vec3.multiply(n).minus(vec4.multiply(n * dot(vec4, vec3) + Math.Sqrt(a)));
            return minus;
        }

        // public static float invSqrt(float intBitsToFloat)
        // {
        //     var n = 0.5f * intBitsToFloat;
        //     intBitsToFloat = Float.intBitsToFloat(1597463007 - (Float.floatToIntBits(intBitsToFloat) >> 1));
        //     intBitsToFloat *= 1.5f - n * intBitsToFloat * intBitsToFloat;
        //     return intBitsToFloat;
        // }
        //
        // public static double invSqrt(double longBitsToDouble)
        // {
        //     var n = 0.5 * longBitsToDouble;
        //     longBitsToDouble =
        //         double.longBitsToDouble(6910470738111508698L - (double.doubleToLongBits(longBitsToDouble) >> 1));
        //     longBitsToDouble *= 1.5 - n * longBitsToDouble * longBitsToDouble;
        //     return longBitsToDouble;
        // }

        public static double atan(double n, double n2)
        {
            return Math.Atan(n / n2);
        }

        public static double atan2(double a, double n)
        {
            var n2 = 0.7853981633974483;
            var n3 = 3.0 * n2;
            var abs = Math.Abs(a);
            double n4;
            if (n >= 0.0)
                n4 = n2 - n2 * ((n - abs) / (n + abs));
            else
                n4 = n3 - n2 * ((n + abs) / (abs - n));
            return a < 0.0 ? -n4 : n4;
        }

        public static double sqrt(double a)
        {
            return Math.Sqrt(a);
        }

        public static vec2 sqrt(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = Math.Sqrt(vec2.x);
            vec3.y = Math.Sqrt(vec2.y);
            return vec3;
        }

        public static vec3 sqrt(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = Math.Sqrt(vec3.x);
            vec4.y = Math.Sqrt(vec3.y);
            vec4.z = Math.Sqrt(vec3.z);
            return vec4;
        }

        public static vec4 sqrt(vec4 vec4)
        {
            var vec5 = new vec4(0.0);
            vec5.x = Math.Sqrt(vec4.x);
            vec5.y = Math.Sqrt(vec4.y);
            vec5.z = Math.Sqrt(vec4.z);
            vec5.w = Math.Sqrt(vec4.w);
            return vec5;
        }

        public static double Length(vec2 vec2)
        {
            return Math.Sqrt(vec2.x * vec2.x + vec2.y * vec2.y);
        }

        public static double Length(vec3 vec3)
        {
            return Math.Sqrt(vec3.x * vec3.x + vec3.y * vec3.y + vec3.z * vec3.z);
        }

        public static double Length(vec4 vec4)
        {
            return Math.Sqrt(vec4.x * vec4.x + vec4.y * vec4.y + vec4.z * vec4.z + vec4.w * vec4.w);
        }

        public static double abs(double n)
        {
            return n >= 0.0 ? n : -n;
        }

        public static vec2 abs(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = vec2.x >= 0.0 ? vec2.x : -vec2.x;
            vec3.y = vec2.y >= 0.0 ? vec2.y : -vec2.y;
            return vec3;
        }

        public static vec3 abs(vec3 vec3)
        {
            return new vec3(vec3.x >= 0.0 ? vec3.x : -vec3.x, vec3.y >= 0.0 ? vec3.y : -vec3.y,
                vec3.z >= 0.0 ? vec3.z : -vec3.z);
        }

        public static vec4 abs(vec4 vec4)
        {
            return new vec4(vec4.x >= 0.0 ? vec4.x : -vec4.x, vec4.y >= 0.0 ? vec4.y : -vec4.y,
                vec4.z >= 0.0 ? vec4.z : -vec4.z, vec4.w >= 0.0 ? vec4.w : -vec4.w);
        }

        public static double sign(double d)
        {
            return Math.Sign(d);
        }

        public static vec2 sign(vec2 vec2)
        {
            return new vec2(Math.Sign(vec2.x), Math.Sign(vec2.y));
        }

        public static vec3 sign(vec3 vec3)
        {
            return new vec3(Math.Sign(vec3.x), Math.Sign(vec3.y), Math.Sign(vec3.z));
        }

        public static double distance(vec2 vec2, vec2 vec3)
        {
            var minus = vec2.minus(vec3);
            return Math.Sqrt(minus.x * minus.x + minus.y * minus.y);
        }

        public static double distance(vec3 vec3, vec3 vec4)
        {
            var minus = vec3.minus(vec4);
            return Math.Sqrt(minus.x * minus.x + minus.y * minus.y + minus.z * minus.z);
        }

        public static double distance(vec4 vec4, vec4 vec5)
        {
            vec4 minus = vec4.minus(vec5);
            return Math.Sqrt(minus.x * minus.x + minus.y * minus.y + minus.z * minus.z + minus.w * minus.w);
        }

        public static vec2 normalize(vec2 vec2)
        {
            return new vec2(vec2.x / Length(vec2), vec2.y / Length(vec2));
        }

        public static vec3 normalize(vec3 vec3)
        {
            return new vec3(vec3.x / Length(vec3), vec3.y / Length(vec3), vec3.z / Length(vec3));
        }

        public static vec4 normalize(vec4 vec4)
        {
            return new vec4(vec4.x / Length(vec4), vec4.y / Length(vec4), vec4.z / Length(vec4), vec4.w / Length(vec4));
        }

        public static double pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        public static vec2 pow(vec2 vec2, vec2 vec3)
        {
            return new vec2(Math.Pow(vec2.x, vec3.x), Math.Pow(vec2.y, vec3.y));
        }

        public static vec3 pow(vec3 vec3, vec3 vec4)
        {
            return new vec3(Math.Pow(vec3.x, vec4.x), Math.Pow(vec3.y, vec4.y), Math.Pow(vec3.z, vec4.z));
        }

        public static vec4 pow(vec4 vec4, vec4 vec5)
        {
            return new vec4(Math.Pow(vec4.x, vec5.x), Math.Pow(vec4.y, vec5.y), Math.Pow(vec4.z, vec5.z),
                Math.Pow(vec4.w, vec5.w));
        }

        public static double Exp(double a)
        {
            return Math.Exp(a);
        }

        public static vec2 Exp(vec2 vec2)
        {
            return new vec2(Math.Exp(vec2.x), Math.Exp(vec2.y));
        }

        public static vec3 Exp(vec3 vec3)
        {
            return new vec3(Math.Exp(vec3.x), Math.Exp(vec3.y), Math.Exp(vec3.z));
        }

        public static vec4 Exp(vec4 vec4)
        {
            return new vec4(Math.Exp(vec4.x), Math.Exp(vec4.y), Math.Exp(vec4.z), Math.Exp(vec4.w));
        }

        public static double Clamp(double a, double b, double b2)
        {
            return Math.Min(Math.Max(a, b), b2);
        }

        public static vec2 Clamp(vec2 vec2, double n, double n2)
        {
            return new vec2(Math.Min(Math.Max(vec2.x, n), n2), Math.Min(Math.Max(vec2.y, n), n2));
        }

        public static vec3 Clamp(vec3 vec3, double b, double b2)
        {
            return new vec3(Math.Min(Math.Max(vec3.x, b), b2), Math.Min(Math.Max(vec3.y, b), b2),
                Math.Min(Math.Max(vec3.z, b), b2));
        }

        public static vec4 Clamp(vec4 vec4, double n, double n2)
        {
            return new vec4(Math.Min(Math.Max(vec4.x, n), n2), Math.Min(Math.Max(vec4.y, n), n2),
                Math.Min(Math.Max(vec4.z, n), n2), Math.Min(Math.Max(vec4.w, n), n2));
        }

        public static double Mix(double n, double n2, double n3)
        {
            return n * (1.0 - n3) + n2 * n3;
        }

        public static vec2 Mix(vec2 vec2, vec2 vec3, double n)
        {
            var vec4 = new vec2(0.0, 0.0);
            vec4.x = vec2.x * (1.0 - n) + vec3.x * n;
            vec4.y = vec2.y * (1.0 - n) + vec3.y * n;
            return vec4;
        }

        public static vec3 Mix(vec3 vec3, vec3 vec4, double n)
        {
            var vec5 = new vec3(0.0, 0.0, 0.0);
            vec5.x = vec3.x * (1.0 - n) + vec4.x * n;
            vec5.y = vec3.y * (1.0 - n) + vec4.y * n;
            vec5.z = vec3.z * (1.0 - n) + vec4.z * n;
            return vec5;
        }

        public static vec4 Mix(vec4 vec4, vec4 vec5, double n)
        {
            return new vec4(vec4.x * (1.0 - n) + vec5.x * n, vec4.y * (1.0 - n) + vec5.y * n,
                vec4.z * (1.0 - n) + vec5.z * n, vec4.w * (1.0 - n) + vec5.w * n);
        }

        public static double log2(double a)
        {
            return Math.Log(a) / Math.Log(2.0);
        }

        public static double smoothstep(double n, double n2, double n3)
        {
            double clamp = Clamp((n3 - n) / (n2 - n), 0.0, 1.0);
            return clamp * clamp * (3.0 - 2.0 * clamp);
        }

        public static vec2 smoothstep(double n, double n2, vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = smoothstep(n, n2, vec2.x);
            vec3.y = smoothstep(n, n2, vec2.y);
            return vec3;
        }

        public static vec2 smoothstep(vec2 vec2, vec2 vec3, vec2 vec4)
        {
            var vec5 = new vec2(0.0);
            vec5.x = smoothstep(vec2.x, vec3.x, vec4.x);
            vec5.y = smoothstep(vec2.y, vec3.y, vec4.y);
            return vec5;
        }

        public static vec3 smoothstep(double n, double n2, vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = smoothstep(n, n2, vec3.x);
            vec4.y = smoothstep(n, n2, vec3.y);
            vec4.z = smoothstep(n, n2, vec3.z);
            return vec4;
        }

        public static vec3 smoothstep(vec3 vec3, vec3 vec4, vec3 vec5)
        {
            var vec6 = new vec3(0.0);
            vec6.x = smoothstep(vec3.x, vec4.x, vec5.x);
            vec6.y = smoothstep(vec3.y, vec4.y, vec5.y);
            vec6.z = smoothstep(vec3.z, vec4.z, vec5.z);
            return vec6;
        }

        public static vec4 smoothstep(double n, double n2, vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = smoothstep(n, n2, vec4.x);
            vec5.y = smoothstep(n, n2, vec4.y);
            vec5.z = smoothstep(n, n2, vec4.z);
            vec5.w = smoothstep(n, n2, vec4.w);
            return vec5;
        }

        public static vec4 smoothstep(vec4 vec4, vec4 vec5, vec4 vec6)
        {
            vec4 vec7 = new vec4(0.0);
            vec7.x = smoothstep(vec4.x, vec5.x, vec6.x);
            vec7.y = smoothstep(vec4.y, vec5.y, vec6.y);
            vec7.z = smoothstep(vec4.z, vec5.z, vec6.z);
            vec7.w = smoothstep(vec4.w, vec5.w, vec6.w);
            return vec7;
        }

        public static double dot(double n, double n2)
        {
            return n * n2;
        }

        public static double dot(vec2 vec2, vec2 vec3)
        {
            return vec2.x * vec3.x + vec2.y * vec3.y;
        }

        public static double dot(vec3 vec3, vec3 vec4)
        {
            return vec3.x * vec4.x + vec3.y * vec4.y + vec3.z * vec4.z;
        }

        public static double dot(vec4 vec4, vec4 vec5)
        {
            return vec4.x * vec5.x + vec4.y * vec5.y + vec4.z * vec5.z + vec4.w * vec5.w;
        }

        public static vec3 cross(vec3 vec3, vec3 vec4)
        {
            return new vec3(vec3.y * vec4.z - vec4.y * vec3.z, vec3.z * vec4.x - vec4.z * vec3.x,
                vec3.x * vec4.y - vec4.x * vec3.y);
        }

        public static double mod(double n, double n2)
        {
            return n - n2 * Math.Floor(n / n2);
        }

        public static vec2 mod(vec2 vec2, double n)
        {
            var vec3 = new vec2(0.0);
            vec3.x = vec2.x - n * Math.Floor(vec2.x / n);
            vec3.y = vec2.y - n * Math.Floor(vec2.y / n);
            return vec3;
        }

        public static vec2 mod(vec2 vec2, vec2 vec3)
        {
            var vec4 = new vec2(0.0);
            vec4.x = vec2.x - vec3.x * Math.Floor(vec2.x / vec3.x);
            vec4.y = vec2.y - vec3.y * Math.Floor(vec2.y / vec3.y);
            return vec4;
        }

        public static vec3 mod(vec3 vec3, double n)
        {
            var vec4 = new vec3(0.0);
            vec4.x = vec3.x - n * Math.Floor(vec3.x / n);
            vec4.y = vec3.y - n * Math.Floor(vec3.y / n);
            vec4.z = vec3.z - n * Math.Floor(vec3.z / n);
            return vec4;
        }

        public static vec3 mod(vec3 vec3, vec3 vec4)
        {
            var vec5 = new vec3(0.0);
            vec5.x = vec3.x - vec4.x * Math.Floor(vec3.x / vec4.x);
            vec5.y = vec3.y - vec4.y * Math.Floor(vec3.y / vec4.y);
            vec5.z = vec3.z - vec4.z * Math.Floor(vec3.z / vec4.z);
            return vec5;
        }

        public static vec4 mod(vec4 vec4, double n)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = vec4.x - n * Math.Floor(vec4.x / n);
            vec5.y = vec4.y - n * Math.Floor(vec4.y / n);
            vec5.z = vec4.z - n * Math.Floor(vec4.z / n);
            vec5.w = vec4.w - n * Math.Floor(vec4.w / n);
            return vec5;
        }

        public static double step(double n, double n2)
        {
            if (n2 < n) return 0.0;
            return 1.0;
        }

        public static vec2 step(double n, vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            if (vec2.x < n)
                vec3.x = 0.0;
            else
                vec3.x = 1.0;
            if (vec2.y < n)
                vec3.y = 0.0;
            else
                vec3.y = 1.0;
            return vec3;
        }

        public static vec2 step(vec2 vec2, vec2 vec3)
        {
            var vec4 = new vec2(0.0);
            if (vec3.x < vec2.x)
                vec4.x = 0.0;
            else
                vec4.x = 1.0;
            if (vec3.y < vec2.y)
                vec4.y = 0.0;
            else
                vec4.y = 1.0;
            return vec4;
        }

        public static vec3 step(double n, vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            if (vec3.x < n)
                vec4.x = 0.0;
            else
                vec4.x = 1.0;
            if (vec3.y < n)
                vec4.y = 0.0;
            else
                vec4.y = 1.0;
            if (vec3.z < n)
                vec4.z = 0.0;
            else
                vec4.z = 1.0;
            return vec4;
        }

        public static vec3 step(vec3 vec3, vec3 vec4)
        {
            var vec5 = new vec3(0.0);
            if (vec4.x < vec3.x)
                vec5.x = 0.0;
            else
                vec5.x = 1.0;
            if (vec4.y < vec3.y)
                vec5.y = 0.0;
            else
                vec5.y = 1.0;
            if (vec4.z < vec3.z)
                vec5.z = 0.0;
            else
                vec5.z = 1.0;
            return vec5;
        }

        public static vec4 step(vec4 vec4, vec4 vec5)
        {
            vec4 vec6 = new vec4(0.0);
            if (vec5.x < vec4.x)
                vec6.x = 0.0;
            else
                vec6.x = 1.0;
            if (vec5.y < vec4.y)
                vec6.y = 0.0;
            else
                vec6.y = 1.0;
            if (vec5.z < vec4.z)
                vec6.z = 0.0;
            else
                vec6.z = 1.0;
            if (vec5.w < vec4.w)
                vec6.w = 0.0;
            else
                vec6.w = 1.0;
            return vec6;
        }

        public static double Floor(double a)
        {
            return Math.Floor(a);
        }

        public static vec2 Floor(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = Math.Floor(vec2.x);
            vec3.y = Math.Floor(vec2.y);
            return vec3;
        }

        public static vec3 Floor(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = Math.Floor(vec3.x);
            vec4.y = Math.Floor(vec3.y);
            vec4.z = Math.Floor(vec3.z);
            return vec4;
        }

        public static vec4 Floor(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = Math.Floor(vec4.x);
            vec5.y = Math.Floor(vec4.y);
            vec5.z = Math.Floor(vec4.z);
            vec5.w = Math.Floor(vec4.w);
            return vec5;
        }

        public static double trunc(double n)
        {
            return (int) n;
        }

        public static vec2 trunc(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = trunc(vec2.x);
            vec3.y = trunc(vec2.y);
            return vec3;
        }

        public static vec3 trunc(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = trunc(vec3.x);
            vec4.y = trunc(vec3.y);
            vec4.z = trunc(vec3.z);
            return vec4;
        }

        public static vec4 trunc(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = trunc(vec4.x);
            vec5.y = trunc(vec4.y);
            vec5.z = trunc(vec4.z);
            vec5.w = trunc(vec4.w);
            return vec5;
        }

        public static double round(double a)
        {
            return Math.Round(a);
        }

        public static vec2 round(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = round(vec2.x);
            vec3.y = round(vec2.y);
            return vec3;
        }

        public static vec3 round(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = round(vec3.x);
            vec4.y = round(vec3.y);
            vec4.z = round(vec3.z);
            return vec4;
        }

        public static vec4 round(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = round(vec4.x);
            vec5.y = round(vec4.y);
            vec5.z = round(vec4.z);
            vec5.w = round(vec4.w);
            return vec5;
        }

        public static double ceil(double a)
        {
            return Math.Ceiling(a);
        }

        public static vec2 ceil(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = ceil(vec2.x);
            vec3.y = ceil(vec2.y);
            return vec3;
        }

        public static vec3 ceil(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = ceil(vec3.x);
            vec4.y = ceil(vec3.y);
            vec4.z = ceil(vec3.z);
            return vec4;
        }

        public static vec4 ceil(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = ceil(vec4.x);
            vec5.y = ceil(vec4.y);
            vec5.z = ceil(vec4.z);
            vec5.w = ceil(vec4.w);
            return vec5;
        }

        public static double Fract(double a)
        {
            return a - Math.Floor(a);
        }

        public static vec2 Fract(vec2 vec2)
        {
            return new vec2(vec2.x - Math.Floor(vec2.x), vec2.y - Math.Floor(vec2.y));
        }

        public static vec3 Fract(vec3 vec3)
        {
            return new vec3(vec3.x - Math.Floor(vec3.x), vec3.y - Math.Floor(vec3.y), vec3.z - Math.Floor(vec3.z));
        }

        public static vec4 Fract(vec4 vec4)
        {
            return new vec4(vec4.x - Math.Floor(vec4.x), vec4.y - Math.Floor(vec4.y), vec4.z - Math.Floor(vec4.z),
                vec4.w - Math.Floor(vec4.w));
        }

        public static double Min(double n, double n2)
        {
            return n2 < n ? n2 : n;
        }

        public static vec2 Min(vec2 vec2, vec2 vec3)
        {
            var vec4 = new vec2(0.0);
            vec4.x = vec3.x < vec2.x ? vec3.x : vec2.x;
            vec4.y = vec3.y < vec2.y ? vec3.y : vec2.y;
            return vec4;
        }

        public static vec3 Min(vec3 vec3, vec3 vec4)
        {
            var vec5 = new vec3(0.0);
            vec5.x = vec4.x < vec3.x ? vec4.x : vec3.x;
            vec5.y = vec4.y < vec3.y ? vec4.y : vec3.y;
            vec5.z = vec4.z < vec3.z ? vec4.z : vec3.z;
            return vec5;
        }

        public static vec4 Min(vec4 vec4, vec4 vec5)
        {
            vec4 vec6 = new vec4(0.0);
            vec6.x = vec5.x < vec4.x ? vec5.x : vec4.x;
            vec6.y = vec5.y < vec4.y ? vec5.y : vec4.y;
            vec6.z = vec5.z < vec4.z ? vec5.z : vec4.z;
            vec6.w = vec5.w < vec4.w ? vec5.w : vec4.w;
            return vec6;
        }

        public static vec2 Min(vec2 vec2, double n)
        {
            var vec3 = new vec2(0.0);
            vec3.x = n < vec2.x ? n : vec2.x;
            vec3.y = n < vec2.y ? n : vec2.y;
            return vec3;
        }

        public static vec3 Min(vec3 vec3, double n)
        {
            var vec4 = new vec3(0.0);
            vec4.x = n < vec3.x ? n : vec3.x;
            vec4.y = n < vec3.y ? n : vec3.y;
            vec4.z = n < vec3.z ? n : vec3.z;
            return vec4;
        }

        public static vec4 Min(vec4 vec4, double n)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = n < vec4.x ? n : vec4.x;
            vec5.y = n < vec4.y ? n : vec4.y;
            vec5.z = n < vec4.z ? n : vec4.z;
            vec5.w = n < vec4.w ? n : vec4.w;
            return vec5;
        }

        public static double max(double n, double n2)
        {
            return n < n2 ? n2 : n;
        }

        public static vec2 max(vec2 vec2, vec2 vec3)
        {
            var vec4 = new vec2(0.0);
            vec4.x = vec2.x < vec3.x ? vec3.x : vec2.x;
            vec4.y = vec2.y < vec3.y ? vec3.y : vec2.y;
            return vec4;
        }

        public static vec3 max(vec3 vec3, vec3 vec4)
        {
            var vec5 = new vec3(0.0);
            vec5.x = vec3.x < vec4.x ? vec4.x : vec3.x;
            vec5.y = vec3.y < vec4.y ? vec4.y : vec3.y;
            vec5.z = vec3.z < vec4.z ? vec4.z : vec3.z;
            return vec5;
        }

        public static vec4 max(vec4 vec4, vec4 vec5)
        {
            vec4 vec6 = new vec4(0.0);
            vec6.x = vec4.x < vec5.x ? vec5.x : vec4.x;
            vec6.y = vec4.y < vec5.y ? vec5.y : vec4.y;
            vec6.z = vec4.z < vec5.z ? vec5.z : vec4.z;
            vec6.w = vec4.w < vec5.w ? vec5.w : vec4.w;
            return vec6;
        }

        public static vec2 max(vec2 vec2, double n)
        {
            var vec3 = new vec2(0.0);
            vec3.x = vec2.x < n ? n : vec2.x;
            vec3.y = vec2.y < n ? n : vec2.y;
            return vec3;
        }

        public static vec3 max(vec3 vec3, double n)
        {
            var vec4 = new vec3(0.0);
            vec4.x = vec3.x < n ? n : vec3.x;
            vec4.y = vec3.y < n ? n : vec3.y;
            vec4.z = vec3.z < n ? n : vec3.z;
            return vec4;
        }

        public static vec4 max(vec4 vec4, double n)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = vec4.x < n ? n : vec4.x;
            vec5.y = vec4.y < n ? n : vec4.y;
            vec5.z = vec4.z < n ? n : vec4.z;
            vec5.w = vec4.w < n ? n : vec4.w;
            return vec5;
        }

        public static double sin(double a)
        {
            return Math.Sin(a);
        }

        public static vec2 sin(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = Math.Sin(vec2.x);
            vec3.y = Math.Sin(vec2.y);
            return vec3;
        }

        public static vec3 sin(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = Math.Sin(vec3.x);
            vec4.y = Math.Sin(vec3.y);
            vec4.z = Math.Sin(vec3.z);
            return vec4;
        }

        public static vec4 sin(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = Math.Sin(vec4.x);
            vec5.y = Math.Sin(vec4.y);
            vec5.z = Math.Sin(vec4.z);
            vec5.w = Math.Sin(vec4.w);
            return vec5;
        }

        public static double cos(double a)
        {
            return Math.Cos(a);
        }

        public static vec2 cos(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = Math.Cos(vec2.x);
            vec3.y = Math.Cos(vec2.y);
            return vec3;
        }

        public static vec3 cos(vec3 vec3)
        {
            var vec4 = new vec3(0.0);
            vec4.x = Math.Cos(vec3.x);
            vec4.y = Math.Cos(vec3.y);
            vec4.z = Math.Cos(vec3.z);
            return vec4;
        }

        public static vec4 cos(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0);
            vec5.x = Math.Cos(vec4.x);
            vec5.y = Math.Cos(vec4.y);
            vec5.z = Math.Cos(vec4.z);
            vec5.w = Math.Cos(vec4.w);
            return vec5;
        }

        public static double plot(vec2 vec2, double n)
        {
            return smoothstep(n - 0.02, n, vec2.y) - smoothstep(n, n + 0.02, vec2.y);
        }

        public static double circle(vec2 vec2, double n)
        {
            var minus = vec2.minus(new vec2(0.5));
            return 1.0 - smoothstep(n - n * 0.01, n + n * 0.01, dot(minus, minus) * 4.0);
        }

        // public static vec2 rotate2D(vec2 vec2, double n)
        // {
        //     return new mat2(Math.Cos(n), -Math.Sin(n), Math.Sin(n), Math.Cos(n)).times(vec2.minus(new vec2(0.5)))
        //         .plus(new vec2(0.5));
        // }

        public static vec2 tile(vec2 vec2, double n)
        {
            return Fract(vec2.multiply(n));
        }

        public static vec2 brickTile(vec2 vec2, double n)
        {
            vec2 multiply;
            var vec3 = multiply = vec2.multiply(n);
            multiply.x += step(1.0, mod(vec3.y, 2.0)) * 0.5;
            return Fract(vec3);
        }

        public static vec2 truchetPattern(vec2 vec2, double fract)
        {
            fract = Fract((fract - 0.5) * 2.0);
            if (fract > 0.75)
                vec2 = new vec2(1.0).minus(vec2);
            else if (fract > 0.5)
                vec2 = new vec2(1.0 - vec2.x, vec2.y);
            else if (fract > 0.25) vec2 = new vec2(1.0).minus(new vec2(1.0 - vec2.x, vec2.y));
            return vec2;
        }

        public static double box(vec2 vec2, vec2 minus, double n)
        {
            minus = new vec2(0.5).minus(minus.multiply(0.5));
            var vec3 = new vec2(n * 0.5);
            var multiply = smoothstep(minus, minus.plus(vec3), vec2)
                .multiply(smoothstep(minus, minus.plus(vec3), new vec2(1.0).minus(vec2)));
            return multiply.x * multiply.y;
        }

        public static vec2 skew(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = 1.1547 * vec2.x;
            vec3.y = vec2.y + 0.5 * vec3.x;
            return vec3;
        }

        public static vec3 simplexGrid(vec2 vec2)
        {
            var vec3 = new vec3(0.0);
            var vec4 = new vec2(0.0);
            vec2 fract = Fract(skew(vec2));
            if (fract.x > fract.y)
            {
                var minus = new vec2(1.0).minus(new vec2(fract.x, fract.y - fract.x));
                vec3.x = minus.x;
                vec3.y = minus.y;
                vec3.z = fract.y;
            }
            else
            {
                var minus2 = new vec2(1.0).minus(new vec2(fract.x - fract.y, fract.y));
                vec3.y = minus2.x;
                vec3.z = minus2.y;
                vec3.x = fract.x;
            }

            return Fract(vec3);
        }

        public static double Random(vec2 vec2)
        {
            return Fract(Math.Sin(dot(new vec2(vec2.x, vec2.y), new vec2(12.9898, 78.233))) * 43758.5453123);
        }

        public static vec2 Random2(vec2 vec2)
        {
            return Fract(sin(new vec2(dot(vec2, new vec2(127.1, 311.7)), dot(vec2, new vec2(269.5, 183.3))))
                .multiply(43758.5453));
        }

        public static vec3 fractal(vec2 vec2)
        {
            var n = 50.0;
            var plus = new vec2(0.0);
            for (var n2 = 0; n2 < n; ++n2)
            {
                plus = new vec2(plus.x * plus.x - plus.y * plus.y, 2.0 * plus.x * plus.y).plus(vec2);
                if (dot(plus, plus) > 4.0)
                {
                    var n3 = 0.125662 * n2;
                    return new vec3(new vec3(Math.Cos(n3 + 0.9), Math.Cos(n3 + 0.3), Math.Cos(n3 + 0.2)).multiply(0.4)
                        .add(0.6));
                }
            }

            return new vec3(0.0);
        }

        public static vec2 B(vec2 vec2)
        {
            return new vec2(Math.Log(Length(vec2)), atan2(vec2.y, vec2.x) - 6.3);
        }

        public static vec3 F(vec2 vec2, double n)
        {
            var plus = vec2;
            var n2 = 6.0;
            for (int n3 = 30, i = 0; i < n3; ++i)
            {
                plus = B(new vec2(plus.x, Math.Abs(plus.y)))
                    .plus(new vec2(0.1 * Math.Sin(n / 3.0) - 0.1, 5.0 + 0.1 * Math.Cos(n / 5.0)));
                n2 += Length(plus);
            }

            var a = log2(log2(n2 * 0.05)) * 6.0;
            return new vec3(0.7 + Math.Tan(0.7 * Math.Cos(a)), 0.5 + 0.5 * Math.Cos(a - 0.7),
                0.7 + Math.Sin(0.7 * Math.Cos(a - 0.7)));
        }

        public static vec3 hash3(vec2 vec2)
        {
            return Fract(sin(new vec3(dot(vec2, new vec2(127.1, 311.7)), dot(vec2, new vec2(269.5, 183.3)),
                dot(vec2, new vec2(419.2, 371.9)))).multiply(43758.5453));
        }

        public static double iqnoise(vec2 vec2, double n, double n2)
        {
            vec2 floor = Floor(vec2);
            vec2 fract = Fract(vec2);
            var b = 1.0 + 63.0 * Math.Pow(1.0 - n2, 4.0);
            var n3 = 0.0;
            var n4 = 0.0;
            for (var i = -2; i <= 2; ++i)
            for (var j = -2; j <= 2; ++j)
            {
                var vec3 = new vec2(j, i);
                var multiply = hash3(floor.plus(vec3)).multiply(new vec3(n, n, 1.0));
                var plus = vec3.minus(fract).plus(new vec2(multiply.x, multiply.y));
                var pow = Math.Pow(1.0 - smoothstep(0.0, 1.414, Math.Sqrt(dot(plus, plus))), b);
                n3 += multiply.z * pow;
                n4 += pow;
            }

            return n3 / n4;
        }

        public static double noise(vec2 vec2)
        {
            vec2 floor = Floor(vec2);
            vec2 fract = Fract(vec2);
            double random = Random(floor);
            double random2 = Random(floor.plus(new vec2(1.0, 0.0)));
            double random3 = Random(floor.plus(new vec2(0.0, 1.0)));
            double random4 = Random(floor.plus(new vec2(1.0, 1.0)));
            var multiply = fract.multiply(fract).multiply(new vec2(3.0).minus(new vec2(2.0).multiply(fract)));
            return Mix(random, random2, multiply.x) + (random3 - random) * multiply.y * (1.0 - multiply.x) +
                   (random4 - random2) * multiply.x * multiply.y;
        }

        public static double fbm(vec2 multiply)
        {
            var n = 0.0;
            var n2 = 0.5;
            for (int n3 = 6, i = 0; i < n3; ++i)
            {
                n += n2 * noise(multiply);
                multiply = multiply.multiply(2.0);
                n2 *= 0.5;
            }

            return n;
        }

        // public static double fbm2(vec2 plus)
        // {
        //     var n = 0.0;
        //     var n2 = 0.5;
        //     var vec2 = new vec2(100.0);
        //     mat2 mat2 = new mat2(Math.Cos(0.5), Math.Sin(0.5), -Math.Sin(0.5), Math.Cos(0.5));
        //     for (var i = 0; i < 5; ++i)
        //     {
        //         n += n2 * noise(plus);
        //         plus = mat2.times(plus).multiply(new vec2(2.0)).plus(vec2);
        //         n2 *= 0.5;
        //     }
        //
        //     return n;
        // }

        public static vec3 rgb2hsb(vec3 vec3)
        {
            vec4 vec4 = new vec4(0.0, -0.3333333333333333, 0.6666666666666666, -1.0);
            vec4 mix = Mix(new vec4(vec3.b, vec3.g, vec4.w, vec4.z), new vec4(vec3.g, vec3.b, vec4.x, vec4.y),
                step(vec3.b, vec3.g));
            vec4 mix2 = Mix(new vec4(mix.x, mix.y, mix.w, vec3.r), new vec4(vec3.r, mix.y, mix.z, mix.x),
                step(mix.x, vec3.r));
            double n = mix2.x - Math.Min(mix2.w, mix2.y);
            var n2 = 1.0E-10;
            return new vec3(abs(mix2.z + (mix2.w - mix2.y) / (6.0 * n + n2)), n / (mix2.x + n2), mix2.x);
        }

        public static vec3 hsb2rgb(vec3 vec3)
        {
            vec3 clamp = Clamp(abs(mod(new vec3(vec3.x * 6.0).add(new vec3(0.0, 4.0, 2.0)), 6.0).minus(3.0)).minus(1.0),
                0.0, 1.0);
            return new vec3(vec3.z).multiply(Mix(new vec3(1.0),
                clamp.multiply(clamp).multiply(new vec3(3.0).minus(new vec3(2.0).multiply(clamp))), vec3.y));
        }

        public static vec4 rgb2hsv(vec4 vec4)
        {
            vec4 vec5 = new vec4(0.0, -0.3333333333333333, 0.6666666666666666, -1.0);
            vec4 mix = Mix(new vec4(vec4.b, vec4.g, vec5.w, vec5.z), new vec4(vec4.g, vec4.b, vec5.x, vec5.y),
                step(vec4.b, vec4.g));
            vec4 mix2 = Mix(new vec4(mix.x, mix.y, mix.w, vec4.r), new vec4(vec4.r, mix.y, mix.z, mix.x),
                step(mix.x, vec4.r));
            double n = mix2.x - Math.Min(mix2.w, mix2.y);
            var n2 = 1.0E-10;
            return new vec4(Math.Abs(mix2.z + (mix2.w - mix2.y) / (6.0 * n + n2)), n / (mix2.x + n2), mix2.x, vec4.a);
        }

        public static vec4 hsv2rgb(vec4 vec4)
        {
            vec4 vec5 = new vec4(1.0, 0.6666666666666666, 0.3333333333333333, 3.0);
            return new vec4(
                new vec3(vec4.z).multiply(Mix(new vec3(vec5.x, vec5.x, vec5.x),
                    Clamp(
                        abs(Fract(new vec3(vec4.x, vec4.x, vec4.x).add(new vec3(vec5.x, vec5.y, vec5.z))).multiply(6.0)
                            .minus(new vec3(vec5.w, vec5.w, vec5.w))).minus(new vec3(vec5.x, vec5.x, vec5.x)), 0.0,
                        1.0), vec4.y)), vec4.a);
        }

        public static vec2 ccon(vec2 vec2)
        {
            return new vec2(vec2.x, -vec2.y);
        }

        public static vec2 cmul(vec2 vec2, vec2 vec3)
        {
            return new vec2(vec2.x * vec3.x - vec2.y * vec3.y, vec2.y * vec3.x + vec2.x * vec3.y);
        }

        private vec2 cdiv(vec2 vec2, vec2 vec3)
        {
            return new vec2(vec2.x * vec3.x + vec2.y * vec3.y, vec2.y * vec3.x - vec2.x * vec3.y).multiply(
                1.0 / (vec3.x * vec3.x + vec3.y * vec3.y));
        }

        public static double cabs(vec2 vec2)
        {
            return Length(vec2);
        }

        public static double Carg(vec2 vec2)
        {
            return atan(vec2.y, vec2.x);
        }

        public static vec2 cpow(vec2 vec2, vec2 vec3)
        {
            double carg = Carg(vec2);
            var n = Math.Log(vec2.x * vec2.x + vec2.y * vec2.y) / 2.0;
            var exp = Math.Exp(n * vec3.x - carg * vec3.y);
            var n2 = n * vec3.y + carg * vec3.x;
            return new vec2(Math.Cos(n2) * exp, Math.Sin(n2) * exp);
        }

        public static vec2 cexp(vec2 vec2)
        {
            return new vec2(Math.Cos(vec2.y), Math.Sin(vec2.y)).multiply(Math.Exp(vec2.x));
        }

        public static vec2 clog(vec2 vec2)
        {
            return new vec2(Math.Log(vec2.x * vec2.x + vec2.y * vec2.y) / 2.0, Carg(vec2));
        }

        public static vec2 f(vec2 vec2, double n)
        {
            vec2 = new vec2(2.0).multiply(cpow(vec2, new vec2(3.0, 0.0))).minus(new vec2(0.1).multiply(vec2))
                .plus(new vec2(0.04 + 0.03 * Math.Sin(n * 0.2), 0.02 * Math.Cos(n * 0.46)));
            vec2 = cmul(vec2, cexp(new vec2(0.0, n)));
            return vec2;
        }

        public static vec2 Kscope(vec2 vec2, double n)
        {
            var n2 = Math.Abs(mod(atan2(vec2.y, vec2.x), 2.0 * n) - n) + 0.0;
            return new vec2(Length(vec2)).multiply(new vec2(Math.Cos(n2), Math.Sin(n2)));
        }

        public static vec3 colorize(double n, vec3 vec3, vec3 vec4, vec3 vec5, vec3 vec6)
        {
            return new vec3(2.5).multiply(vec3).multiply(vec4)
                .multiply(cos(new vec3(1.2566370614359172).multiply(vec5.multiply(n).add(vec6))));
        }

        public static double v(vec2 vec2, double n, double n2, double n3)
        {
            return 0.0 + 0.5 * Math.Cos((Math.Cos(n3) * vec2.x + Math.Sin(n3) * vec2.y) * n + n2);
        }

        // public static mat3 rot(vec3 vec3)
        // {
        //     double sin = sin(vec3.x);
        //     double cos = cos(vec3.x);
        //     double sin2 = sin(vec3.y);
        //     double cos2 = cos(vec3.y);
        //     double sin3 = sin(vec3.z);
        //     double cos3 = cos(vec3.z);
        //     return new mat3(new vec3(cos2 * cos3, -cos2 * sin3, sin2),
        //         new vec3(sin * sin2 * cos3 + cos * sin3, -sin * sin2 * sin3 + cos * cos3, -sin * cos2),
        //         new vec3(-cos * sin2 * cos3 + sin * sin3, cos * sin2 * sin3 + sin * cos3, cos * cos2));
        // }

        // public static vec3 app(vec3 minus, double n, mat3 mat3)
        // {
        //     for (var i = 0; i < 50; ++i)
        //         minus = abs(mat3.times(n).times(minus).division(dot(minus, minus)).multiply(0.5).minus(0.5))
        //             .multiply(2.0).minus(1.0);
        //     return minus;
        // }

        public static double cosh(double a)
        {
            var exp = Math.Exp(a);
            return (exp + 1.0 / exp) / 2.0;
        }

        public static double tanh(double n)
        {
            double exp = Exp(n);
            return (exp - 1.0 / exp) / (exp + 1.0 / exp);
        }

        public static double sinh(double a)
        {
            var exp = Math.Exp(a);
            return (exp - 1.0 / exp) / 2.0;
        }

        public static vec2 cosh(vec2 vec2)
        {
            vec2 exp = Exp(vec2);
            vec2 c_add = C_add(exp, C_div(C_one(), exp));
            return new vec2(c_add.x / 2.0, c_add.y / 2.0);
        }

        public static vec2 tanh(vec2 vec2)
        {
            vec2 exp = Exp(vec2);
            return C_div(C_sub(exp, C_div(C_one(), exp)), C_add(exp, C_div(C_one(), exp)));
        }

        public static vec2 sinh(vec2 vec2)
        {
            vec2 exp = Exp(vec2);
            vec2 c_sub = C_sub(exp, C_div(C_one(), exp));
            return new vec2(c_sub.x / 2.0, c_sub.y / 2.0);
        }

        public static vec2 C_one()
        {
            return new vec2(1.0, 0.0);
        }

        public static vec2 C_i()
        {
            return new vec2(0.0, 1.0);
        }

        public static vec2 C_ni()
        {
            return new vec2(0.0, -1.0);
        }

        public static double arg(vec2 vec2)
        {
            return atan(vec2.y, vec2.x);
        }

        public static vec2 c_conj(vec2 vec2)
        {
            return new vec2(vec2.x, -vec2.y);
        }

        public static vec2 c_from_polar(double n, double n2)
        {
            return new vec2(n * cos(n2), n * sin(n2));
        }

        public static vec2 C_to_polar(vec2 vec2)
        {
            return new vec2(Length(vec2), atan(vec2.y, vec2.x));
        }

        public static vec2 c_exp(vec2 vec2)
        {
            return c_from_polar(Math.Exp(vec2.x), vec2.y);
        }

        public static vec2 c_exp(double a, vec2 vec2)
        {
            return c_from_polar(pow(a, vec2.x), vec2.y * Math.Log(a));
        }

        public static vec2 c_ln(vec2 vec2)
        {
            vec2 c_to_polar = C_to_polar(vec2);
            return new vec2(Math.Log(c_to_polar.x), c_to_polar.y);
        }

        public static vec2 c_log(vec2 vec2, double n)
        {
            vec2 c_to_polar = C_to_polar(vec2);
            return new vec2(Math.Log(c_to_polar.x) / Math.Log(n), c_to_polar.y / Math.Log(n));
        }

        public static vec2 c_sqrt(vec2 vec2)
        {
            vec2 c_to_polar = C_to_polar(vec2);
            return c_from_polar(sqrt(c_to_polar.x), c_to_polar.y / 2.0);
        }

        public static vec2 c_pow(vec2 vec2, double n)
        {
            vec2 c_to_polar = C_to_polar(vec2);
            return c_from_polar(pow(c_to_polar.x, n), c_to_polar.y * n);
        }

        public static vec2 c_pow(vec2 vec2, vec2 vec3)
        {
            vec2 c_to_polar = C_to_polar(vec2);
            return c_from_polar(pow(c_to_polar.x, vec3.x) * Exp(-vec3.y * c_to_polar.y),
                vec3.x * c_to_polar.y + vec3.y * Math.Log(c_to_polar.x));
        }

        public static vec2 C_add(vec2 vec2, vec2 vec3)
        {
            return new vec2(vec2.x + vec3.x, vec2.y + vec3.y);
        }

        public static vec2 C_sub(vec2 vec2, vec2 vec3)
        {
            return new vec2(vec2.x - vec3.x, vec2.y - vec3.y);
        }

        public static vec2 c_mul(vec2 vec2, vec2 vec3)
        {
            return new vec2(vec2.x * vec3.x - vec2.y * vec3.y, vec2.x * vec3.y + vec2.y * vec3.x);
        }

        public static vec2 C_div(vec2 vec2, vec2 vec3)
        {
            double length = Length(vec3);
            return new vec2((vec2.x * vec3.x + vec2.y * vec3.y) / (length * length),
                (vec2.y * vec3.x - vec2.x * vec3.y) / (length * length));
        }

        public static vec2 c_sin(vec2 vec2)
        {
            return new vec2(sin(vec2.x) * cosh(vec2.y), cos(vec2.x) * sinh(vec2.y));
        }

        public static vec2 c_cos(vec2 vec2)
        {
            return new vec2(cos(vec2.x) * cosh(vec2.y), -sin(vec2.x) * sinh(vec2.y));
        }

        public static vec2 c_tan(vec2 vec2)
        {
            var multiply = vec2.multiply(2.0);
            return new vec2(sin(multiply.x), sinh(multiply.y)).division(cos(multiply.x) + cosh(multiply.y));
        }

        public static bool c_eq(vec2 vec2, vec2 vec3)
        {
            return vec2.x == vec3.x && vec2.y == vec3.y;
        }

        public static vec2 c_atan(vec2 vec2)
        {
            vec2 c_i = C_i();
            vec2 c_ni = C_ni();
            vec2 c_one = C_one();
            var plus = c_one.plus(c_one);
            if (c_eq(vec2, c_i)) return new vec2(0.0, double.PositiveInfinity);
            if (c_eq(vec2, c_ni)) return new vec2(0.0, double.NegativeInfinity);
            return C_div(C_sub(c_ln(C_add(c_one, c_mul(c_i, vec2))), c_ln(C_sub(c_one, c_mul(c_i, vec2)))),
                c_mul(plus, c_i));
        }

        public static vec2 c_asin(vec2 vec2)
        {
            vec2 c_i = C_i();
            vec2 c_ni = C_ni();
            C_one();
            return c_mul(c_ni, c_ln(C_add(c_sqrt(C_sub(C_one(), c_mul(vec2, vec2))), c_mul(c_i, vec2))));
        }

        public static vec2 c_acos(vec2 vec2)
        {
            return c_mul(C_ni(), c_ln(C_add(c_mul(C_i(), c_sqrt(C_sub(C_one(), c_mul(vec2, vec2)))), vec2)));
        }

        public static vec2 c_sinh(vec2 vec2)
        {
            return new vec2(sinh(vec2.x) * cos(vec2.y), cosh(vec2.x) * sin(vec2.y));
        }

        public static vec2 c_cosh(vec2 vec2)
        {
            return new vec2(cosh(vec2.x) * cos(vec2.y), sinh(vec2.x) * sin(vec2.y));
        }

        public static vec2 c_tanh(vec2 vec2)
        {
            var vec3 = new vec2(2.0 * vec2.x, 2.0 * vec2.y);
            return new vec2(sinh(vec3.x) / (cosh(vec3.x) + cos(vec3.y)), sin(vec3.y) / (cosh(vec3.x) + cos(vec3.y)));
        }

        public static vec2 c_asinh(vec2 vec2)
        {
            return c_ln(C_add(vec2, c_sqrt(C_add(C_one(), c_mul(vec2, vec2)))));
        }

        public static vec2 c_acosh(vec2 vec2)
        {
            vec2 c_one = C_one();
            vec2 c_add = C_add(c_one, c_one);
            return c_mul(c_add,
                c_ln(C_add(c_sqrt(C_div(C_add(vec2, c_one), c_add)), c_sqrt(C_div(C_sub(vec2, c_one), c_add)))));
        }

        public static vec2 c_atanh(vec2 vec2)
        {
            vec2 c_one = C_one();
            var vec3 = new vec2(-c_one.x, -c_one.y);
            var plus = c_one.plus(c_one);
            if (c_eq(vec2, c_one)) return new vec2(double.PositiveInfinity, 0.0);
            if (c_eq(vec2, vec3)) return new vec2(double.NegativeInfinity, 0.0);
            return C_div(C_sub(c_ln(C_add(c_one, vec2)), c_ln(C_sub(c_one, vec2))), plus);
        }

        public static vec2 c_rem(vec2 vec2, vec2 vec3)
        {
            vec2 c_div = C_div(vec2, vec3);
            return vec2.minus(c_mul(vec3, new vec2(c_div.x - mod(c_div.x, 1.0), c_div.y - mod(c_div.y, 1.0))));
        }

        public static vec2 c_inv(vec2 vec2)
        {
            double length = Length(vec2);
            return new vec2(vec2.x / (length * length), -vec2.y / (length * length));
        }
    }
}

    