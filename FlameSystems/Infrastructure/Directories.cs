using System;

namespace FlameSystems.Infrastructure
{
    public static class Directories
    {
        private static readonly string CurrentDirectory = Environment.CurrentDirectory;
        public static string Flames { get; } = $"{CurrentDirectory}\\flames";
        public static string Images { get; } = $"{CurrentDirectory}\\images";
        public static string Renders { get; } = $"{CurrentDirectory}\\renders";
        public static string[] GetAll => new[] {Flames, Images, Renders};
    }
}