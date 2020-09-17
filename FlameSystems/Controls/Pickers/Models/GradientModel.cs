using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ColorMine.ColorSpaces;
using FlameBase.FlameMath;
using FlameSystems.Models;

namespace FlameSystems.Controls.Pickers.Models
{
    public class GradientModel
    {
        public GradientModel(Color startColor, Color endColor)
        {
            ModelId = GiveIdModel.Get;

            StartColor = startColor;
            EndColor = endColor;
            InitArrays();
        }

        public GradientModel(Color startColor, Color endColor, IReadOnlyList<Color> midColors,
            IReadOnlyList<double> midValues)
        {
            ModelId = GiveIdModel.Get;

            StartColor = startColor;
            EndColor = endColor;
            InitArrays();
            for (var i = 0; i < midColors.Count; i++) Add(GiveIdModel.Get, midColors[i], midValues[i]);
        }

        // public GradientModel(GradientPackModel pack)
        // {
        //     var length = pack.Values.Length;
        //     StartColor = pack.Colors[0];
        //     EndColor = pack.Colors[length - 1];
        //     InitArrays();
        //     for (var i = 0; i < length - 2; i++) Add(GiveIdModel.Get, pack.Colors[i + 1], pack.Values[i + 1]);
        // }

        public Color StartColor { get; set; }
        public Color EndColor { get; set; }

        public Color[] Colors { get; set; }
        public double[] Values { get; set; }
        public int[] Ids { get; set; }
        public int Length => Values.Length;
        public int ModelId { get; }


        private void InitArrays()
        {
            Colors = Array.Empty<Color>();
            Values = Array.Empty<double>();
            Ids = Array.Empty<int>();
        }

        public bool Add(int id, Color color, double position)
        {
            if (position <= 0.0 || position >= 1.0) return false;

            var i = GetI(position);
            var colors = Colors.ToList();
            var values = Values.ToList();
            var ids = Ids.ToList();
            values.Insert(i, position);
            colors.Insert(i, color);
            ids.Insert(i, id);
            Values = values.ToArray();
            Colors = colors.ToArray();
            Ids = ids.ToArray();
            return true;
        }

        public bool Move(int id, double position)
        {
            var i = Ids.ToList().FindIndex(x => x == id);
            if (i < 0 || i >= Length) return false;
            var c = Colors[i];
            return Delete(id) && Add(id, c, position);
        }

        public bool Delete(int id)
        {
            var i = Ids.ToList().FindIndex(x => x == id);
            if (i < 0 || i >= Length) return false;

            var colors = Colors.ToList();
            var values = Values.ToList();
            var ids = Ids.ToList();

            colors.RemoveAt(i);
            values.RemoveAt(i);
            ids.RemoveAt(i);

            Values = values.ToArray();
            Colors = colors.ToArray();
            Ids = ids.ToArray();
            return true;
        }

        private int GetI(double position)
        {
            int i;
            for (i = 0; i < Values.Length; i++)
                if (position < Values[i])
                    break;
            return i;
        }


        public double[] GetValueBetween(double position)
        {
            var min = 0.0;
            var max = 1.0;
            for (var i = 0; i < Values.Length; i++)
            {
                if (position < Values[i])
                {
                    max = Values[i];
                    if (i > 0)
                        min = Values[i - 1];
                    break;
                }

                min = Values[i];
            }

            return new[] {min, max};
        }

        public Color[] GetColorBetween(double position)
        {
            var min = StartColor;
            var max = EndColor;
            for (var i = 0; i < Colors.Length; i++)
            {
                if (position < Values[i])
                {
                    max = Colors[i];
                    if (i > 0)
                        min = Colors[i - 1];
                    break;
                }

                min = Colors[i];
            }

            return new[] {min, max};
        }


        public Color? GetColor(int i)
        {
            if (i < 0 || i >= Length) return null;
            return Colors[i];
        }

        public double? GetValue(int i)
        {
            if (i < 0 || i >= Length) return null;
            return Values[i];
        }


        public Color GetFromPosition(double position)
        {
            var cb = GetColorBetween(position);
            var p = GetColorsPercent(position);

            var labA = Color2Lab(cb[0]);
            var labB = Color2Lab(cb[1]);
            var labSum = new Lab
            {
                L = labA.L * p[0] + labB.L * p[1],
                A = labA.A * p[0] + labB.A * p[1],
                B = labA.B * p[0] + labB.B * p[1]
            };
            return Lab2Color(labSum);
        }

        public Lab GetLabFromPosition(double position)
        {
            var cb = GetColorBetween(position);
            var p = GetColorsPercent(position);

            var labA = Color2Lab(cb[0]);
            var labB = Color2Lab(cb[1]);
            var labSum = new Lab
            {
                L = labA.L * p[0] + labB.L * p[1],
                A = labA.A * p[0] + labB.A * p[1],
                B = labA.B * p[0] + labB.B * p[1]
            };
            return labSum;
        }

        private static Color Lab2Color(IColorSpace lab)
        {
            var rgb = lab.To<Rgb>();
            return Color.FromRgb((byte) Math.Round(rgb.R), (byte) Math.Round(rgb.G), (byte) Math.Round(rgb.B));
        }

        private static Lab Color2Lab(Color color)
        {
            var rgb = new Rgb
            {
                R = color.R,
                G = color.G,
                B = color.B
            };
            return rgb.To<Lab>();
        }


        public Color GetFromPositionLog(double position)
        {
            var p = position * position;
            return GetFromPosition(p);
        }

        public double[] GetColorsPercent(double position)
        {
            var vb = GetValueBetween(position);
            var c1 = Algebra.Map(position, vb[0], vb[1], 0.0, 1.0);
            // var c1 = Scale(vb[0], vb[1], 0.0, 1.0, position);

            var c2 = 1.0 - c1;
            return new[] {c2, c1};
        }

        public double GetPosition(int id)
        {
            var i = Ids.ToList().FindIndex(x => x == id);
            return Values[i];
        }

        public void ChangeColor(int id, Color color)
        {
            if (id == -11)
            {
                StartColor = color;
                return;
            }

            if (id == -22)
            {
                EndColor = color;
                return;
            }

            var i = Ids.ToList().FindIndex(x => x == id);
            Colors[i] = color;
        }

        public GradientStop GetGradientStop(int i)
        {
            return new GradientStop(Colors[i], Values[i]);
        }

        public GradientStopCollection GetGradientStopCollection()
        {
            var gsc = new GradientStopCollection {new GradientStop(StartColor, 0.0)};
            for (var i = 0; i < Length; i++) gsc.Add(GetGradientStop(i));
            gsc.Add(new GradientStop(EndColor, 1.0));
            return gsc;
        }

        public GradientModel Copy()
        {
            return new GradientModel(StartColor, EndColor, Colors, Values);
        }

        // public GradientPackModel Pack()
        // {
        //     var packLength = Length + 2;
        //     var pack = new GradientPackModel
        //     {
        //         Colors = new Color[packLength],
        //         Values = new double[packLength]
        //     };
        //     pack.Colors[0] = StartColor;
        //     pack.Colors[packLength - 1] = EndColor;
        //     pack.Values[0] = 0.0;
        //     pack.Values[packLength - 1] = 1.0;
        //     for (var i = 0; i < Length; i++)
        //     {
        //         pack.Colors[i + 1] = Colors[i];
        //         pack.Values[i + 1] = Values[i];
        //     }
        //
        //     return pack;
        // }
    }
}