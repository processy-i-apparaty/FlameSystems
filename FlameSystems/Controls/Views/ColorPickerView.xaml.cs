using System.Windows.Media;
using FlameSystems.Controls.ViewModels;

namespace FlameSystems.Controls.Views
{
    /// <summary>
    ///     Interaction logic for ColorPickerView.xaml
    /// </summary>
    public partial class ColorPickerView
    {
        public ColorPickerView(Color color)
        {
            InitializeComponent();
            DataContext = new ColorPickerViewModel(color);
        }

        public ColorPickerView()
        {
            InitializeComponent();
            DataContext = new ColorPickerViewModel(Colors.Gray);
        }
    }
}