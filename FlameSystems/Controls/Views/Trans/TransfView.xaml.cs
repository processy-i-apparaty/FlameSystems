using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlameSystems.Controls.ViewModels;

namespace FlameSystems.Controls.Views.Trans
{
    /// <summary>
    /// Interaction logic for TransfView.xaml
    /// </summary>
    public partial class TransfView : IView
    {
        public TransfView()
        {
            InitializeComponent();
        }

        public TransformViewModelBase DataContex
        {
            get => (TransformViewModelBase) DataContext;
            set => DataContext = value;
        }
    }
}