namespace FlameBase.FlameMath
{
    public static class Algebra
    {
        public static double Map(double value, double iStart, double iStop, double oStart, double oStop)
        {
            return oStart + (oStop - oStart) * ((value - iStart) / (iStop - iStart));
        }


    }
}