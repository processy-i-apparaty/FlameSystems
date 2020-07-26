using System.Windows.Controls;
using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;
using FlameSystems.Views;

namespace FlameSystems.ViewModels
{
    internal class MainWindowViewModel : Notifier
    {
        [ValueBind]
        public Control WindowContent
        {
            get => Get();
            set => Set(value);
        }

        public MainWindowViewModel()
        {
            WindowContent = new CreateFlameView();
        }
    }
}