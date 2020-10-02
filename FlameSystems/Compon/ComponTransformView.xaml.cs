using FlameSystems.Compon.Base;

namespace FlameSystems.Compon
{
    /// <summary>
    ///     Interaction logic for ComponTransformView.xaml
    /// </summary>
    public partial class ComponTransformView : IComponentView
    {
        public ComponTransformView()
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