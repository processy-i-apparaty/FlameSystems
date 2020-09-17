using System;
using System.Windows.Controls;
using FlameSystems.Controls.Pickers.Enums;
using FlameSystems.Controls.Pickers.Models;
using FlameSystems.Controls.Pickers.ViewModels;

namespace FlameSystems.Controls.Pickers.Views
{
    /// <summary>
    ///     Interaction logic for GradientPickView.xaml
    /// </summary>
    public partial class GradientPickView : UserControl
    {
        public GradientPickView()
        {
            InitializeComponent();
            DataContext = new GradientPickViewModel();
        }

        public GradientPickView(GradientModel gradientModel,
            Action<GradientCallbackType, object, double> gradientPickCallback)
        {
            InitializeComponent();
            DataContext = new GradientPickViewModel(gradientModel, gradientPickCallback);
        }

        public GradientPickView(GradientModel gradientModel, double gradientValue,
            Action<GradientCallbackType, object, double> gradientPickCallback)
        {
            InitializeComponent();
            DataContext = new GradientPickViewModel(gradientModel, gradientValue, gradientPickCallback);
        }
    }
}