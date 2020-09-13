namespace FlameBase.RenderMachine.Models
{
    public class RenderSettingsModel
    {
        public RenderSettingsModel(int iterations = 20000, int renderPerIterations = 1000, int shotsPerIteration = 64000,
            int draftImageSideWidth = 700)
        {
            Iterations = iterations;
            ShotsPerIteration = shotsPerIteration;
            RenderPerIterations = renderPerIterations;
            DraftImageSideWidth = draftImageSideWidth;
            RenderColorMode = RenderColorModeModel.RenderColorMode.Hsb;
        }

        public int Iterations { get; set; }
        public int ShotsPerIteration { get; set; }
        public int RenderPerIterations { get; set; }
        public int DraftImageSideWidth { get; set; }
        public RenderColorModeModel.RenderColorMode RenderColorMode { get; set; }
    }
}