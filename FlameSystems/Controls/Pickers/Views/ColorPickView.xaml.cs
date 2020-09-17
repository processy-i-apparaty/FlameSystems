using System;
using System.Windows.Controls;
using System.Windows.Media;
using FlameSystems.Controls.Pickers.ViewModels;

namespace FlameSystems.Controls.Pickers.Views
{
    /// <summary>
    ///     Interaction logic for ColorPickView.xaml
    /// </summary>
    public partial class ColorPickView
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