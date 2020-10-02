using FlameSystems.Compon.Base;

namespace FlameSystems.Views
{
    /// <summary>
    ///     Interaction logic for FalamePanelsView.xaml
    /// </summary>
    public partial class FalamePanelsView : IComponentView
    {
        public FalamePanelsView()
        {
            InitializeComponent();
        }

        public IComponentViewModel ViewModel
        {
            get => (IComponentViewModel) DataContext;
            set => DataContext = value;
        }
    }
}