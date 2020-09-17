using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using ConsoleWpfApp.FlameMath;
using Encog.Neural.Networks;
using Encog.Persist;

namespace ConsoleWpfApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BasicNetwork _network;
        private readonly Random r;

        public MainWindow()
        {
            InitializeComponent();

            var networkFile = new FileInfo(@"network22.eg");
            _network = (BasicNetwork) EncogDirectoryPersistence.LoadObject(networkFile);

            r = new Random(123);
            Next();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void Next()
        {
            for (var i = 0; i < 10; i++)
            {
                var color1 = RandomColor();
                var color2 = RandomColor();

                // var additiveMix = MixColorModel.AdditiveMix(new[] {color1, color2});
                // var subtractiveMix = MixColorModel.SubtractiveMix(new[] {color1, color2});
                // var dilutingSubtractiveMix =
                //     MixColorModel.DilutingSubtractiveMix(new[] {color1, color2});
                // var mixC = MixColorModel.MixC(color1, color2);
                // var rgb = MixColorModel.MixRgb(color1, color2);
                //
                // var km = new MixColorModel.KmColor(color1);
                // km.Mix(color2);
                //
                // var output = new double[3];
                // _network.Compute(ColorPairToDouble(color1, color2), output);
                //
                // var netC = DoubleToColor2(output);
                //
                //
                // Color1.Fill = ColorToBrush(color1);
                // Color2.Fill = ColorToBrush(color2);
                // // M1.Fill = ColorToBrush(additiveMix);
                // M2.Fill = ColorToBrush(netC);
                // M3.Fill = ColorToBrush(km.GetColor());
                // M4.Fill = ColorToBrush(mixC);
                // M5.Fill = ColorToBrush(rgb);
            }
        }

        private Color RandomColor()
        {
            return Color.FromRgb((byte) r.Next(256), (byte) r.Next(256), (byte) r.Next(256));
        }

        private static Brush ColorToBrush(Color color)
        {
            return new SolidColorBrush(color);
        }

        private static double[] ColorPairToDouble(Color c1, Color c2)
        {
            return new[] {c1.R / 255.0, c1.G / 255.0, c1.B / 255.0, c2.R / 255.0, c2.G / 255.0, c2.B / 255.0};
        }

        private static Color DoubleToColor(double[] d)
        {
            return Color.FromRgb((byte) (d[0] * 255.0), (byte) (d[1] * 255.0), (byte) (d[2] * 255.0));
        }

        public static Color DoubleToColor2(IReadOnlyList<double> d)
        {
            var r = (byte) Algebra.Map(d[0], -1.0, 1.0, 0.0, 255.0);
            var g = (byte) Algebra.Map(d[1], -1.0, 1.0, 0.0, 255.0);
            var b = (byte) Algebra.Map(d[2], -1.0, 1.0, 0.0, 255.0);
            return Color.FromRgb(r, g, b);
        }

        private static Color DoubleToColor2(double[] d)
        {
            return Color.FromRgb((byte) (d[0] * 255.0), (byte) (d[1] * 255.0), (byte) (d[2] * 255.0));
        }
    }
}