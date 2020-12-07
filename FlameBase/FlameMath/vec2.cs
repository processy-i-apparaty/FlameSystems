namespace FlameBase.FlameMath
{
    public class Vec2
    {
        public Vec2(double n)
        {
            X = n;
            Y = n;
        }

        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vec2(Vec2 vec2)
        {
            X = vec2.X;
            Y = vec2.Y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public Vec2 Plus(double n)
        {
            var vec2 = new Vec2(0.0) {X = X + n, Y = Y + n};
            return vec2;
        }

        public Vec2 Plus(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0) {X = X + vec2.X, Y = Y + vec2.Y};
            return vec3;
        }

        public Vec2 Add(double n)
        {
            var vec2 = new Vec2(0.0) {X = X + n, Y = Y + n};
            return vec2;
        }

        public Vec2 Add(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0) {X = X + vec2.X, Y = Y + vec2.Y};
            return vec3;
        }

        public Vec2 Minus(double n)
        {
            var vec2 = new Vec2(0.0) {X = X - n, Y = Y - n};
            return vec2;
        }

        public Vec2 Minus(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0) {X = X - vec2.X, Y = Y - vec2.Y};
            return vec3;
        }

        public Vec2 Multiply(double n)
        {
            var vec2 = new Vec2(0.0) {X = X * n, Y = Y * n};
            return vec2;
        }

        public Vec2 Multiply(Vec2 vec2)
        {
            var vec3 = new Vec2(0.0) {X = X * vec2.X, Y = Y * vec2.Y};
            return vec3;
        }

        public Vec2 Division(double n)
        {
            return new Vec2(X / n, Y / n);
        }

        public Vec2 Division(Vec2 vec2)
        {
            return new Vec2(X / vec2.X, Y / vec2.Y);
        }

        public Vec2 Times(Mat2 mat2)
        {
            return new Vec2(mat2.A00 * X + mat2.A10 * Y, X * mat2.A01 + Y * mat2.A11);
        }
    }


    // 
    // Decompiled by Procyon v0.5.36
    // 
}