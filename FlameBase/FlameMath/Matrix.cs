using System;
using System.Diagnostics;
using FlameBase.Models;

namespace FlameBase.FlameMath
{
    public class Matrix
    {
        private double[,] _array;
        public bool IsInitialized => _array != null;

        public double[] Values
        {
            get
            {
                if (!IsInitialized) return null;
                var k = 0;
                var values = new double[_array.Length];
                for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                {
                    values[k] = _array[i, j];
                    k++;
                }

                return values;
            }
        }

        public double[,] Array
        {
            get
            {
                var ret = new double[Rows, Columns];
                for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    ret[i, j] = _array[i, j];
                return ret;
            }
        }

        public int Rows => _array.GetLength(0);
        public int Columns => _array.GetLength(1);

        public override string ToString()
        {
            var str = string.Empty;
            for (var i = 0; i < Array.GetLength(0); i++)
            {
                str += "[";
                for (var j = 0; j < Array.GetLength(1); j++) str += Array[i, j] + " ";
                str += "]";
                if (i < Array.GetLength(0) - 1)
                    str += "\n";
            }

            return str;
        }


        public static Matrix FromViewSettings(ViewSettingsModel viewSettings)
        {
            var z = viewSettings.Zoom * viewSettings.ImageWidth;
            var sX = viewSettings.ShiftX * z;
            var sY = viewSettings.ShiftY * z;

            var cos = Math.Cos(viewSettings.Rotation);
            var sin = Math.Sin(viewSettings.Rotation);

            var rotate = FromArray(new[,]
            {
                {cos, -sin, 0},
                {sin, cos, 0},
                {0, 0, 1}
            });

            var shift = FromArray(new[,]
            {
                {1, 0, sX},
                {0, 1, sY},
                {0, 0, 1}
            });

            var scale = FromArray(new[,]
            {
                {z, 0, 0},
                {0, z, 0},
                {0, 0, 1}
            });

            var mtx = shift * rotate * scale;
            return mtx;
        }
        public static Matrix FromArray(double[,] array)
        {
            return new Matrix {_array = array};
        }

        public static Matrix FromVector(params double[] vector)
        {
            var array = new double[vector.Length, 1];
            for (var i = 0; i < vector.Length; i++) array[i, 0] = vector[i];
            var m = FromArray(array);
            return m;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (!Check(a, b, CheckType.Mul)) return null;
            var result = new double[a.Rows, b.Columns];
            for (var i = 0; i < a.Rows; i++)
            for (var j = 0; j < b.Columns; j++)
            {
                var sum = 0.0;
                for (var k = 0; k < a.Columns; k++) sum += a._array[i, k] * b._array[k, j];
                result[i, j] = sum;
            }

            return FromArray(result);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (!Check(a, b, CheckType.Add)) return null;
            var array = new double[a.Rows, a.Columns];
            for (var i = 0; i < a.Rows; i++)
            for (var j = 0; j < a.Columns; j++)
                array[i, j] = a._array[i, j] + b._array[i, j];
            return FromArray(array);
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (!Check(a, b, CheckType.Add)) return null;
            var array = new double[a.Rows, a.Columns];
            for (var i = 0; i < a.Rows; i++)
            for (var j = 0; j < a.Columns; j++)
                array[i, j] = a._array[i, j] - b._array[i, j];
            return FromArray(array);
        }


        private static bool Check(Matrix a, Matrix b, CheckType t)
        {
            if (!a.IsInitialized)
            {
                Debug.WriteLine("A must be initialized");
                return false;
            }

            if (!b.IsInitialized)
            {
                Debug.WriteLine("A must be initialized");
                return false;
            }

            switch (t)
            {
                case CheckType.Add:
                    if (a.Rows != b.Rows)
                    {
                        Debug.WriteLine("Rows of A must match rows of B");
                        return false;
                    }

                    if (a.Columns == b.Columns) return true;
                    Debug.WriteLine("Columns of A must match columns of B");
                    return false;
                case CheckType.Mul:
                    if (a.Columns == b.Rows) return true;
                    Debug.WriteLine("Columns of A must match rows of B");
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), t, null);
            }
        }

        // public static XMatrix FromVector(FPoint3D vector)
        // {
        //     return FromVector(vector.X, vector.Y, vector.Z);
        // }

        private enum CheckType
        {
            Add,
            Mul
        }
    }
}