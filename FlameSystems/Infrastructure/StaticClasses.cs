using FlameSystems.Controls.ViewModels;
using FlameSystems.Controls.Views;

namespace FlameSystems.Infrastructure
{
    internal static class StaticClasses
    {
        private static readonly SpinnerViewModel SpinnerVm;

        static StaticClasses()
        {
            Spinner = new SpinnerView();
            SpinnerVm = (SpinnerViewModel) Spinner.DataContext;
        }

        public static SpinnerView Spinner { get; }

        public static void SetSpinnerText(string text)
        {
            SpinnerVm.Text = text;
        }
    }
}