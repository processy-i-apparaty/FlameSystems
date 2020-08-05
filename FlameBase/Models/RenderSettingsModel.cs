namespace FlameBase.Models
{
    public class RenderSettingsModel
    {
        public RenderSettingsModel(int iterations = 1000, int renderPerIterations = 100, int shotsPerIteration = 64000,
            int draftImageSideWidth = 700)
        {
            Iterations = iterations;
            ShotsPerIteration = shotsPerIteration;
            RenderPerIterations = renderPerIterations;
            DraftImageSideWidth = draftImageSideWidth;
        }

        public int Iterations { get; set; }
        public int ShotsPerIteration { get; set; }
        public int RenderPerIterations { get; set; }
        public int DraftImageSideWidth { get; set; }
    }
}