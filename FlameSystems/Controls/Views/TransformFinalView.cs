using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views.Trans;

namespace FlameSystems.Controls.Views
{
    public class FinalView : TransfView
    {
        public FinalView()
        {
            DataContex = new TransformFinalViewModel();
        }
    }
}