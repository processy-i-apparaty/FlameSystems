using System;

namespace FlameBase.FlameMath
{
    public class G
    {
        public static Vec2 Reflect(Vec2 vec2, Vec2 vec3)
        {
            return vec2.Minus(vec3.Multiply(2.0 * Dot(vec2, vec3)));
        }

        public static Vec3 Reflect(Vec3 vec3, Vec3 vec4)
        {
            return vec3.Minus(vec4.Multiply(2.0 * Dot(vec3, vec4)));
        }

        public static Vec2 Refract(Vec2 vec2, Vec2 vec3, double n)
        {
            var vec4 = new Vec2(0.0);
            var a = 1.0 - n * n * (1.0 - Dot(vec3, vec2) * Dot(vec3, vec2));
            var minus = a < 0.0
                ? new Vec2(0.0)
                : vec2.Multiply(n).Minus(vec3.Multiply(n * Dot(vec3, vec2) + Math.Sqrt(a)));
            return minus;
        }

        public static Vec3 Refract(Vec3 vec3, Vec3 vec4, double n)
        {
            var vec5 = new Vec3(0.0);
            var a = 1.0 - n * n * (1.0 - Dot(vec4, vec3) * Dot(vec4, vec3));
            Vec3 minus;
            if (a < 0.0)
                minus = new Vec3(0.0);
            else
                minus = vec3.Multiply(n).Minus(vec4.Multiply(n * Dot(vec4, vec3) + Math.Sqrt(a)));
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

        public static double Atan(double n, double n2)
        {
            return Math.Atan(n / n2);
        }

        public static double Atan2(double a, double n)
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

        public static double Sqrt(double a)
        {
            return Math.Sqrt(a);
        }

        public static Vec2 Sqrt(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Math.Sqrt(vec2.X);
            vec3.Y = Math.Sqrt(vec2.Y);
            return vec3;
        }

        public static Vec3 Sqrt(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Math.Sqrt(vec3.X);
            vec4.Y = Math.Sqrt(vec3.Y);
            vec4.Z = Math.Sqrt(vec3.Z);
            return vec4;
        }

        public static Vec4 Sqrt(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Math.Sqrt(vec4.X);
            vec5.Y = Math.Sqrt(vec4.Y);
            vec5.Z = Math.Sqrt(vec4.Z);
            vec5.W = Math.Sqrt(vec4.W);
            return vec5;
        }

        public static double Length(Vec2 vec2)
        {
            return Math.Sqrt(vec2.X * vec2.X + vec2.Y * vec2.Y);
        }

        public static double Length(Vec3 vec3)
        {
            return Math.Sqrt(vec3.X * vec3.X + vec3.Y * vec3.Y + vec3.Z * vec3.Z);
        }

        public static double Length(Vec4 vec4)
        {
            return Math.Sqrt(vec4.X * vec4.X + vec4.Y * vec4.Y + vec4.Z * vec4.Z + vec4.W * vec4.W);
        }

        public static double Abs(double n)
        {
            return n >= 0.0 ? n : -n;
        }

        public static Vec2 Abs(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = vec2.X >= 0.0 ? vec2.X : -vec2.X;
            vec3.Y = vec2.Y >= 0.0 ? vec2.Y : -vec2.Y;
            return vec3;
        }

        public static Vec3 Abs(Vec3 vec3)
        {
            return new Vec3(vec3.X >= 0.0 ? vec3.X : -vec3.X, vec3.Y >= 0.0 ? vec3.Y : -vec3.Y,
                vec3.Z >= 0.0 ? vec3.Z : -vec3.Z);
        }

        public static Vec4 Abs(Vec4 vec4)
        {
            return new Vec4(vec4.X >= 0.0 ? vec4.X : -vec4.X, vec4.Y >= 0.0 ? vec4.Y : -vec4.Y,
                vec4.Z >= 0.0 ? vec4.Z : -vec4.Z, vec4.W >= 0.0 ? vec4.W : -vec4.W);
        }

        public static double Sign(double d)
        {
            return Math.Sign(d);
        }

        public static Vec2 Sign(Vec2 vec2)
        {
            return new Vec2(Math.Sign(vec2.X), Math.Sign(vec2.Y));
        }

        public static Vec3 Sign(Vec3 vec3)
        {
            return new Vec3(Math.Sign(vec3.X), Math.Sign(vec3.Y), Math.Sign(vec3.Z));
        }

        public static double Distance(Vec2 vec2, Vec2 vec3)
        {
            var minus = vec2.Minus(vec3);
            return Math.Sqrt(minus.X * minus.X + minus.Y * minus.Y);
        }

        public static double Distance(Vec3 vec3, Vec3 vec4)
        {
            var minus = vec3.Minus(vec4);
            return Math.Sqrt(minus.X * minus.X + minus.Y * minus.Y + minus.Z * minus.Z);
        }

        public static double Distance(Vec4 vec4, Vec4 vec5)
        {
            var minus = vec4.Minus(vec5);
            return Math.Sqrt(minus.X * minus.X + minus.Y * minus.Y + minus.Z * minus.Z + minus.W * minus.W);
        }

        public static Vec2 Normalize(Vec2 vec2)
        {
            return new Vec2(vec2.X / Length(vec2), vec2.Y / Length(vec2));
        }

        public static Vec3 Normalize(Vec3 vec3)
        {
            return new Vec3(vec3.X / Length(vec3), vec3.Y / Length(vec3), vec3.Z / Length(vec3));
        }

        public static Vec4 Normalize(Vec4 vec4)
        {
            return new Vec4(vec4.X / Length(vec4), vec4.Y / Length(vec4), vec4.Z / Length(vec4), vec4.W / Length(vec4));
        }

        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        public static Vec2 Pow(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(Math.Pow(vec2.X, vec3.X), Math.Pow(vec2.Y, vec3.Y));
        }

        public static Vec3 Pow(Vec3 vec3, Vec3 vec4)
        {
            return new Vec3(Math.Pow(vec3.X, vec4.X), Math.Pow(vec3.Y, vec4.Y), Math.Pow(vec3.Z, vec4.Z));
        }

        public static Vec4 Pow(Vec4 vec4, Vec4 vec5)
        {
            return new Vec4(Math.Pow(vec4.X, vec5.X), Math.Pow(vec4.Y, vec5.Y), Math.Pow(vec4.Z, vec5.Z),
                Math.Pow(vec4.W, vec5.W));
        }

        public static double Exp(double a)
        {
            return Math.Exp(a);
        }

        public static Vec2 Exp(Vec2 vec2)
        {
            return new Vec2(Math.Exp(vec2.X), Math.Exp(vec2.Y));
        }

        public static Vec3 Exp(Vec3 vec3)
        {
            return new Vec3(Math.Exp(vec3.X), Math.Exp(vec3.Y), Math.Exp(vec3.Z));
        }

        public static Vec4 Exp(Vec4 vec4)
        {
            return new Vec4(Math.Exp(vec4.X), Math.Exp(vec4.Y), Math.Exp(vec4.Z), Math.Exp(vec4.W));
        }

        public static double Clamp(double a, double b, double b2)
        {
            return Math.Min(Math.Max(a, b), b2);
        }

        public static Vec2 Clamp(Vec2 vec2, double n, double n2)
        {
            return new Vec2(Math.Min(Math.Max(vec2.X, n), n2), Math.Min(Math.Max(vec2.Y, n), n2));
        }

        public static Vec3 Clamp(Vec3 vec3, double b, double b2)
        {
            return new Vec3(Math.Min(Math.Max(vec3.X, b), b2), Math.Min(Math.Max(vec3.Y, b), b2),
                Math.Min(Math.Max(vec3.Z, b), b2));
        }

        public static Vec4 Clamp(Vec4 vec4, double n, double n2)
        {
            return new Vec4(Math.Min(Math.Max(vec4.X, n), n2), Math.Min(Math.Max(vec4.Y, n), n2),
                Math.Min(Math.Max(vec4.Z, n), n2), Math.Min(Math.Max(vec4.W, n), n2));
        }

        public static double Mix(double n, double n2, double n3)
        {
            return n * (1.0 - n3) + n2 * n3;
        }

        public static Vec2 Mix(Vec2 vec2, Vec2 vec3, double n)
        {
            var vec4 = new Vec2(0.0, 0.0);
            vec4.X = vec2.X * (1.0 - n) + vec3.X * n;
            vec4.Y = vec2.Y * (1.0 - n) + vec3.Y * n;
            return vec4;
        }

        public static Vec3 Mix(Vec3 vec3, Vec3 vec4, double n)
        {
            var vec5 = new Vec3(0.0, 0.0, 0.0);
            vec5.X = vec3.X * (1.0 - n) + vec4.X * n;
            vec5.Y = vec3.Y * (1.0 - n) + vec4.Y * n;
            vec5.Z = vec3.Z * (1.0 - n) + vec4.Z * n;
            return vec5;
        }

        public static Vec4 Mix(Vec4 vec4, Vec4 vec5, double n)
        {
            return new Vec4(vec4.X * (1.0 - n) + vec5.X * n, vec4.Y * (1.0 - n) + vec5.Y * n,
                vec4.Z * (1.0 - n) + vec5.Z * n, vec4.W * (1.0 - n) + vec5.W * n);
        }

        public static double Log2(double a)
        {
            return Math.Log(a) / Math.Log(2.0);
        }

        public static double Smoothstep(double n, double n2, double n3)
        {
            var clamp = Clamp((n3 - n) / (n2 - n), 0.0, 1.0);
            return clamp * clamp * (3.0 - 2.0 * clamp);
        }

        public static Vec2 Smoothstep(double n, double n2, Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Smoothstep(n, n2, vec2.X);
            vec3.Y = Smoothstep(n, n2, vec2.Y);
            return vec3;
        }

        public static Vec2 Smoothstep(Vec2 vec2, Vec2 vec3, Vec2 vec4)
        {
            var vec5 = new Vec2(0.0);
            vec5.X = Smoothstep(vec2.X, vec3.X, vec4.X);
            vec5.Y = Smoothstep(vec2.Y, vec3.Y, vec4.Y);
            return vec5;
        }

        public static Vec3 Smoothstep(double n, double n2, Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Smoothstep(n, n2, vec3.X);
            vec4.Y = Smoothstep(n, n2, vec3.Y);
            vec4.Z = Smoothstep(n, n2, vec3.Z);
            return vec4;
        }

        public static Vec3 Smoothstep(Vec3 vec3, Vec3 vec4, Vec3 vec5)
        {
            var vec6 = new Vec3(0.0);
            vec6.X = Smoothstep(vec3.X, vec4.X, vec5.X);
            vec6.Y = Smoothstep(vec3.Y, vec4.Y, vec5.Y);
            vec6.Z = Smoothstep(vec3.Z, vec4.Z, vec5.Z);
            return vec6;
        }

        public static Vec4 Smoothstep(double n, double n2, Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Smoothstep(n, n2, vec4.X);
            vec5.Y = Smoothstep(n, n2, vec4.Y);
            vec5.Z = Smoothstep(n, n2, vec4.Z);
            vec5.W = Smoothstep(n, n2, vec4.W);
            return vec5;
        }

        public static Vec4 Smoothstep(Vec4 vec4, Vec4 vec5, Vec4 vec6)
        {
            var vec7 = new Vec4(0.0);
            vec7.X = Smoothstep(vec4.X, vec5.X, vec6.X);
            vec7.Y = Smoothstep(vec4.Y, vec5.Y, vec6.Y);
            vec7.Z = Smoothstep(vec4.Z, vec5.Z, vec6.Z);
            vec7.W = Smoothstep(vec4.W, vec5.W, vec6.W);
            return vec7;
        }

        public static double Dot(double n, double n2)
        {
            return n * n2;
        }

        public static double Dot(Vec2 vec2, Vec2 vec3)
        {
            return vec2.X * vec3.X + vec2.Y * vec3.Y;
        }

        public static double Dot(Vec3 vec3, Vec3 vec4)
        {
            return vec3.X * vec4.X + vec3.Y * vec4.Y + vec3.Z * vec4.Z;
        }

        public static double Dot(Vec4 vec4, Vec4 vec5)
        {
            return vec4.X * vec5.X + vec4.Y * vec5.Y + vec4.Z * vec5.Z + vec4.W * vec5.W;
        }

        public static Vec3 Cross(Vec3 vec3, Vec3 vec4)
        {
            return new Vec3(vec3.Y * vec4.Z - vec4.Y * vec3.Z, vec3.Z * vec4.X - vec4.Z * vec3.X,
                vec3.X * vec4.Y - vec4.X * vec3.Y);
        }

        public static double Mod(double n, double n2)
        {
            return n - n2 * Math.Floor(n / n2);
        }

        public static Vec2 Mod(Vec2 vec2, double n)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = vec2.X - n * Math.Floor(vec2.X / n);
            vec3.Y = vec2.Y - n * Math.Floor(vec2.Y / n);
            return vec3;
        }

