namespace FlameBase.FlameMath
{
    public class vec3
    {
        public double x;
        public double y;
        public double z;
        public double r;
        public double g;
        public double b;

        public void copyColor( double r,  double g,  double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public vec3( double z)
        {
            this.x = z;
            this.y = z;
            this.copyColor(this.z = z, z, z);
        }

        public vec3( double x,  double y,  double z)
        {
            this.copyColor(this.x = x, this.y = y, this.z = z);
        }

        public vec3( vec3 vec3)
        {
            this.x = vec3.x;
            this.y = vec3.y;
            this.z = vec3.z;
            this.copyColor(vec3.x, vec3.y, vec3.z);
        }

        public vec3( vec2 vec2,  double z)
        {
            this.x = vec2.x;
            this.y = vec2.y;
            this.z = z;
            this.copyColor(vec2.x, vec2.y, z);
        }

        public vec3 plus( double n)
        {
            return new vec3(this.x + n, this.y + n, this.z + n);
        }

        public vec3 plus( vec3 vec3)
        {
            return new vec3(this.x + vec3.x, this.y + vec3.y, this.z + vec3.z);
        }

        public vec3 add( double n)
        {
            return new vec3(this.x + n, this.y + n, this.z + n);
        }

        public vec3 add( vec3 vec3)
        {
            return new vec3(this.x + vec3.x, this.y + vec3.y, this.z + vec3.z);
        }

        public vec3 minus( double n)
        {
            return new vec3(this.x - n, this.y - n, this.z - n);
        }

        public vec3 minus( vec3 vec3)
        {
            return new vec3(this.x - vec3.x, this.y - vec3.y, this.z - vec3.z);
        }

        public vec3 multiply( double n)
        {
            return new vec3(this.x * n, this.y * n, this.z * n);
        }

        public vec3 multiply( vec3 vec3)
        {
            return new vec3(this.x * vec3.x, this.y * vec3.y, this.z * vec3.z);
        }

        public vec3 division( double n)
        {
            return new vec3(this.x / n, this.y / n, this.z / n);
        }

        public vec3 division( vec3 vec3)
        {
            return new vec3(this.x / vec3.x, this.y / vec3.y, this.z / vec3.z);
        }

        // public vec3 times( mat3 mat3)
        // {
        //     return new vec3(this.x * mat3.a00 + this.y * mat3.a10 + this.z * mat3.a20, this.x * mat3.a01 + this.y * mat3.a11 + this.z * mat3.a21, this.x * mat3.a02 + this.y * mat3.a12 + this.z * mat3.a22);
        // }
    }
}