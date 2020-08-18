using System.Collections.Generic;
using System.Windows.Media;

namespace FlameBase.Models
{
    public class FlameModel
    {
        public List<double[]> Coefficients { get; set; }
        public List<double[]> Parameters { get; set; }
        public List<int> VariationIds { get; set; }
        public List<Color> FunctionColors { get; set; }
        public double ViewShiftX { get; set; }
        public double ViewShiftY { get; set; }
        public double ViewZoom { get; set; }
        public double Rotation { get; set; }
        public int Symmetry { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public List<double> Weights { get; set; }
        public List<double> FunctionColorPositions { get; set; }
        public GradientPackModel GradientPack { get; set; }
        public Color BackColor { get; set; } = Colors.Black;
    }
}