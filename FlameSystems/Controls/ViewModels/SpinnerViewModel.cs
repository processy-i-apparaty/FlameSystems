using FlameSystems.Infrastructure;
using FlameSystems.Infrastructure.ValueBind;

namespace FlameSystems.Controls.ViewModels
{
    internal class SpinnerViewModel : Notifier
    {
        [ValueBind]
        public string Text
        {
            get => Get();
            set => Set(value);
        }

        public SpinnerViewModel()
        {
            Text = "wait...";
        }
    }
}