namespace FlameBase.FlameMath
{
    public class mat2
    {
        public double a00;
        public double a01;
        public double a10;
        public double a11;

        public mat2( double a00,  double a2,  double a3,  double a4)
        {
            this.a00 = a00;
            this.a10 = a2;
            this.a01 = a3;
            this.a11 = a4;
        }

        public mat2( vec2 vec2,  vec2 vec3)
        {
            this.a00 = vec2.x;
            this.a10 = vec2.y;
            this.a01 = vec3.x;
            this.a11 = vec3.y;
        }

        public mat2( vec4 vec4)
        {
            this.a00 = vec4.x;
            this.a10 = vec4.y;
            this.a01 = vec4.z;
            this.a11 = vec4.w;
        }

        public vec2 times( vec2 vec2)
        {
            return new vec2(this.a00 * vec2.x + this.a01 * vec2.y, this.a10 * vec2.x + this.a11 * vec2.y);
        }

        public mat2 add( double n)
        {
            return new mat2(this.a00 + n, this.a10 + n, this.a01 + n, this.a11 + n);
        }

        public mat2 minus( double n)
        {
            return new mat2(this.a00 - n, this.a10 - n, this.a01 - n, this.a11 - n);
        }

        public mat2 times( double n)
        {
            return new mat2(this.a00 * n, this.a10 * n, this.a01 * n, this.a11 * n);
        }

        public mat2 division( double n)
        {
            return new mat2(this.a00 / n, this.a10 / n, this.a01 / n, this.a11 / n);
        }
    }
}