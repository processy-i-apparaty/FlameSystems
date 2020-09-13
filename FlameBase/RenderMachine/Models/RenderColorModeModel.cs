using System;

namespace FlameBase.RenderMachine.Models
{
    public class RenderColorModeModel
    {
        public enum RenderColorMode
        {
            Hsb,
            Lab,
            LogGamma
        }


        public bool ModeHsb { get; set; } = true;
        public bool ModeLab { get; set; }
        public bool ModeLogGamma { get; set; }

        public RenderColorMode Mode
        {
            get
            {
                if (ModeHsb) return RenderColorMode.Hsb;
                if (ModeLab) return RenderColorMode.Lab;
                if (ModeLogGamma) return RenderColorMode.LogGamma;
                throw new ArgumentException("ModeError");
            }
        }

        public void SetMode(RenderColorMode renderColorMode)
        {
            ModeHsb = false;
            ModeLab = false;
            ModeLogGamma = false;

            switch (renderColorMode)
            {
                case RenderColorMode.Hsb:
                    ModeHsb = true;
                    break;
                case RenderColorMode.Lab:
                    ModeLab = true;
                    break;
                case RenderColorMode.LogGamma:
                    ModeLogGamma = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(renderColorMode), renderColorMode, null);
            }
        }
    }
}