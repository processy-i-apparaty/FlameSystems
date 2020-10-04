using System.Windows;
using System.Windows.Media;
using FlameSystems.Controls.Views.Trans;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    public class TransformFinalViewModel : TransformViewModelBase
    {
        [ValueBind(1.0, 1.0, 1.0)]
        public override double Weight
        {
            get => Get();
            set => Set(value);
        }

        public override void SetFromCoefficients(double[] coefficients, double probability, Color color, bool modelIsFinal,
            double colorPosition)
        {
            
        }

        [ValueBind(Visibility.Collapsed)]
        public override Visibility WeightVisibility
        {
            get => Get();
            set => Set(value);
        }

        [ValueBind(Visibility.Collapsed)]
        public override Visibility VisibilityColorSelector
        {
            get => Get();
            set => Set(value);
        }

        public override bool IsFinal => true;
    }
}