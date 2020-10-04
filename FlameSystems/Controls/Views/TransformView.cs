using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views.Trans;

namespace FlameSystems.Controls.Views
{
    public class TransformView : TransfView
    {
        public TransformView()
        {
            var viewModel = new TransformViewModel();
            
            DataContex = viewModel;

        }

    }
}