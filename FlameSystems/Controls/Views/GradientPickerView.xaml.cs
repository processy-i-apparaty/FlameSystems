using FlameBase.Models;
using FlameSystems.Controls.ViewModels;

namespace FlameSystems.Controls.Views
{
    /// <summary>
    ///     Interaction logic for GradientPickerView.xaml
    /// </summary>
    public partial class GradientPickerView
    {
        public GradientPickerView()
        {
            InitializeComponent();
            DataContext = new GradientPickerViewModel();
        }

        public GradientPickerView(GradientModel gradientModel, double position)
        {
            InitializeComponent();
            DataContext = new GradientPickerViewModel(gradientModel, position);
        }

        public GradientPickerView(GradientModel gradientModel)
        {
            InitializeComponent();
            DataContext = new GradientPickerViewModel(gradientModel);
        }
    }
}