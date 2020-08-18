using FlameBase.Enums;
using FlameBase.Models;

namespace FlameBase.RenderMachine.Models
{
    public class RenderPackModel
    {
        public RenderPackModel(TransformModel[] transformations, VariationModel[] variations,
            ViewSettingsModel viewSettings, RenderSettingsModel renderSettings, FlameColorMode colorMode, GradientModel gradModel)
        {
            Transformations = transformations;
            Variations = variations;
            ViewSettings = viewSettings;
            RenderSettings = renderSettings;
            ColorMode = colorMode;
            if (gradModel != null) GradModelCopy = gradModel.Copy();
        }

        public RenderSettingsModel RenderSettings { get; set; }

        public ViewSettingsModel ViewSettings { get; set; }

        public VariationModel[] Variations { get; set; }

        public TransformModel[] Transformations { get; set; }
        public GradientModel GradModelCopy { get; set; }
        public FlameColorMode ColorMode { get; set; }
    }
}