using System;
using System.Windows.Controls;
using System.Windows.Media;
using ConsoleWpfApp.Pickers.ViewModels;

namespace ConsoleWpfApp.Pickers.Views
{
    /// <summary>
    ///     Interaction logic for ColorPickView.xaml
    /// </summary>
    public partial class ColorPickView : UserControl
    {
        public ColorPickView()
        {
            InitializeComponent();
            DataContext = new ColorPickViewModel(Colors.Gray, null);
        }

        public ColorPickView(Color color, Action<bool, Color> callback)
        {
            InitializeComponent();
            DataContext = new ColorPickViewModel(color, callback);
        }
    }
}