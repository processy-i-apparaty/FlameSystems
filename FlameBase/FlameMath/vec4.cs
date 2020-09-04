namespace FlameBase.FlameMath
{
    public class vec4
    {
        public double x;
        public double y;
        public double z;
        public double w;
        public double r;
        public double g;
        public double b;
        public double a;

        public void copyColor( double r,  double b,  double g,  double a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public vec4( double n)
        {
            this.x = n;
            this.y = n;
            this.z = n;
            this.copyColor(this.w = n, n, n, n);
        }

        public vec4( double x,  double y,  double z,  double w)
        {
            this.copyColor(this.x = x, this.y = y, this.z = z, this.w = w);
        }

        public vec4( vec2 vec2,  double z,  double w)
        {
            this.x = vec2.x;
            this.y = vec2.y;
            this.z = z;
            this.w = w;
            this.copyColor(vec2.x, vec2.y, z, w);
        }

        public vec4( vec2 vec2,  vec2 vec3)
        {
            this.x = vec2.x;
            this.y = vec2.y;
            this.z = vec3.x;
            this.w = vec3.y;
            this.copyColor(vec2.x, vec2.y, vec3.x, vec3.y);
        }

        public vec4( vec3 vec3,  double w)
        {
            this.x = vec3.x;
            this.y = vec3.y;
            this.z = vec3.z;
            this.w = w;
            this.copyColor(vec3.x, vec3.y, vec3.z, w);
        }

        public vec4 plus( double n)
        {
            return new vec4(this.x + n, this.y + n, this.z + n, this.w + n);
        }

        public vec4 plus( vec4 vec4)
        {
            return new vec4(this.x + vec4.x, this.y + vec4.y, this.z + vec4.z, this.w + vec4.w);
        }

        public vec4 add( double n)
        {
            return new vec4(this.x + n, this.y + n, this.z + n, this.w + n);
        }

        public vec4 add( vec4 vec4)
        {
            return new vec4(this.x + vec4.x, this.y + vec4.y, this.z + vec4.z, this.w + vec4.w);
        }

        public vec4 minus( double n)
        {
            return new vec4(this.x - n, this.y - n, this.z - n, this.w - n);
        }

        public vec4 minus( vec4 vec4)
        {
            return new vec4(this.x - vec4.x, this.y - vec4.y, this.z - vec4.z, this.w - vec4.w);
        }

        public vec4 multiply( double n)
        {
            return new vec4(this.x * n, this.y * n, this.z * n, this.w * n);
        }

        public vec4 multiply( vec4 vec4)
        {
            return new vec4(this.x * vec4.x, this.y * vec4.y, this.z * vec4.z, this.w * vec4.w);
        }

        public vec4 division( double n)
        {
            return new vec4(this.x / n, this.y / n, this.z / n, this.w / n);
        }

        public vec4 division( vec4 vec4)
        {
            return new vec4(this.x / vec4.x, this.y / vec4.y, this.z / vec4.z, this.w / vec4.w);
        }

        // public vec4 times( mat4 mat4)
        // {
        //     return new vec4(this.x * mat4.a00 + mat4.a01 * this.y + this.z * mat4.a02 + this.w * mat4.a03, this.x * mat4.a01 + mat4.a11 * this.y + this.z * mat4.a12 + this.w * mat4.a13, this.x * mat4.a02 + mat4.a21 * this.y + this.z * mat4.a22 + this.w * mat4.a23, this.x * mat4.a03 + mat4.a31 * this.y + this.z * mat4.a32 + this.w * mat4.a33);
        // }
    }
}