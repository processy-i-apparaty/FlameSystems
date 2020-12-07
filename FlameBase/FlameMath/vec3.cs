namespace FlameBase.FlameMath
{
    public class Vec3
    {
        public Vec3(double z)
        {
            X = z;
            Y = z;
            CopyColor(Z = z, z, z);
        }

        public Vec3(double x, double y, double z)
        {
            CopyColor(X = x, Y = y, Z = z);
        }

        public Vec3(Vec3 vec3)
        {
            X = vec3.X;
            Y = vec3.Y;
            Z = vec3.Z;
            CopyColor(vec3.X, vec3.Y, vec3.Z);
        }

        public Vec3(Vec2 vec2, double z)
        {
            X = vec2.X;
            Y = vec2.Y;
            Z = z;
            CopyColor(vec2.X, vec2.Y, z);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public void CopyColor(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Vec3 Plus(double n)
        {
            return new Vec3(X + n, Y + n, Z + n);
        }

        public Vec3 Plus(Vec3 vec3)
        {
            return new Vec3(X + vec3.X, Y + vec3.Y, Z + vec3.Z);
        }

        public Vec3 Add(double n)
        {
            return new Vec3(X + n, Y + n, Z + n);
        }

        public Vec3 Add(Vec3 vec3)
        {
            return new Vec3(X + vec3.X, Y + vec3.Y, Z + vec3.Z);
        }

        public Vec3 Minus(double n)
        {
            return new Vec3(X - n, Y - n, Z - n);
        }

        public Vec3 Minus(Vec3 vec3)
        {
            return new Vec3(X - vec3.X, Y - vec3.Y, Z - vec3.Z);
        }

        public Vec3 Multiply(double n)
        {
            return new Vec3(X * n, Y * n, Z * n);
        }

        public Vec3 Multiply(Vec3 vec3)
        {
            return new Vec3(X * vec3.X, Y * vec3.Y, Z * vec3.Z);
        }

        public Vec3 Division(double n)
        {
            return new Vec3(X / n, Y / n, Z / n);
        }

        public Vec3 Division(Vec3 vec3)
        {
            return new Vec3(X / vec3.X, Y / vec3.Y, Z / vec3.Z);
        }

        // public vec3 times( mat3 mat3)
        // {
        //     return new vec3(this.x * mat3.a00 + this.y * mat3.a10 + this.z * mat3.a20, this.x * mat3.a01 + this.y * mat3.a11 + this.z * mat3.a21, this.x * mat3.a02 + this.y * mat3.a12 + this.z * mat3.a22);
        // }
    }
}