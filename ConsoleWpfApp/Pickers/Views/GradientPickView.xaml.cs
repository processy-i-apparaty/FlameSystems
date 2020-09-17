using System;
using ConsoleWpfApp.Pickers.Enums;
using ConsoleWpfApp.Pickers.Models;
using ConsoleWpfApp.Pickers.ViewModels;

namespace ConsoleWpfApp.Pickers.Views
{
    /// <summary>
    ///     Interaction logic for GradientPickView.xaml
    /// </summary>
    public partial class GradientPickView
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