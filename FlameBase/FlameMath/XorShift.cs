using System;

namespace FlameBase.FlameMath
{
    public class XorShift
    {
        private uint _w; // = 88675123;
        private uint _x; // = 123456789;
        private uint _y; // = 362436069;
        private uint _z; // = 521288629;

        public XorShift()
        {
            _x = (uint) Guid.NewGuid().GetHashCode();
            _y = (uint) Guid.NewGuid().GetHashCode();
            _z = (uint) Guid.NewGuid().GetHashCode();
            _w = (uint) Guid.NewGuid().GetHashCode();
        }

        public unsafe void FillBuffer(byte[] buf, int offset, int offsetEnd)
        {
            uint x = _x, y = _y, z = _z, w = _w;
            fixed (byte* pBytes = buf)
            {
                var pBuf = (uint*) (pBytes + offset);
                var pend = (uint*) (pBytes + offsetEnd);
                while (pBuf < pend)
                {
                    var t = x ^ (x << 11);
                    x = y;
                    y = z;
                    z = w;
                    w = w ^ (w >> 19) ^ t ^ (t >> 8);
                    *pBuf++ = w;
                }
            }

            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public byte[] GetBytes(int length)
        {
            var bytes = new byte[length];
            FillBuffer(bytes, 0, length);
            return bytes;
        }
    }
}