using System.Windows;

namespace FlameBase.RenderMachine.Models
{
    internal class RenderSequenceModel
    {
        public int TotalIterations { get; set; }
        public int Cores { get; set; }
        public IteratorModel[] Iterators { get; set; }
        public int TotalPointsPerCore { get; set; }
        public int Symmetry { get; set; }
        public double[,] TranslationArray { get; set; }
        public Point CenterPoint { get; set; }
        public Size ImageSize { get; set; }
        public LogDisplayModel Display { get; set; }
        public RenderActionsModel RenderActionsPack { get; set; }
        public int RenderId { get; set; }
        public bool DraftMode { get; set; }
        public RenderPackModel RenderPack { get; set; }
        public bool IsDrawingIntermediate { get; set; }
        public double SectorCos { get; set; }
        public double SectorSin { get; set; }
        public int Iteration { get; set; }
    }
}