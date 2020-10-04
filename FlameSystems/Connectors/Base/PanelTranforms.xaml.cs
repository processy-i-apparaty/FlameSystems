namespace FlameSystems.Connectors.Base
{
    /// <summary>
    ///     Interaction logic for PanelTranforms.xaml
    /// </summary>
    public partial class PanelTranforms : IControlV
    {
        public PanelTranforms()
        {
            InitializeComponent();
        }

        public IControlVm ViewModel
        {
            get => (IControlVm) DataContext;
            set => DataContext = value;
        }
    }
}