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

        public GradientPickerView(GradientModel gradientModel, double position, string callbackString1 = "",
            string callbackString2 = "")
        {
            InitializeComponent();
            DataContext = new GradientPickerViewModel(gradientModel, position, callbackString1, callbackString2);
        }

        public GradientPickerView(GradientModel gradientModel, string callbackString1 = "", string callbackString2 = "")
        {
            InitializeComponent();
            DataContext = new GradientPickerViewModel(gradientModel, callbackString1, callbackString2);
        }
    }
}