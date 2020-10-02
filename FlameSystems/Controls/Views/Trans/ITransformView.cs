using FlameSystems.Controls.ViewModels;

namespace FlameSystems.Controls.Views.Trans
{
    internal interface IView
    {
        TransformViewModelBase DataContex { get; set; }
    }
}