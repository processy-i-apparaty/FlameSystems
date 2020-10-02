using System.Collections.Generic;
using System.Linq;
using FlameBase.FlameMath;
using FlameBase.Models;

namespace FlameBase.RenderMachine.Models
{
    public class RandomTransformModel
    {
        private byte[] _p;
        private XorShift _xorShift;

        public void InitProbabilities(TransformModel[] transformations)
        {
            _xorShift = new XorShift();
            var length = transformations.Length;
            _p = new byte[length];
            var probabilities = GetTProbabilities(transformations);
            var sum = probabilities.Sum();
            for (var i = 0; i < length; i++)
            {
                _p[i] = (byte) Algebra.Map(probabilities[i], 0, sum, 0, 255);
                if (i > 0) _p[i] += _p[i - 1];
            }
        }

        public byte[] GetRandomT(int arrayLength)
        {
            var p = _p.ToArray();
            var length = p.Length;
            var randomArray = new byte[arrayLength];
            var bytes = _xorShift.GetBytes(arrayLength);

            for (var i = 0; i < arrayLength; i++)
            {
                var b = bytes[i];
                for (byte j = 0; j < length; j++)
                {
                    if (b > p[j]) continue;
                    randomArray[i] = j;
                    break;
                }
            }

            return randomArray;
        }

        private static double[] GetTProbabilities(IReadOnlyList<TransformModel> transformations)
        {
            var length = transformations.Count;
            var probabilities = new double[length];
            for (var i = 0; i < length; i++)
            {
                if (transformations[i].IsFinal)
                    probabilities[i] = 0.0;
                else
                    probabilities[i] = transformations[i].Probability;
            }
            return probabilities;
        }
    }
}