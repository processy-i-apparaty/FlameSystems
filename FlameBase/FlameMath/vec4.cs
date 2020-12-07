namespace FlameBase.FlameMath
{
    public class Vec4
    {
        public Vec4(double n)
        {
            X = n;
            Y = n;
            Z = n;
            CopyColor(W = n, n, n, n);
        }

        public Vec4(double x, double y, double z, double w)
        {
            CopyColor(X = x, Y = y, Z = z, W = w);
        }

        public Vec4(Vec2 vec2, double z, double w)
        {
            X = vec2.X;
            Y = vec2.Y;
            Z = z;
            W = w;
            CopyColor(vec2.X, vec2.Y, z, w);
        }

        public Vec4(Vec2 vec2, Vec2 vec3)
        {
            X = vec2.X;
            Y = vec2.Y;
            Z = vec3.X;
            W = vec3.Y;
            CopyColor(vec2.X, vec2.Y, vec3.X, vec3.Y);
        }

        public Vec4(Vec3 vec3, double w)
        {
            X = vec3.X;
            Y = vec3.Y;
            Z = vec3.Z;
            W = w;
            CopyColor(vec3.X, vec3.Y, vec3.Z, w);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public double A { get; set; }

        public void CopyColor(double r, double b, double g, double a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Vec4 Plus(double n)
        {
            return new Vec4(X + n, Y + n, Z + n, W + n);
        }

        public Vec4 Plus(Vec4 vec4)
        {
            return new Vec4(X + vec4.X, Y + vec4.Y, Z + vec4.Z, W + vec4.W);
        }

        public Vec4 Add(double n)
        {
            return new Vec4(X + n, Y + n, Z + n, W + n);
        }

        public Vec4 Add(Vec4 vec4)
        {
            return new Vec4(X + vec4.X, Y + vec4.Y, Z + vec4.Z, W + vec4.W);
        }

        public Vec4 Minus(double n)
        {
            return new Vec4(X - n, Y - n, Z - n, W - n);
        }

        public Vec4 Minus(Vec4 vec4)
        {
            return new Vec4(X - vec4.X, Y - vec4.Y, Z - vec4.Z, W - vec4.W);
        }

        public Vec4 Multiply(double n)
        {
            return new Vec4(X * n, Y * n, Z * n, W * n);
        }

        public Vec4 Multiply(Vec4 vec4)
        {
            return new Vec4(X * vec4.X, Y * vec4.Y, Z * vec4.Z, W * vec4.W);
        }

        public Vec4 Division(double n)
        {
            return new Vec4(X / n, Y / n, Z / n, W / n);
        }

        public Vec4 Division(Vec4 vec4)
        {
            return new Vec4(X / vec4.X, Y / vec4.Y, Z / vec4.Z, W / vec4.W);
        }

        // public vec4 times( mat4 mat4)
        // {
        //     return new vec4(this.x * mat4.a00 + mat4.a01 * this.y + this.z * mat4.a02 + this.w * mat4.a03, this.x * mat4.a01 + mat4.a11 * this.y + this.z * mat4.a12 + this.w * mat4.a13, this.x * mat4.a02 + mat4.a21 * this.y + this.z * mat4.a22 + this.w * mat4.a23, this.x * mat4.a03 + mat4.a31 * this.y + this.z * mat4.a32 + this.w * mat4.a33);
        // }
    }
}