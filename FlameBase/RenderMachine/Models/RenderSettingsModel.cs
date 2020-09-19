using static FlameBase.RenderMachine.Models.RenderColorModeModel;

namespace FlameBase.RenderMachine.Models
{
    public class RenderSettingsModel
    {
        public RenderSettingsModel(int iterations = 20000, int renderPerIterations = 1000,
            int shotsPerIteration = 64000,
            int draftImageSideWidth = 700)
        {
            Iterations = iterations;
            ShotsPerIteration = shotsPerIteration;
            RenderPerIterations = renderPerIterations;
            DraftImageSideWidth = draftImageSideWidth;
            RenderColorMode = RenderColorMode.Hsb;
        }

        public int Iterations { get; set; }
        public int ShotsPerIteration { get; set; }
        public int RenderPerIterations { get; set; }
        public int DraftImageSideWidth { get; set; }
        public RenderColorMode RenderColorMode { get; set; }
        public bool RenderByQuality { get; set; }
        public double Quality { get; set; } = 50.0;
    }
}