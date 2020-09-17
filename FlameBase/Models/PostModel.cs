using System.Windows.Media;

namespace FlameBase.Models
{
    public class PostModel
    {
        public GradientModel GradientModel { get; set; }
        public double[] GradientValues { get; set; }
        public Color[] TransformColors { get; set; }
        public Color BackColor { get; set; }
        public uint[,,] DisplayArray { get; set; }

        public PostModel()
        {
            GradientModel = new GradientModel(Colors.Gray, Colors.Gray);
            GradientValues = new double[0];
            TransformColors = new Color[0];
            BackColor = Colors.Black;
        }
    }
}