        public static Vec2 Mod(Vec2 vec2, Vec2 vec3)
        {
            var vec4 = new Vec2(0.0);
            vec4.X = vec2.X - vec3.X * Math.Floor(vec2.X / vec3.X);
            vec4.Y = vec2.Y - vec3.Y * Math.Floor(vec2.Y / vec3.Y);
            return vec4;
        }

        public static Vec3 Mod(Vec3 vec3, double n)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = vec3.X - n * Math.Floor(vec3.X / n);
            vec4.Y = vec3.Y - n * Math.Floor(vec3.Y / n);
            vec4.Z = vec3.Z - n * Math.Floor(vec3.Z / n);
            return vec4;
        }

        public static Vec3 Mod(Vec3 vec3, Vec3 vec4)
        {
            var vec5 = new Vec3(0.0);
            vec5.X = vec3.X - vec4.X * Math.Floor(vec3.X / vec4.X);
            vec5.Y = vec3.Y - vec4.Y * Math.Floor(vec3.Y / vec4.Y);
            vec5.Z = vec3.Z - vec4.Z * Math.Floor(vec3.Z / vec4.Z);
            return vec5;
        }

        public static Vec4 Mod(Vec4 vec4, double n)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = vec4.X - n * Math.Floor(vec4.X / n);
            vec5.Y = vec4.Y - n * Math.Floor(vec4.Y / n);
            vec5.Z = vec4.Z - n * Math.Floor(vec4.Z / n);
            vec5.W = vec4.W - n * Math.Floor(vec4.W / n);
            return vec5;
        }

        public static double Step(double n, double n2)
        {
            if (n2 < n) return 0.0;
            return 1.0;
        }

        public static Vec2 Step(double n, Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            if (vec2.X < n)
                vec3.X = 0.0;
            else
                vec3.X = 1.0;
            if (vec2.Y < n)
                vec3.Y = 0.0;
            else
                vec3.Y = 1.0;
            return vec3;
        }

        public static Vec2 Step(Vec2 vec2, Vec2 vec3)
        {
            var vec4 = new Vec2(0.0);
            if (vec3.X < vec2.X)
                vec4.X = 0.0;
            else
                vec4.X = 1.0;
            if (vec3.Y < vec2.Y)
                vec4.Y = 0.0;
            else
                vec4.Y = 1.0;
            return vec4;
        }

        public static Vec3 Step(double n, Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            if (vec3.X < n)
                vec4.X = 0.0;
            else
                vec4.X = 1.0;
            if (vec3.Y < n)
                vec4.Y = 0.0;
            else
                vec4.Y = 1.0;
            if (vec3.Z < n)
                vec4.Z = 0.0;
            else
                vec4.Z = 1.0;
            return vec4;
        }

        public static Vec3 Step(Vec3 vec3, Vec3 vec4)
        {
            var vec5 = new Vec3(0.0);
            if (vec4.X < vec3.X)
                vec5.X = 0.0;
            else
                vec5.X = 1.0;
            if (vec4.Y < vec3.Y)
                vec5.Y = 0.0;
            else
                vec5.Y = 1.0;
            if (vec4.Z < vec3.Z)
                vec5.Z = 0.0;
            else
                vec5.Z = 1.0;
            return vec5;
        }

        public static Vec4 Step(Vec4 vec4, Vec4 vec5)
        {
            var vec6 = new Vec4(0.0);
            if (vec5.X < vec4.X)
                vec6.X = 0.0;
            else
                vec6.X = 1.0;
            if (vec5.Y < vec4.Y)
                vec6.Y = 0.0;
            else
                vec6.Y = 1.0;
            if (vec5.Z < vec4.Z)
                vec6.Z = 0.0;
            else
                vec6.Z = 1.0;
            if (vec5.W < vec4.W)
                vec6.W = 0.0;
            else
                vec6.W = 1.0;
            return vec6;
        }

        public static double Floor(double a)
        {
            return Math.Floor(a);
        }

        public static Vec2 Floor(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Math.Floor(vec2.X);
            vec3.Y = Math.Floor(vec2.Y);
            return vec3;
        }

        public static Vec3 Floor(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Math.Floor(vec3.X);
            vec4.Y = Math.Floor(vec3.Y);
            vec4.Z = Math.Floor(vec3.Z);
            return vec4;
        }

        public static Vec4 Floor(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Math.Floor(vec4.X);
            vec5.Y = Math.Floor(vec4.Y);
            vec5.Z = Math.Floor(vec4.Z);
            vec5.W = Math.Floor(vec4.W);
            return vec5;
        }

        public static double Trunc(double n)
        {
            return (int) n;
        }

        public static Vec2 Trunc(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Trunc(vec2.X);
            vec3.Y = Trunc(vec2.Y);
            return vec3;
        }

        public static Vec3 Trunc(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Trunc(vec3.X);
            vec4.Y = Trunc(vec3.Y);
            vec4.Z = Trunc(vec3.Z);
            return vec4;
        }

        public static Vec4 Trunc(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Trunc(vec4.X);
            vec5.Y = Trunc(vec4.Y);
            vec5.Z = Trunc(vec4.Z);
            vec5.W = Trunc(vec4.W);
            return vec5;
        }

        public static double Round(double a)
        {
            return Math.Round(a);
        }

        public static Vec2 Round(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Round(vec2.X);
            vec3.Y = Round(vec2.Y);
            return vec3;
        }

        public static Vec3 Round(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Round(vec3.X);
            vec4.Y = Round(vec3.Y);
            vec4.Z = Round(vec3.Z);
            return vec4;
        }

        public static Vec4 Round(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Round(vec4.X);
            vec5.Y = Round(vec4.Y);
            vec5.Z = Round(vec4.Z);
            vec5.W = Round(vec4.W);
            return vec5;
        }

        public static double Ceil(double a)
        {
            return Math.Ceiling(a);
        }

        public static Vec2 Ceil(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Ceil(vec2.X);
            vec3.Y = Ceil(vec2.Y);
            return vec3;
        }

        public static Vec3 Ceil(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Ceil(vec3.X);
            vec4.Y = Ceil(vec3.Y);
            vec4.Z = Ceil(vec3.Z);
            return vec4;
        }

        public static Vec4 Ceil(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Ceil(vec4.X);
            vec5.Y = Ceil(vec4.Y);
            vec5.Z = Ceil(vec4.Z);
            vec5.W = Ceil(vec4.W);
            return vec5;
        }

        public static double Fract(double a)
        {
            return a - Math.Floor(a);
        }

        public static Vec2 Fract(Vec2 vec2)
        {
            return new Vec2(vec2.X - Math.Floor(vec2.X), vec2.Y - Math.Floor(vec2.Y));
        }

        public static Vec3 Fract(Vec3 vec3)
        {
            return new Vec3(vec3.X - Math.Floor(vec3.X), vec3.Y - Math.Floor(vec3.Y), vec3.Z - Math.Floor(vec3.Z));
        }

        public static Vec4 Fract(Vec4 vec4)
        {
            return new Vec4(vec4.X - Math.Floor(vec4.X), vec4.Y - Math.Floor(vec4.Y), vec4.Z - Math.Floor(vec4.Z),
                vec4.W - Math.Floor(vec4.W));
        }

        public static double Min(double n, double n2)
        {
            return n2 < n ? n2 : n;
        }

        public static Vec2 Min(Vec2 vec2, Vec2 vec3)
        {
            var vec4 = new Vec2(0.0);
            vec4.X = vec3.X < vec2.X ? vec3.X : vec2.X;
            vec4.Y = vec3.Y < vec2.Y ? vec3.Y : vec2.Y;
            return vec4;
        }

        public static Vec3 Min(Vec3 vec3, Vec3 vec4)
        {
            var vec5 = new Vec3(0.0);
            vec5.X = vec4.X < vec3.X ? vec4.X : vec3.X;
            vec5.Y = vec4.Y < vec3.Y ? vec4.Y : vec3.Y;
            vec5.Z = vec4.Z < vec3.Z ? vec4.Z : vec3.Z;
            return vec5;
        }

        public static Vec4 Min(Vec4 vec4, Vec4 vec5)
        {
            var vec6 = new Vec4(0.0);
            vec6.X = vec5.X < vec4.X ? vec5.X : vec4.X;
            vec6.Y = vec5.Y < vec4.Y ? vec5.Y : vec4.Y;
            vec6.Z = vec5.Z < vec4.Z ? vec5.Z : vec4.Z;
            vec6.W = vec5.W < vec4.W ? vec5.W : vec4.W;
            return vec6;
        }

        public static Vec2 Min(Vec2 vec2, double n)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = n < vec2.X ? n : vec2.X;
            vec3.Y = n < vec2.Y ? n : vec2.Y;
            return vec3;
        }

        public static Vec3 Min(Vec3 vec3, double n)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = n < vec3.X ? n : vec3.X;
            vec4.Y = n < vec3.Y ? n : vec3.Y;
            vec4.Z = n < vec3.Z ? n : vec3.Z;
            return vec4;
        }

        public static Vec4 Min(Vec4 vec4, double n)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = n < vec4.X ? n : vec4.X;
            vec5.Y = n < vec4.Y ? n : vec4.Y;
            vec5.Z = n < vec4.Z ? n : vec4.Z;
            vec5.W = n < vec4.W ? n : vec4.W;
            return vec5;
        }

        public static double Max(double n, double n2)
        {
            return n < n2 ? n2 : n;
        }

        public static Vec2 Max(Vec2 vec2, Vec2 vec3)
        {
            var vec4 = new Vec2(0.0);
            vec4.X = vec2.X < vec3.X ? vec3.X : vec2.X;
            vec4.Y = vec2.Y < vec3.Y ? vec3.Y : vec2.Y;
            return vec4;
        }

        public static Vec3 Max(Vec3 vec3, Vec3 vec4)
        {
            var vec5 = new Vec3(0.0);
            vec5.X = vec3.X < vec4.X ? vec4.X : vec3.X;
            vec5.Y = vec3.Y < vec4.Y ? vec4.Y : vec3.Y;
            vec5.Z = vec3.Z < vec4.Z ? vec4.Z : vec3.Z;
            return vec5;
        }

        public static Vec4 Max(Vec4 vec4, Vec4 vec5)
        {
            var vec6 = new Vec4(0.0);
            vec6.X = vec4.X < vec5.X ? vec5.X : vec4.X;
            vec6.Y = vec4.Y < vec5.Y ? vec5.Y : vec4.Y;
            vec6.Z = vec4.Z < vec5.Z ? vec5.Z : vec4.Z;
            vec6.W = vec4.W < vec5.W ? vec5.W : vec4.W;
            return vec6;
        }

        public static Vec2 Max(Vec2 vec2, double n)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = vec2.X < n ? n : vec2.X;
            vec3.Y = vec2.Y < n ? n : vec2.Y;
            return vec3;
        }

        public static Vec3 Max(Vec3 vec3, double n)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = vec3.X < n ? n : vec3.X;
            vec4.Y = vec3.Y < n ? n : vec3.Y;
            vec4.Z = vec3.Z < n ? n : vec3.Z;
            return vec4;
        }

        public static Vec4 Max(Vec4 vec4, double n)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = vec4.X < n ? n : vec4.X;
            vec5.Y = vec4.Y < n ? n : vec4.Y;
            vec5.Z = vec4.Z < n ? n : vec4.Z;
            vec5.W = vec4.W < n ? n : vec4.W;
            return vec5;
        }

        public static double Sin(double a)
        {
            return Math.Sin(a);
        }

        public static Vec2 Sin(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Math.Sin(vec2.X);
            vec3.Y = Math.Sin(vec2.Y);
            return vec3;
        }

        public static Vec3 Sin(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Math.Sin(vec3.X);
            vec4.Y = Math.Sin(vec3.Y);
            vec4.Z = Math.Sin(vec3.Z);
            return vec4;
        }

        public static Vec4 Sin(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Math.Sin(vec4.X);
            vec5.Y = Math.Sin(vec4.Y);
            vec5.Z = Math.Sin(vec4.Z);
            vec5.W = Math.Sin(vec4.W);
            return vec5;
        }

        public static double Cos(double a)
        {
            return Math.Cos(a);
        }

        public static Vec2 Cos(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = Math.Cos(vec2.X);
            vec3.Y = Math.Cos(vec2.Y);
            return vec3;
        }

        public static Vec3 Cos(Vec3 vec3)
        {
            var vec4 = new Vec3(0.0);
            vec4.X = Math.Cos(vec3.X);
            vec4.Y = Math.Cos(vec3.Y);
            vec4.Z = Math.Cos(vec3.Z);
            return vec4;
        }

        public static Vec4 Cos(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0);
            vec5.X = Math.Cos(vec4.X);
            vec5.Y = Math.Cos(vec4.Y);
            vec5.Z = Math.Cos(vec4.Z);
            vec5.W = Math.Cos(vec4.W);
            return vec5;
        }

        public static double Plot(Vec2 vec2, double n)
        {
            return Smoothstep(n - 0.02, n, vec2.Y) - Smoothstep(n, n + 0.02, vec2.Y);
        }

        public static double Circle(Vec2 vec2, double n)
        {
            var minus = vec2.Minus(new Vec2(0.5));
            return 1.0 - Smoothstep(n - n * 0.01, n + n * 0.01, Dot(minus, minus) * 4.0);
        }

        // public static vec2 rotate2D(vec2 vec2, double n)
        // {
        //     return new mat2(Math.Cos(n), -Math.Sin(n), Math.Sin(n), Math.Cos(n)).times(vec2.minus(new vec2(0.5)))
        //         .plus(new vec2(0.5));
        // }

        public static Vec2 Tile(Vec2 vec2, double n)
        {
            return Fract(vec2.Multiply(n));
        }

        public static Vec2 BrickTile(Vec2 vec2, double n)
        {
            Vec2 multiply;
            var vec3 = multiply = vec2.Multiply(n);
            multiply.X += Step(1.0, Mod(vec3.Y, 2.0)) * 0.5;
            return Fract(vec3);
        }

        public static Vec2 TruchetPattern(Vec2 vec2, double fract)
        {
            fract = Fract((fract - 0.5) * 2.0);
            if (fract > 0.75)
                vec2 = new Vec2(1.0).Minus(vec2);
            else if (fract > 0.5)
                vec2 = new Vec2(1.0 - vec2.X, vec2.Y);
            else if (fract > 0.25) vec2 = new Vec2(1.0).Minus(new Vec2(1.0 - vec2.X, vec2.Y));
            return vec2;
        }

        public static double Box(Vec2 vec2, Vec2 minus, double n)
        {
            minus = new Vec2(0.5).Minus(minus.Multiply(0.5));
            var vec3 = new Vec2(n * 0.5);
            var multiply = Smoothstep(minus, minus.Plus(vec3), vec2)
                .Multiply(Smoothstep(minus, minus.Plus(vec3), new Vec2(1.0).Minus(vec2)));
            return multiply.X * multiply.Y;
        }

        public static Vec2 Skew(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0);
            vec3.X = 1.1547 * vec2.X;
            vec3.Y = vec2.Y + 0.5 * vec3.X;
            return vec3;
        }

        public static Vec3 SimplexGrid(Vec2 vec2)
        {
            var vec3 = new Vec3(0.0);
            var vec4 = new Vec2(0.0);
            var fract = Fract(Skew(vec2));
            if (fract.X > fract.Y)
            {
                var minus = new Vec2(1.0).Minus(new Vec2(fract.X, fract.Y - fract.X));
                vec3.X = minus.X;
                vec3.Y = minus.Y;
                vec3.Z = fract.Y;
            }
            else
            {
                var minus2 = new Vec2(1.0).Minus(new Vec2(fract.X - fract.Y, fract.Y));
                vec3.Y = minus2.X;
                vec3.Z = minus2.Y;
                vec3.X = fract.X;
            }

            return Fract(vec3);
        }

        public static double Random(Vec2 vec2)
        {
            return Fract(Math.Sin(Dot(new Vec2(vec2.X, vec2.Y), new Vec2(12.9898, 78.233))) * 43758.5453123);
        }

        public static Vec2 Random2(Vec2 vec2)
        {
            return Fract(Sin(new Vec2(Dot(vec2, new Vec2(127.1, 311.7)), Dot(vec2, new Vec2(269.5, 183.3))))
                .Multiply(43758.5453));
        }

        public static Vec3 Fractal(Vec2 vec2)
        {
            var n = 50.0;
            var plus = new Vec2(0.0);
            for (var n2 = 0; n2 < n; ++n2)
            {
                plus = new Vec2(plus.X * plus.X - plus.Y * plus.Y, 2.0 * plus.X * plus.Y).Plus(vec2);
                if (Dot(plus, plus) > 4.0)
                {
                    var n3 = 0.125662 * n2;
                    return new Vec3(new Vec3(Math.Cos(n3 + 0.9), Math.Cos(n3 + 0.3), Math.Cos(n3 + 0.2)).Multiply(0.4)
                        .Add(0.6));
                }
            }

            return new Vec3(0.0);
        }

        public static Vec2 B(Vec2 vec2)
        {
            return new Vec2(Math.Log(Length(vec2)), Atan2(vec2.Y, vec2.X) - 6.3);
        }

        public static Vec3 F(Vec2 vec2, double n)
        {
            var plus = vec2;
            var n2 = 6.0;
            for (int n3 = 30, i = 0; i < n3; ++i)
            {
                plus = B(new Vec2(plus.X, Math.Abs(plus.Y)))
                    .Plus(new Vec2(0.1 * Math.Sin(n / 3.0) - 0.1, 5.0 + 0.1 * Math.Cos(n / 5.0)));
                n2 += Length(plus);
            }

            var a = Log2(Log2(n2 * 0.05)) * 6.0;
            return new Vec3(0.7 + Math.Tan(0.7 * Math.Cos(a)), 0.5 + 0.5 * Math.Cos(a - 0.7),
                0.7 + Math.Sin(0.7 * Math.Cos(a - 0.7)));
        }

        public static Vec3 Hash3(Vec2 vec2)
        {
            return Fract(Sin(new Vec3(Dot(vec2, new Vec2(127.1, 311.7)), Dot(vec2, new Vec2(269.5, 183.3)),
                Dot(vec2, new Vec2(419.2, 371.9)))).Multiply(43758.5453));
        }

        public static double Iqnoise(Vec2 vec2, double n, double n2)
        {
            var floor = Floor(vec2);
            var fract = Fract(vec2);
            var b = 1.0 + 63.0 * Math.Pow(1.0 - n2, 4.0);
            var n3 = 0.0;
            var n4 = 0.0;
            for (var i = -2; i <= 2; ++i)
            for (var j = -2; j <= 2; ++j)
            {
                var vec3 = new Vec2(j, i);
                var multiply = Hash3(floor.Plus(vec3)).Multiply(new Vec3(n, n, 1.0));
                var plus = vec3.Minus(fract).Plus(new Vec2(multiply.X, multiply.Y));
                var pow = Math.Pow(1.0 - Smoothstep(0.0, 1.414, Math.Sqrt(Dot(plus, plus))), b);
                n3 += multiply.Z * pow;
                n4 += pow;
            }

            return n3 / n4;
        }

        public static double Noise(Vec2 vec2)
        {
            var floor = Floor(vec2);
            var fract = Fract(vec2);
            var random = Random(floor);
            var random2 = Random(floor.Plus(new Vec2(1.0, 0.0)));
            var random3 = Random(floor.Plus(new Vec2(0.0, 1.0)));
            var random4 = Random(floor.Plus(new Vec2(1.0, 1.0)));
            var multiply = fract.Multiply(fract).Multiply(new Vec2(3.0).Minus(new Vec2(2.0).Multiply(fract)));
            return Mix(random, random2, multiply.X) + (random3 - random) * multiply.Y * (1.0 - multiply.X) +
                   (random4 - random2) * multiply.X * multiply.Y;
        }

        public static double Fbm(Vec2 multiply)
        {
            var n = 0.0;
            var n2 = 0.5;
            for (int n3 = 6, i = 0; i < n3; ++i)
            {
                n += n2 * Noise(multiply);
                multiply = multiply.Multiply(2.0);
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

        public static Vec3 Rgb2Hsb(Vec3 vec3)
        {
            var vec4 = new Vec4(0.0, -0.3333333333333333, 0.6666666666666666, -1.0);
            var mix = Mix(new Vec4(vec3.B, vec3.G, vec4.W, vec4.Z), new Vec4(vec3.G, vec3.B, vec4.X, vec4.Y),
                Step(vec3.B, vec3.G));
            var mix2 = Mix(new Vec4(mix.X, mix.Y, mix.W, vec3.R), new Vec4(vec3.R, mix.Y, mix.Z, mix.X),
                Step(mix.X, vec3.R));
            var n = mix2.X - Math.Min(mix2.W, mix2.Y);
            var n2 = 1.0E-10;
            return new Vec3(Abs(mix2.Z + (mix2.W - mix2.Y) / (6.0 * n + n2)), n / (mix2.X + n2), mix2.X);
        }

        public static Vec3 Hsb2Rgb(Vec3 vec3)
        {
            var clamp = Clamp(Abs(Mod(new Vec3(vec3.X * 6.0).Add(new Vec3(0.0, 4.0, 2.0)), 6.0).Minus(3.0)).Minus(1.0),
                0.0, 1.0);
            return new Vec3(vec3.Z).Multiply(Mix(new Vec3(1.0),
                clamp.Multiply(clamp).Multiply(new Vec3(3.0).Minus(new Vec3(2.0).Multiply(clamp))), vec3.Y));
        }

        public static Vec4 Rgb2Hsv(Vec4 vec4)
        {
            var vec5 = new Vec4(0.0, -0.3333333333333333, 0.6666666666666666, -1.0);
            var mix = Mix(new Vec4(vec4.B, vec4.G, vec5.W, vec5.Z), new Vec4(vec4.G, vec4.B, vec5.X, vec5.Y),
                Step(vec4.B, vec4.G));
            var mix2 = Mix(new Vec4(mix.X, mix.Y, mix.W, vec4.R), new Vec4(vec4.R, mix.Y, mix.Z, mix.X),
                Step(mix.X, vec4.R));
            var n = mix2.X - Math.Min(mix2.W, mix2.Y);
            var n2 = 1.0E-10;
            return new Vec4(Math.Abs(mix2.Z + (mix2.W - mix2.Y) / (6.0 * n + n2)), n / (mix2.X + n2), mix2.X, vec4.A);
        }

        public static Vec4 Hsv2Rgb(Vec4 vec4)
        {
            var vec5 = new Vec4(1.0, 0.6666666666666666, 0.3333333333333333, 3.0);
            return new Vec4(
                new Vec3(vec4.Z).Multiply(Mix(new Vec3(vec5.X, vec5.X, vec5.X),
                    Clamp(
                        Abs(Fract(new Vec3(vec4.X, vec4.X, vec4.X).Add(new Vec3(vec5.X, vec5.Y, vec5.Z))).Multiply(6.0)
                            .Minus(new Vec3(vec5.W, vec5.W, vec5.W))).Minus(new Vec3(vec5.X, vec5.X, vec5.X)), 0.0,
                        1.0), vec4.Y)), vec4.A);
        }

        public static Vec2 Ccon(Vec2 vec2)
        {
            return new Vec2(vec2.X, -vec2.Y);
        }

        public static Vec2 Cmul(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(vec2.X * vec3.X - vec2.Y * vec3.Y, vec2.Y * vec3.X + vec2.X * vec3.Y);
        }

        private Vec2 Cdiv(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(vec2.X * vec3.X + vec2.Y * vec3.Y, vec2.Y * vec3.X - vec2.X * vec3.Y).Multiply(
                1.0 / (vec3.X * vec3.X + vec3.Y * vec3.Y));
        }

        public static double Cabs(Vec2 vec2)
        {
            return Length(vec2);
        }

        public static double Carg(Vec2 vec2)
        {
            return Atan(vec2.Y, vec2.X);
        }

        public static Vec2 Cpow(Vec2 vec2, Vec2 vec3)
        {
            var carg = Carg(vec2);
            var n = Math.Log(vec2.X * vec2.X + vec2.Y * vec2.Y) / 2.0;
            var exp = Math.Exp(n * vec3.X - carg * vec3.Y);
            var n2 = n * vec3.Y + carg * vec3.X;
            return new Vec2(Math.Cos(n2) * exp, Math.Sin(n2) * exp);
        }

        public static Vec2 Cexp(Vec2 vec2)
        {
            return new Vec2(Math.Cos(vec2.Y), Math.Sin(vec2.Y)).Multiply(Math.Exp(vec2.X));
        }

        public static Vec2 Clog(Vec2 vec2)
        {
            return new Vec2(Math.Log(vec2.X * vec2.X + vec2.Y * vec2.Y) / 2.0, Carg(vec2));
        }

        public static Vec2 f(Vec2 vec2, double n)
        {
            vec2 = new Vec2(2.0).Multiply(Cpow(vec2, new Vec2(3.0, 0.0))).Minus(new Vec2(0.1).Multiply(vec2))
                .Plus(new Vec2(0.04 + 0.03 * Math.Sin(n * 0.2), 0.02 * Math.Cos(n * 0.46)));
            vec2 = Cmul(vec2, Cexp(new Vec2(0.0, n)));
            return vec2;
        }

        public static Vec2 Kscope(Vec2 vec2, double n)
        {
            var n2 = Math.Abs(Mod(Atan2(vec2.Y, vec2.X), 2.0 * n) - n) + 0.0;
            return new Vec2(Length(vec2)).Multiply(new Vec2(Math.Cos(n2), Math.Sin(n2)));
        }

        public static Vec3 Colorize(double n, Vec3 vec3, Vec3 vec4, Vec3 vec5, Vec3 vec6)
        {
            return new Vec3(2.5).Multiply(vec3).Multiply(vec4)
                .Multiply(Cos(new Vec3(1.2566370614359172).Multiply(vec5.Multiply(n).Add(vec6))));
        }

        public static double V(Vec2 vec2, double n, double n2, double n3)
        {
            return 0.0 + 0.5 * Math.Cos((Math.Cos(n3) * vec2.X + Math.Sin(n3) * vec2.Y) * n + n2);
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

        public static double Cosh(double a)
        {
            var exp = Math.Exp(a);
            return (exp + 1.0 / exp) / 2.0;
        }

        public static double Tanh(double n)
        {
            var exp = Exp(n);
            return (exp - 1.0 / exp) / (exp + 1.0 / exp);
        }

        public static double Sinh(double a)
        {
            var exp = Math.Exp(a);
            return (exp - 1.0 / exp) / 2.0;
        }

        public static Vec2 Cosh(Vec2 vec2)
        {
            var exp = Exp(vec2);
            var cAdd = C_add(exp, C_div(C_one(), exp));
            return new Vec2(cAdd.X / 2.0, cAdd.Y / 2.0);
        }

        public static Vec2 Tanh(Vec2 vec2)
        {
            var exp = Exp(vec2);
            return C_div(C_sub(exp, C_div(C_one(), exp)), C_add(exp, C_div(C_one(), exp)));
        }

        public static Vec2 Sinh(Vec2 vec2)
        {
            var exp = Exp(vec2);
            var cSub = C_sub(exp, C_div(C_one(), exp));
            return new Vec2(cSub.X / 2.0, cSub.Y / 2.0);
        }

        public static Vec2 C_one()
        {
            return new Vec2(1.0, 0.0);
        }

        public static Vec2 C_i()
        {
            return new Vec2(0.0, 1.0);
        }

        public static Vec2 C_ni()
        {
            return new Vec2(0.0, -1.0);
        }

        public static double Arg(Vec2 vec2)
        {
            return Atan(vec2.Y, vec2.X);
        }

        public static Vec2 c_conj(Vec2 vec2)
        {
            return new Vec2(vec2.X, -vec2.Y);
        }

        public static Vec2 c_from_polar(double n, double n2)
        {
            return new Vec2(n * Cos(n2), n * Sin(n2));
        }

        public static Vec2 C_to_polar(Vec2 vec2)
        {
            return new Vec2(Length(vec2), Atan(vec2.Y, vec2.X));
        }

        public static Vec2 c_exp(Vec2 vec2)
        {
            return c_from_polar(Math.Exp(vec2.X), vec2.Y);
        }

        public static Vec2 c_exp(double a, Vec2 vec2)
        {
            return c_from_polar(Pow(a, vec2.X), vec2.Y * Math.Log(a));
        }

        public static Vec2 c_ln(Vec2 vec2)
        {
            var cToPolar = C_to_polar(vec2);
            return new Vec2(Math.Log(cToPolar.X), cToPolar.Y);
        }

        public static Vec2 c_log(Vec2 vec2, double n)
        {
            var cToPolar = C_to_polar(vec2);
            return new Vec2(Math.Log(cToPolar.X) / Math.Log(n), cToPolar.Y / Math.Log(n));
        }

        public static Vec2 c_sqrt(Vec2 vec2)
        {
            var cToPolar = C_to_polar(vec2);
            return c_from_polar(Sqrt(cToPolar.X), cToPolar.Y / 2.0);
        }

        public static Vec2 c_pow(Vec2 vec2, double n)
        {
            var cToPolar = C_to_polar(vec2);
            return c_from_polar(Pow(cToPolar.X, n), cToPolar.Y * n);
        }

        public static Vec2 c_pow(Vec2 vec2, Vec2 vec3)
        {
            var cToPolar = C_to_polar(vec2);
            return c_from_polar(Pow(cToPolar.X, vec3.X) * Exp(-vec3.Y * cToPolar.Y),
                vec3.X * cToPolar.Y + vec3.Y * Math.Log(cToPolar.X));
        }

        public static Vec2 C_add(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(vec2.X + vec3.X, vec2.Y + vec3.Y);
        }

        public static Vec2 C_sub(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(vec2.X - vec3.X, vec2.Y - vec3.Y);
        }

        public static Vec2 c_mul(Vec2 vec2, Vec2 vec3)
        {
            return new Vec2(vec2.X * vec3.X - vec2.Y * vec3.Y, vec2.X * vec3.Y + vec2.Y * vec3.X);
        }

        public static Vec2 C_div(Vec2 vec2, Vec2 vec3)
        {
            var length = Length(vec3);
            return new Vec2((vec2.X * vec3.X + vec2.Y * vec3.Y) / (length * length),
                (vec2.Y * vec3.X - vec2.X * vec3.Y) / (length * length));
        }

        public static Vec2 c_sin(Vec2 vec2)
        {
            return new Vec2(Sin(vec2.X) * Cosh(vec2.Y), Cos(vec2.X) * Sinh(vec2.Y));
        }

        public static Vec2 c_cos(Vec2 vec2)
        {
            return new Vec2(Cos(vec2.X) * Cosh(vec2.Y), -Sin(vec2.X) * Sinh(vec2.Y));
        }

        public static Vec2 c_tan(Vec2 vec2)
        {
            var multiply = vec2.Multiply(2.0);
            return new Vec2(Sin(multiply.X), Sinh(multiply.Y)).Division(Cos(multiply.X) + Cosh(multiply.Y));
        }

        public static bool c_eq(Vec2 vec2, Vec2 vec3)
        {
            return vec2.X == vec3.X && vec2.Y == vec3.Y;
        }

        public static Vec2 c_atan(Vec2 vec2)
        {
            var cI = C_i();
            var cNi = C_ni();
            var cOne = C_one();
            var plus = cOne.Plus(cOne);
            if (c_eq(vec2, cI)) return new Vec2(0.0, double.PositiveInfinity);
            if (c_eq(vec2, cNi)) return new Vec2(0.0, double.NegativeInfinity);
            return C_div(C_sub(c_ln(C_add(cOne, c_mul(cI, vec2))), c_ln(C_sub(cOne, c_mul(cI, vec2)))),
                c_mul(plus, cI));
        }

        public static Vec2 c_asin(Vec2 vec2)
        {
            var cI = C_i();
            var cNi = C_ni();
            C_one();
            return c_mul(cNi, c_ln(C_add(c_sqrt(C_sub(C_one(), c_mul(vec2, vec2))), c_mul(cI, vec2))));
        }

        public static Vec2 c_acos(Vec2 vec2)
        {
            return c_mul(C_ni(), c_ln(C_add(c_mul(C_i(), c_sqrt(C_sub(C_one(), c_mul(vec2, vec2)))), vec2)));
        }

        public static Vec2 c_sinh(Vec2 vec2)
        {
            return new Vec2(Sinh(vec2.X) * Cos(vec2.Y), Cosh(vec2.X) * Sin(vec2.Y));
        }

        public static Vec2 c_cosh(Vec2 vec2)
        {
            return new Vec2(Cosh(vec2.X) * Cos(vec2.Y), Sinh(vec2.X) * Sin(vec2.Y));
        }

        public static Vec2 c_tanh(Vec2 vec2)
        {
            var vec3 = new Vec2(2.0 * vec2.X, 2.0 * vec2.Y);
            return new Vec2(Sinh(vec3.X) / (Cosh(vec3.X) + Cos(vec3.Y)), Sin(vec3.Y) / (Cosh(vec3.X) + Cos(vec3.Y)));
        }

        public static Vec2 c_asinh(Vec2 vec2)
        {
            return c_ln(C_add(vec2, c_sqrt(C_add(C_one(), c_mul(vec2, vec2)))));
        }

        public static Vec2 c_acosh(Vec2 vec2)
        {
            var cOne = C_one();
            var cAdd = C_add(cOne, cOne);
            return c_mul(cAdd,
                c_ln(C_add(c_sqrt(C_div(C_add(vec2, cOne), cAdd)), c_sqrt(C_div(C_sub(vec2, cOne), cAdd)))));
        }

        public static Vec2 c_atanh(Vec2 vec2)
        {
            var cOne = C_one();
            var vec3 = new Vec2(-cOne.X, -cOne.Y);
            var plus = cOne.Plus(cOne);
            if (c_eq(vec2, cOne)) return new Vec2(double.PositiveInfinity, 0.0);
            if (c_eq(vec2, vec3)) return new Vec2(double.NegativeInfinity, 0.0);
            return C_div(C_sub(c_ln(C_add(cOne, vec2)), c_ln(C_sub(cOne, vec2))), plus);
        }

        public static Vec2 c_rem(Vec2 vec2, Vec2 vec3)
        {
            var cDiv = C_div(vec2, vec3);
            return vec2.Minus(c_mul(vec3, new Vec2(cDiv.X - Mod(cDiv.X, 1.0), cDiv.Y - Mod(cDiv.Y, 1.0))));
        }

        public static Vec2 c_inv(Vec2 vec2)
        {
            var length = Length(vec2);
            return new Vec2(vec2.X / (length * length), -vec2.Y / (length * length));
        }
    }
}