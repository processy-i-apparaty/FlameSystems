namespace FlameBase.FlameMath
{
    public class vec2
    {
        public double x;
        public double y;

        public vec2(double n)
        {
            x = n;
            y = n;
        }

        public vec2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public vec2(vec2 vec2)
        {
            x = vec2.x;
            y = vec2.y;
        }

        public vec2 plus(double n)
        {
            var vec2 = new vec2(0.0);
            vec2.x = x + n;
            vec2.y = y + n;
            return vec2;
        }

        public vec2 plus(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = x + vec2.x;
            vec3.y = y + vec2.y;
            return vec3;
        }

        public vec2 add(double n)
        {
            var vec2 = new vec2(0.0);
            vec2.x = x + n;
            vec2.y = y + n;
            return vec2;
        }

        public vec2 add(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = x + vec2.x;
            vec3.y = y + vec2.y;
            return vec3;
        }

        public vec2 minus(double n)
        {
            var vec2 = new vec2(0.0);
            vec2.x = x - n;
            vec2.y = y - n;
            return vec2;
        }

        public vec2 minus(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = x - vec2.x;
            vec3.y = y - vec2.y;
            return vec3;
        }

        public vec2 multiply(double n)
        {
            var vec2 = new vec2(0.0);
            vec2.x = x * n;
            vec2.y = y * n;
            return vec2;
        }

        public vec2 multiply(vec2 vec2)
        {
            var vec3 = new vec2(0.0);
            vec3.x = x * vec2.x;
            vec3.y = y * vec2.y;
            return vec3;
        }

        public vec2 division(double n)
        {
            return new vec2(x / n, y / n);
        }

        public vec2 division(vec2 vec2)
        {
            return new vec2(x / vec2.x, y / vec2.y);
        }

        public vec2 times(mat2 mat2)
        {
            return new vec2(mat2.a00 * x + mat2.a10 * y, x * mat2.a01 + y * mat2.a11);
        }
    }


    // 
    // Decompiled by Procyon v0.5.36
    // 
}