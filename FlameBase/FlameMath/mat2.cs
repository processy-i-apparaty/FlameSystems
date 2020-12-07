namespace FlameBase.FlameMath
{
    public class Mat2
    {
        public Mat2(double a00, double a2, double a3, double a4)
        {
            A00 = a00;
            A10 = a2;
            A01 = a3;
            A11 = a4;
        }

        public Mat2(Vec2 vec2, Vec2 vec3)
        {
            A00 = vec2.X;
            A10 = vec2.Y;
            A01 = vec3.X;
            A11 = vec3.Y;
        }

        public Mat2(Vec4 vec4)
        {
            A00 = vec4.X;
            A10 = vec4.Y;
            A01 = vec4.Z;
            A11 = vec4.W;
        }

        public double A00 { get; }
        public double A01 { get; }
        public double A10 { get; }
        public double A11 { get; }

        public Vec2 Times(Vec2 vec2)
        {
            return new Vec2(A00 * vec2.X + A01 * vec2.Y, A10 * vec2.X + A11 * vec2.Y);
        }

        public Mat2 Add(double n)
        {
            return new Mat2(A00 + n, A10 + n, A01 + n, A11 + n);
        }

        public Mat2 Minus(double n)
        {
            return new Mat2(A00 - n, A10 - n, A01 - n, A11 - n);
        }

        public Mat2 Times(double n)
        {
            return new Mat2(A00 * n, A10 * n, A01 * n, A11 * n);
        }

        public Mat2 Division(double n)
        {
            return new Mat2(A00 / n, A10 / n, A01 / n, A11 / n);
        }
    }
}