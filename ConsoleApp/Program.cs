using System;
using System.Windows.Media;
using FlameBase.RenderMachine.Models;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var r = new Random(123);

            for (var i = 0; i < 10; i++)
            {
                var bytes = new byte[6];
                r.NextBytes(bytes);
                var color1 = Color.FromRgb(bytes[0], bytes[1], bytes[2]);
                var color2 = Color.FromRgb(bytes[3], bytes[4], bytes[5]);

                var additiveMix = ColorToString(MixColorModel.AdditiveMix(new[] {color1, color2}));
                var subtractiveMix = ColorToString(MixColorModel.SubtractiveMix(new[] {color1, color2}));
                var dilutingSubtractiveMix =
                    ColorToString(MixColorModel.DilutingSubtractiveMix(new[] {color1, color2}));
                var mixC = ColorToString(MixColorModel.MixC(color1, color2));
                var rgb = ColorToString(MixColorModel.MixRgb(color1, color2));

                Console.WriteLine($"{ColorToString(color1)} + {ColorToString(color2)}");
                Console.WriteLine($"{additiveMix} additiveMix");
                Console.WriteLine($"{subtractiveMix} subtractiveMix");
                Console.WriteLine($"{dilutingSubtractiveMix} dilutingSubtractiveMix");
                Console.WriteLine($"{mixC} mixC");
                Console.WriteLine($"{rgb} rgb");
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static string ColorToString(Color c)
        {
            return $"{c.R:000} {c.G:000} {c.B:000}";
        }
    }
}