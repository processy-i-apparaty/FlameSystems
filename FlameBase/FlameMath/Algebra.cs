namespace FlameBase.FlameMath
{
    public class Algebra
    {
        public double Map(double value, double iStart, double iStop, double oStart, double oStop)
        {
            return oStart + (oStop - oStart) * ((value - iStart) / (iStop - iStart));
        }
    }
}